using Leopotam.EcsLite;
using Microsoft.Xna.Framework;
using System.Globalization;
using System.Xml.Linq;

namespace EcsUI.Parsing.Subparsing;

internal class ColorParser : BaseParser<ColorComponent>
{
    public ColorParser(EcsWorld ecsWorld) : base(ecsWorld)
    {
    }

    public override void Parse(XElement element, int entity)
    {
        var attribute = element.Attribute("color");

        if (attribute == null)
            return;

        ref ColorComponent component = ref Pool.GetSafe(entity);

        string[] color = attribute.Value.Split(',');

        color[0] = color[0].Trim();
        color[1] = color[1].Trim();
        color[2] = color[2].Trim();

        if (color.Length == 4)
            color[3] = color[3].Trim();

        var R = int.Parse(color[0]);
        var G = int.Parse(color[1]);
        var B = int.Parse(color[2]);

        var A = color.Length == 4
              ? float.Parse(color[3], CultureInfo.InvariantCulture)
              : 1;

        component.Color = new Color(R / 255f, G / 255f, B / 255f, A);
    }
}