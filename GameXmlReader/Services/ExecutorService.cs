namespace GameXmlReader.Services;

public class ExecutorService
{
    private readonly ILogger<ExecutorService> _logger;

    private readonly GameXmlService _gameXmlService;

    private readonly GameScanner _gameScanner;

    private readonly GameListService _gameListService;

    private readonly FileSystemService _fileSystemService;

    private readonly Settings _settings;

    public ExecutorService(ILogger<ExecutorService> logger, GameXmlService gameXmlService, GameScanner gameScanner, GameListService gameListService, FileSystemService fileSystemService, Settings settings)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _gameXmlService = gameXmlService ?? throw new ArgumentNullException(nameof(gameXmlService));
        _gameScanner = gameScanner ?? throw new ArgumentNullException(nameof(gameScanner));
        _gameListService = gameListService ?? throw new ArgumentNullException(nameof(gameListService));
        _fileSystemService = fileSystemService ?? throw new ArgumentNullException(nameof(fileSystemService));
        _settings = settings ?? throw new ArgumentNullException(nameof(settings));
    }

    public async Task<bool> ExecuteAsync(CancellationToken cancellationToken)
    {
        var tempPath = Path.Combine(
            Path.GetTempPath(),
            "GameXmlUpdater",
            Guid.NewGuid().ToString()
        );

        _logger.LogInformation("Target directory: {targetDirectory}", _settings.Target.Directory);
        _logger.LogInformation("Target GameList: {targerGameList}", _settings.Target.Xml);

        _logger.LogInformation("Deserializing game list...");

        var currentGameListRaw = Utility.ReadFileWithoutLocking(_settings.Target.Xml);
        var gameList = _gameXmlService.DeserializeXml(currentGameListRaw);

        _logger.LogInformation("Done. {gameCount} games found in XML.", gameList.Games.Count());

        if (_settings.FlagUnidentifiedGames ?? false)
            _logger.LogWarning("Unidentified games will be flagged!");

        if (_settings.FlagWhenAdultFieldIsTrue ?? false)
            _logger.LogInformation("Games with <adult>true</adult> will be flagged");
        else
            _logger.LogInformation("Games with <adult>true</adult> will *not* be flagged");

        _logger.LogInformation("Flagged genres: {flaggedGenreCount} ", _settings.FlaggedTerms.Genres.Count);
        _logger.LogInformation("Flagged words: {flaggedWordCount} ", _settings.FlaggedTerms.Words.Count);
        _logger.LogInformation("Flagged publishers: {flaggedPublisherCount} ", _settings.FlaggedTerms.Words.Count);

        if (!PromptForVerification("Proceed with scan? This will look for flagged terms in game list."))
            return false;

        var flaggedGames = _gameScanner.ScanForFlaggedGames(gameList.Games).ToList();

        if(!flaggedGames.Any())
        {
            _logger.LogInformation("No games with flagged terms found.");
            return true;
        }

        _logger.LogInformation("{flaggedGamesCount} games with flagged terms found.", flaggedGames.Count);

        var newGameList = _gameListService.RemoveFlaggedGames(gameList, flaggedGames);
        var flaggedGameList = _gameListService.GetFlaggedGamesOnly(gameList, flaggedGames);

        await _fileSystemService.WriteNewGameListToTempAsync(
            tempPath,
            currentGameListRaw,
            newGameList,
            flaggedGameList
            );

        if (!PromptForVerification("Proceed with getting all files that will be deleted? This includes the actual game files, plus images, marquees, and videos."))
            return false;

        var filesToDelete = _gameListService.GetFilesToDelete(flaggedGames).ToList();

        _logger.LogInformation("{filesToDelete} files will be deleted.", filesToDelete.Count);

        if (!PromptForVerification("Proceed with deleting all files, and updating the game list XML? This cannot be undone.", LogLevel.Warning))
            return false;

        _fileSystemService.DeleteFiles(filesToDelete);
        await _fileSystemService.WriteNewGameListToProductionAsync(newGameList);

        _logger.LogInformation("Success");

        return true;
    }

    public bool PromptForVerification(string prompt, LogLevel logLevel = LogLevel.Information)
    {
        bool? result = null;
        while(!result.HasValue)
        {
            _logger.Log(logLevel, $"{prompt} (1, Y = Yes, 0, 2, N = No): ");
            result = Console.ReadKey().KeyChar.ToString().ToUpperInvariant() switch
            {
                "1" => true,
                "Y" => true,
                "0" => false,
                "2" => false,
                "N" => false,
                _ => null
            };
            Console.WriteLine();
        }
        return result.Value;
    }
}
