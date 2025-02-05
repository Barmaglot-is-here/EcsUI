using System.Xml.Linq;

namespace EcsUI.Parsing;

internal static class ResourcesParser
{
    internal static Resources? Parse(XDocument document, string uiDocumentPath)
    {
        if (!ContainsResources(document, out var resourcesContainer))
            return null;

        Resources resources = new();
        string? directory = null;

        foreach (var resource in resourcesContainer!.Elements())
        {
            if (resource.Name.ToString() == "style")
            {
                directory ??= Path.GetDirectoryName(uiDocumentPath);

                string stylePath    = resource.Attribute("path").Value;
                stylePath           = Path.Combine(directory!, stylePath);

                StyleSheet styleSheet = StyleSheet.Load(stylePath);

                resources.StyleSheets.Add(styleSheet);
            }
        }

        resourcesContainer.Remove();

        return resources;
    }

    private static bool ContainsResources(XDocument document, out XElement? resourcesContainer)
    {
        resourcesContainer = document.Root!.Element("resources");

        return resourcesContainer != null;
    }
}