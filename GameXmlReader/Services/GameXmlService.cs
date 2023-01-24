using System.Text;
using System.Xml;

namespace GameXmlReader.Services;

public class GameXmlService
{
    public GameList DeserializeXml(string xml) =>
        (GameList)new XmlSerializer(typeof(GameList)).Deserialize(new StringReader(xml));

    public string SerializeXml(GameList gameList)
    {
        XmlSerializer serializer = new (typeof(GameList));
        var settings = new XmlWriterSettings
        {
            OmitXmlDeclaration = false,
            Indent = true,
            IndentChars = new string(' ', 2),
            Encoding = Encoding.UTF8
        };

        using var stream = new MemoryStream();
        using var writer = XmlWriter.Create(stream, settings);
        writer.WriteStartDocument(true);
        serializer.Serialize(writer, gameList);

        var xml = Encoding.UTF8.GetString(stream.ToArray());

        return xml
            // there's a better way to make these replacements. However, this works, and
            // I don't want to spend more time trying to implement the right way.
            .Replace(@" xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance""", string.Empty)
            .Replace(@" xmlns:xsd=""http://www.w3.org/2001/XMLSchema""", string.Empty);
    }
}