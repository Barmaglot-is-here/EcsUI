using Microsoft.Xna.Framework;
using System.Globalization;
using System.Xml.Linq;

namespace EcsUI.Parsing.Subparsing;

internal static class Utils
{
    public static void ParseVector(XElement element, string firstValueName,
                         string secondValueName, ref Vector2 vector)
    {
        var firstAttribute = element.Attribute(firstValueName);
        var secondAttribute = element.Attribute(secondValueName);

        if (firstAttribute != null && !float.TryParse(firstAttribute.Value,
                                                      CultureInfo.InvariantCulture,
                                                      out vector.X))
            throw new Exception($"incorrect value: {firstValueName}");

        if (secondAttribute != null && !float.TryParse(secondAttribute.Value,
                                                       CultureInfo.InvariantCulture,
                                                       out vector.Y))
            throw new Exception($"incorrect value: {secondValueName}");
    }
}