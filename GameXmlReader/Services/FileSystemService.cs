using GameXmlReader.Services;
using System.IO.Abstractions;

public class FileSystemService
{
    private readonly IFileSystem _fileSystem;

    private readonly ILogger<FileSystemService> _logger;

    private readonly GameXmlService _gameXmlService;

    private readonly Settings _settings;

    public FileSystemService(IFileSystem fileSystem, ILogger<FileSystemService> logger, GameXmlService gameXmlService, Settings settings)
    {
        _fileSystem = fileSystem ?? throw new ArgumentNullException(nameof(fileSystem));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _gameXmlService = gameXmlService ?? throw new ArgumentNullException(nameof(gameXmlService));
        _settings = settings ?? throw new ArgumentNullException(nameof(settings));
    }

    public async Task WriteNewGameListToTempAsync(
        string tempPath,
        string currentRawGameListXml,
        GameList newGameList,
        GameList flaggedGamesOnly
        )
    {
        _fileSystem.Directory.CreateDirectory(tempPath);

        var newGameListXml = _gameXmlService.SerializeXml(newGameList);
        var flaggedGamesOnlyXml = _gameXmlService.SerializeXml(flaggedGamesOnly);

        await _fileSystem.File.WriteAllTextAsync(Path.Combine(tempPath, "gamelist-pre-update.xml"), currentRawGameListXml);
        await _fileSystem.File.WriteAllTextAsync(Path.Combine(tempPath, "gamelist-new.xml"), newGameListXml);
        await _fileSystem.File.WriteAllTextAsync(Path.Combine(tempPath, "gamelist-flagged.xml"), flaggedGamesOnlyXml);

        _logger.LogWarning("Game lists written to temporary folder, {tempPath}, for review. Please review these files before committing changes.", tempPath);

        return;
    }

    public void DeleteFiles(IEnumerable<string> filesToDelete)
    {
        foreach (var file in filesToDelete)
        {
            _logger.LogInformation("Deleting {file}", file);
            _fileSystem.File.Delete(file);
        }
    }

    public async Task WriteNewGameListToProductionAsync(GameList newGameList)
    {
        var newGameListXml = _gameXmlService.SerializeXml(newGameList);
        await _fileSystem.File.WriteAllTextAsync(_settings.Target.Xml, newGameListXml);
    }
}
