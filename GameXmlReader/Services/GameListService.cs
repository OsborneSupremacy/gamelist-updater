namespace GameXmlReader.Services;

public class GameListService
{
    private readonly Settings _settings;

    public GameListService(Settings settings)
    {
        _settings = settings ?? throw new ArgumentNullException(nameof(settings));
    }

    public GameList RemoveFlaggedGames(GameList gameListIn, List<Game> flaggedGames)
    {
        var flaggedPaths = flaggedGames
            .Select(x => x.Path);

        var validGames = gameListIn
            .Games
            .Where(x => !flaggedPaths.Contains(x.Path))
            .ToList();

        return gameListIn with
        {
            Games = validGames
        };
    }

    public GameList GetFlaggedGamesOnly(GameList gameListIn, List<Game> flaggedGames)
    {
        var flaggedPaths = flaggedGames
            .Select(x => x.Path);

        var validGames = gameListIn
            .Games
            .Where(x => flaggedPaths.Contains(x.Path));

        return gameListIn with
        {
            Games = validGames.ToList()
        };
    }

    public IEnumerable<string> GetFilesToDelete(List<Game> flaggedGames)
    {
        foreach(var file in GetFilesToDeleteFromGameList(flaggedGames)) 
            if(File.Exists(file))
                yield return file;
    }

    private IEnumerable<string> GetFilesToDeleteFromGameList(List<Game> flaggedGames)
    {
        foreach (var game in flaggedGames)
        {
            yield return Path.Combine(_settings.Target.Directory, game.Path.TrimStart('.').TrimStart('/'));

            if (!string.IsNullOrWhiteSpace(game.Image))
                yield return Path.Combine(_settings.Target.Directory, game.Image.TrimStart('.').TrimStart('/'));

            if (!string.IsNullOrWhiteSpace(game.Marquee))
                yield return Path.Combine(_settings.Target.Directory, game.Marquee.TrimStart('.').TrimStart('/'));

            if (!string.IsNullOrWhiteSpace(game.Video))
                yield return Path.Combine(_settings.Target.Directory, game.Video.TrimStart('.').TrimStart('/'));
        }
    } 
}
