using System.Xml.Linq;

namespace EcsUI;

internal static class StyleParser
{
    internal static StyleSheet Parse(string path)
    {
        Dictionary<string, XElement> styles = new();

        var document    = XDocument.Load(path);
        var root        = document.Root;

        if (root == null)
            throw new Exception("Incorrect markup");

        foreach (var element in root.Elements())
        {
            var name = GetName(element);

            styles.Add(name, element);
        }

        return new(styles);
    }

    private static string GetName(XElement element)
    {
        var nameAttribute = element.Attribute("name");

        if (nameAttribute == null)
            throw new Exception("Name attribute couldn't be found");

        return nameAttribute.Value;
    }
}