namespace GameXmlReader.Models;

[XmlRoot(ElementName = "gameList")]
public record GameList
{
    [XmlElement("provider")]
    public Provider Provider { get; set; }

    [XmlElement(ElementName = "game")]
    public List<Game> Games { get; set; }
}

