using GameXmlReader.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace GameXmlReader
{
    public class Program
    {
        const string _romRoot = @"\\retropie\roms";

        static void Main(string[] args)
        {
            var romDirs = Directory.GetDirectories(_romRoot);

            //foreach (var dir in romDirs)
            //{
            //    var xmlInfo = GetGameXml(dir);
            //    if (xmlInfo.Exists)
            //    {
            //        var gameList = DeserializeGameXml(dir, xmlInfo.FullName);
            //    }
            //}
        }

        public static List<Game> GetFullGameList(string rootDirectory)
        {
            var romDirs = Directory.GetDirectories(_romRoot);
            var gameList = new List<Game>();

            //foreach (var dir in romDirs)
            //{
            //    var xmlInfo = GetGameXml(dir);
            //    if (xmlInfo.Exists)
            //        gameList.AddRange(DeserializeGameXml(dir, xmlInfo.FullName));
            //}

            return gameList;
        }

        //public static List<Game> DeserializeGameXml(string parentDirectory, string xmlFileName)
        //{
        //    var rawXml = File.ReadAllText(xmlFileName);
        //    var gameList = new List<Game>();

        //    var xDoc = XDocument.Parse(rawXml);
        //    var xGameList = xDoc.Descendants("gameList").Single();
        //    IEnumerable<XElement> xGames = xGameList.Descendants("game");

        //    foreach(var xGame in xGames)
        //    {
        //        gameList.Add(new Game()
        //        {
        //            fullPath = Path.Combine(parentDirectory, IsolateFileName(xGame.Element("path").Value.ToString())),
        //            path = xGame.Element("path").Value,
        //            name = xGame.Element("name").Value,
        //            cover = xGame.Element("cover")?.Value ?? string.Empty,
        //            image = xGame.Element("image")?.Value ?? string.Empty,
        //            margquee = xGame.Element("marquee")?.Value ?? string.Empty,
        //            video = xGame.Element("video")?.Value ?? string.Empty,
        //            rating = StringToNullableDouble(xGame.Element("rating")?.Value ?? string.Empty),
        //            desc = xGame.Element("desc")?.Value ?? string.Empty,
        //            // date parsing not working.  Look into it later, if dates are needed for anything
        //            releaseDate = StringToNullableDateTimeOffset(xGame.Element("releasedate")?.Value ?? string.Empty),
        //            developer = xGame.Element("developer")?.Value ?? string.Empty,
        //            publisher = xGame.Element("publisher")?.Value ?? string.Empty,
        //            genre = xGame.Element("genre")?.Value ?? string.Empty,
        //            players = StringToNullableInt(xGame.Element("players")?.Value ?? string.Empty)
        //        });
        //    };

        //    return gameList;
        //}

        protected static string IsolateFileName(string input)
        {
            if (input.Contains('/'))
                return input[(input.LastIndexOf(@"/") + 1)..];

            if (input.Contains('\\'))
                return input[(input.LastIndexOf(@"\") + 1)..];

            return input;
        }

        //public static List<Game> IdentifyUndesirableGames(
        //    List<Game> sourceList,
        //    List<string> genreKeyWords, 
        //    List<string> descKeyWords,
        //    List<string> publisherKeyWords
        //    )
        //{
        //    var outputLst = new List<Game>();

        //    foreach(var game in sourceList)
        //    {
        //        bool flagged = false;
        //        foreach(var genre in genreKeyWords)
        //            if (game.genre.Contains(genre, StringComparison.OrdinalIgnoreCase))
        //            {
        //                flagged = true;
        //                break;
        //            }
        //        if (!flagged)
        //            foreach(var word in descKeyWords)
        //                if (game.desc.Contains(word, StringComparison.OrdinalIgnoreCase))
        //                {
        //                    flagged = true;
        //                    break;
        //                }
        //        if (!flagged)
        //            foreach (var word in publisherKeyWords)
        //                if (game.publisher.Contains(word, StringComparison.OrdinalIgnoreCase))
        //                {
        //                    flagged = true;
        //                    break;
        //                }
        //        if (flagged)
        //            if (File.Exists(game.fullPath))
        //                outputLst.Add(game);
        //    };

        //    return outputLst;
        //}
        
        public static void DeleteGames(List<string> gamePaths)
        {
            foreach(var game in gamePaths)
                if (new FileInfo(game).Exists)
                    File.Delete(game);
        }

        public static FileInfo GetGameXml(string directory) =>
            new FileInfo(Path.Combine(directory, "gamelist.xml"));

        public static int? StringToNullableInt(string input)
        {
            if (string.IsNullOrEmpty(input)) return new int?();
            if (int.TryParse(input, out int intVal))
                return intVal;
            else
                return new int?();
        }

        public static double? StringToNullableDouble(string input)
        {
            if (string.IsNullOrEmpty(input)) return new double?();
            if (double.TryParse(input, out double val))
                return val;
            else
                return new double?();
        }

        public static DateTimeOffset? StringToNullableDateTimeOffset(string input)
        {
            if (string.IsNullOrEmpty(input)) return new DateTimeOffset?();
            if (DateTimeOffset.TryParse(input, out DateTimeOffset dtVal))
                return dtVal;
            else
                return new DateTimeOffset?();
        }

    }
}
