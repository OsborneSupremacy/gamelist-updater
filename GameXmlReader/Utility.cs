using System.IO;

namespace GameXmlReader;

public static class Utility
{
    public static string ReadFileWithoutLocking(string fullFilePath)
    {
        using FileStream fileStream = new(fullFilePath, FileMode.Open, FileAccess.Read, FileShare.Read);
        using StreamReader reader = new (fileStream);
        return reader.ReadToEnd();
    }
}
