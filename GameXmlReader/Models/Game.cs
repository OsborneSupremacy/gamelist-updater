using System.Xml.Serialization;

namespace GameXmlReader.Models;

[XmlRoot(ElementName = "game")]
public record Game
{
    [XmlAttribute("id")]
    public string Id { get; set; }

    [XmlAttribute("source")]
    public string Source { get; set; }

    [XmlElement("path")]
    public string Path { get; set; }
    
    [XmlElement("name")]
    public string Name { get; set; }
    
    [XmlElement("desc")]
    public string Desc { get; set; }
    
    [XmlElement("rating")]
    public string Rating { get; set; }
    
    [XmlElement("releasedate")]
    public string ReleaseDate { get; set; }
    
    [XmlElement("developer")]
    public string Developer { get; set; }
    
    [XmlElement("publisher")]
    public string Publisher { get; set; }

    [XmlElement("genre")]
    public string Genre { get; set; }
    
    [XmlElement("players")]
    public string Players { get; set; }
    
    [XmlElement("image")]
    public string Image { get; set; }
    
    [XmlElement("marquee")]
    public string Marquee { get; set; }
    
    [XmlElement("video")]
    public string Video { get; set; }

    [XmlElement("adult")]
    public string? Adult { get; set; }

    [XmlElement("genreid")]
    public string GenreId { get; set; }
}
