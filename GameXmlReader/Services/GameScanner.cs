namespace GameXmlReader.Services;

public class GameScanner
{
    private readonly Settings _settings;

    private List<Func<Game, bool>> _flagChecks;

    public GameScanner(Settings settings)
    {
        _settings = settings ?? throw new ArgumentNullException(nameof(settings));

        _flagChecks = new()
        {
            HasFlaggedName,
            HasFlaggedGenre,
            HasFlaggedWord,
            HasFlaggedPublisher,
        };
    }

    public IEnumerable<Game> ScanForFlaggedGames(IEnumerable<Game> games)
    {
        foreach (var game in games)
            foreach (var flagCheck in _flagChecks)
                if (flagCheck(game))
                {
                    yield return game;
                }
    }

    private bool HasFlaggedName(Game game)
    {
        foreach (var flaggedName in _settings.FlaggedTerms.Names)
            if (game.Name.Contains(flaggedName, StringComparison.OrdinalIgnoreCase))
                return true;
        return false;
    }

    private bool HasFlaggedGenre(Game game)
    {
        foreach (var flaggedGenre in _settings.FlaggedTerms.Genres)
            if ((game.Genre ?? string.Empty).Contains(flaggedGenre, StringComparison.OrdinalIgnoreCase))
                return true;
        return false;
    }

    private bool HasFlaggedWord(Game game)
    {
        foreach (var word in _settings.FlaggedTerms.Words)
            if ((game.Desc ?? string.Empty).Contains(word, StringComparison.OrdinalIgnoreCase))
                return true;
        return false;
    }

    private bool HasFlaggedPublisher(Game game)
    {
        foreach (var publisher in _settings.FlaggedTerms.Publishers)
            if ((game.Publisher ?? string.Empty).Contains(publisher, StringComparison.OrdinalIgnoreCase))
                return true;
        return false;
    }
}