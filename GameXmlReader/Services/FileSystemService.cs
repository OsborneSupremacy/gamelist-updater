using GameXmlReader.Models;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameXmlReader.Services;

public class FileSystemService
{
    private readonly ILogger<FileSystemService> _logger;

    private readonly GameXmlService _gameXmlService;

    public FileSystemService(ILogger<FileSystemService> logger, GameXmlService gameXmlService)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _gameXmlService = gameXmlService ?? throw new ArgumentNullException(nameof(gameXmlService));
    }

    public async Task WriteNewGamesListToTempAsync(GameList newGameList, GameList flaggedGamesOnly)
    {
        var tempPath = Path.Combine(
            Path.GetTempPath(),
            "GameXmlUpdater",
            Guid.NewGuid().ToString()
        );

        var tempDir = new DirectoryInfo(tempPath);
        tempDir.Create();

        var newGameListXml = _gameXmlService.SerializeXml(newGameList);
        var flaggedGamesOnlyXml = _gameXmlService.SerializeXml(flaggedGamesOnly);

        await File.WriteAllTextAsync(Path.Combine(tempPath, "gamelist.xml"), newGameListXml);
        await File.WriteAllTextAsync(Path.Combine(tempPath, "gamelist-flagged.xml"), flaggedGamesOnlyXml);

        _logger.LogWarning("Game lists written to temporary folder, {tempPath}, for review. Please review these files before committing changes.", tempPath);

        return;
    }
}
