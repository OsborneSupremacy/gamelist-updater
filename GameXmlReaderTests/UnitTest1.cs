//using NUnit.Framework;
//using GameXmlReader;
//using System.Collections.Generic;
//using System.Text;
//using System.Diagnostics;
//using System.Linq;

//namespace Tests
//{
//    public class Tests
//    {
//        List<string> _badGenres = null;
//        List<string> _badPublishers = null;
//        List<string> _badWords = null;

//        [SetUp]
//        public void Setup()
//        {
//            _badGenres = new List<string>() { "mahjong", "quiz" };
//            _badWords = new List<string>() { "adult content", "mature content", "sexy", "ladies", "naked", "sexual" };
//            _badPublishers = new List<string>() { "Mystique", "PlayAround" };
//        }

//        [Test]
//        public void DeserializeGameXml_Should_Work()
//        {
//            var result = Program.DeserializeGameXml(@"\\retropie\roms\megadrive\", @"\\retropie\roms\megadrive\gamelist.xml");

//            Assert.IsTrue(result.Count > 0);

//        }

//        [Test]
//        public void GetFullGameList_Should_Work()
//        {
//            var result = Program.GetFullGameList(@"\\retropie\roms");

//            Assert.IsTrue(result.Count > 0);

//        }

//        [Test]
//        public void Identify_Undesirable_Games_Should_Work()
//        {
//            var inputList = Program.GetFullGameList(@"\\retropie\roms");

//            var badGames = Program.IdentifyUndesirableGames(inputList, _badGenres, _badWords, _badPublishers);

//            var list = new StringBuilder();
//            list.AppendLine("Title|Genre|Path|Description");

//            foreach(var game in badGames)
//                list.AppendLine($"{game.Name}|{game.Genre}|{game.fullPath}|{ReplaceLineBreaks(game.desc, ';')}");

//            Debug.WriteLine("---------------------------------------------------------------------------");
//            Debug.WriteLine(list.ToString());
//            Debug.WriteLine("---------------------------------------------------------------------------");

//            Assert.IsTrue(badGames.Count > 0);

//        }

//        [Test]
//        public void Delete_Games_Should_Work()
//        {
//            var inputList = Program.GetFullGameList(@"\\retropie\roms");
//            var badGames = Program.IdentifyUndesirableGames(inputList, _badGenres, _badWords, _badPublishers);

//            Program.DeleteGames(badGames.Select(x => x.fullPath).ToList());
//        }

//        protected string ReplaceLineBreaks(string input, char replacement)
//        {
//            return input.Replace("\r\n", "\n").Replace('\r', '\n').Replace('\n', replacement);
//        }

//    }
//}