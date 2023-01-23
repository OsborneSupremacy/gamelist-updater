using GameXmlReader.Models;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace GameXmlReader.Services;

public class ExecutorService
{
    private readonly ILogger<ExecutorService> _logger;

    private readonly GameXmlService _gameXmlService;

    private readonly Settings _settings;

    public ExecutorService(ILogger<ExecutorService> logger, GameXmlService gameXmlService, Settings settings)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _gameXmlService = gameXmlService ?? throw new ArgumentNullException(nameof(gameXmlService));
        _settings = settings ?? throw new ArgumentNullException(nameof(settings));
    }

    public Task<bool> ExecuteAsync(CancellationToken cancellationToken)
    {
        _logger.LogInformation("Target directory: {targetDirectory}", _settings.Target.Directory);
        _logger.LogInformation("Target GameList: {targerGameList}", _settings.Target.Xml);

        _logger.LogInformation("Deserializing game list...");

        var gameList = _gameXmlService.DeserializeXml(
            Utility.ReadFileWithoutLocking(_settings.Target.Xml)
        );

        _logger.LogInformation("Done. {gameCount} games found in XML.", gameList.Games.Count());

        _logger.LogInformation("Flagged genres: {flaggedGenreCount} ", _settings.FlaggedTerms.Genres.Count);
        _logger.LogInformation("Flagged words: {flaggedWordCount} ", _settings.FlaggedTerms.Words.Count);
        _logger.LogInformation("Flagged publishers: {flaggedPublisherCount} ", _settings.FlaggedTerms.Words.Count);

        if (!PromptForVerification("Proceed with scan? This will look for flagged terms in game list."))
            return Task.FromResult(false);

        List<Game> flaggedGames = new();

        List<Func<Game, bool>> _flagChecks = new()
        {
            HasFlaggedGenre,
            HasFlaggedWord,
            HasFlaggedPublisher,
        };

        foreach(var game in gameList.Games)
            foreach(var flagCheck in _flagChecks)
                if (flagCheck(game))
                {
                    flaggedGames.Add(game);
                    continue;
                }

        if(!flaggedGames.Any())
        {
            _logger.LogInformation("No games with flagged terms found.");
            return Task.FromResult(true);
        }

        _logger.LogInformation("{flaggedGamesCount} games with flagged terms found.", flaggedGames.Count);


        return Task.FromResult(true);
    }


    public bool HasFlaggedGenre(Game game)
    {
        foreach (var flaggedGenre in _settings.FlaggedTerms.Genres)
            if ((game.Genre ?? string.Empty).Contains(flaggedGenre, Str1ingComparison.OrdinalIgnoreCase))
                return true;
        return false;
    }

    public bool HasFlaggedWord(Game game)
    {
        foreach (var word in _settings.FlaggedTerms.Words)
            if ((game.Desc ?? string.Empty).Contains(word, StringComparison.OrdinalIgnoreCase))
                return true;
        return false;
    }

    public bool HasFlaggedPublisher(Game game)
    {
        foreach (var publisher in _settings.FlaggedTerms.Publishers)
            if ((game.Publisher ?? string.Empty).Contains(publisher, StringComparison.OrdinalIgnoreCase))
                return true;
        return false;
    }

    public bool PromptForVerification(string prompt)
    {
        bool? result = null;
        while(!result.HasValue)
        {
            _logger.LogInformation($"{prompt} (1, Y = Yes, 0, 2, N = No): ");
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
