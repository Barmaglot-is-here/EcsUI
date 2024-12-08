using System.Xml.Linq;

namespace EcsUI;
internal class StyleSheet
{
    private readonly IReadOnlyDictionary<string, XElement> _styles;

    public XElement? this[string name]
    {
        get
        {
            _styles.TryGetValue(name, out XElement? result);

            return result;
        }
    }

    internal StyleSheet(IReadOnlyDictionary<string, XElement> styles)
    {
        _styles = styles;
    }

    public static StyleSheet Load(string path) => StyleParser.Parse(path);
}