using GameXmlReader.Models;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace GameXmlReader.Services;

public class FileSystemService
{
    private readonly ILogger<FileSystemService> _logger;

    private readonly GameXmlService _gameXmlService;

    private readonly Settings _settings;

    public FileSystemService(ILogger<FileSystemService> logger, GameXmlService gameXmlService, Settings settings)
    {
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
        var tempDir = new DirectoryInfo(tempPath);
        tempDir.Create();

        var newGameListXml = _gameXmlService.SerializeXml(newGameList);
        var flaggedGamesOnlyXml = _gameXmlService.SerializeXml(flaggedGamesOnly);

        await File.WriteAllTextAsync(Path.Combine(tempPath, "gamelist-pre-update.xml"), currentRawGameListXml);
        await File.WriteAllTextAsync(Path.Combine(tempPath, "gamelist-new.xml"), newGameListXml);
        await File.WriteAllTextAsync(Path.Combine(tempPath, "gamelist-flagged.xml"), flaggedGamesOnlyXml);

        _logger.LogWarning("Game lists written to temporary folder, {tempPath}, for review. Please review these files before committing changes.", tempPath);

        return;
    }

    public void DeleteFiles(IEnumerable<string> filesToDelete)
    {
        foreach(var file in filesToDelete)
        {
            _logger.LogInformation("Deleting {file}", file);
            File.Delete(file);
        }
    }

    public async Task WriteNewGameListToProductionAsync(GameList newGameList)
    {
        var newGameListXml = _gameXmlService.SerializeXml(newGameList);
        await File.WriteAllTextAsync(_settings.Target.Xml, newGameListXml);
    }
}
