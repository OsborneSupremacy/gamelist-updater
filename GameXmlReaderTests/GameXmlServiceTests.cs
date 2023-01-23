namespace GameXmlReaderTests;

using System;
using System.IO;
using System.Reflection;
using FluentAssertions;
using GameXmlReader.Models;
using GameXmlReader.Services;
using Xunit;

public class GameXmlServiceTests
{
    private readonly GameXmlService _service = new GameXmlService();

    protected string GetSampleInput(string fileName)
    {
        var appDir = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);

        return File.ReadAllText(
            Path.Combine(appDir, "Sample", fileName)
        );
    }

    [Fact]
    public void DeserializeXml_ShouldDeserializeXMLToGameList()
    {
        // Act
        var result = _service
            .DeserializeXml(GetSampleInput("sample-gamelist-01.xml"));

        // Assert
        result.Should().BeOfType<GameList>();
        result.Provider.System.Should().Be("Mame");
        result.Games.Should().HaveCount(2);
        result.Games[0].Id.Should().Be("39869");
    }

    [Fact]
    public void SerializeXml_Should_SerializeGameListToXml()
    {
        // Arrange
        var sampleInput = GetSampleInput("sample-gamelist-02.xml");

        var gameList = _service.DeserializeXml(sampleInput);

        // Act
        var result = _service.SerializeXml(gameList)
            .TrimStart('\uFEFF'); // removes the BOM

        // Assert

        // can't just compare strings as they are, because 
        // of difference between tabs and spaces
        using StringReader srSample = new(sampleInput);
        using StringReader srResult = new(result);

        while (true)
        {
            string sampleLine = srSample.ReadLine();
            string resultLine = srResult.ReadLine();

            if (sampleLine == null && resultLine == null)
                break;

            sampleLine.Should().Be(resultLine);
        }
    }
}