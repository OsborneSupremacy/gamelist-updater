using System.Xml.Serialization;

namespace GameXmlReader.Models;

public record Provider
{
    [XmlElement("System")]
    public string System { get; set; }
    [XmlElement("software")]
    public string Software { get; set; }
    [XmlElement("database")]
    public string Database { get; set; }
    [XmlElement("web")]
    public string Web { get; set; }
}

