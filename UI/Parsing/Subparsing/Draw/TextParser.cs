using Leopotam.EcsLite;
using Microsoft.Xna.Framework.Graphics;
using System.Xml.Linq;

namespace EcsUI.Parsing.Subparsing;
internal class TextParser : BaseParser<TextComponent>
{
    private readonly EcsPool<FontComponent> _fontPool;

    private readonly IReadOnlyDictionary<string, SpriteFont> _spriteFonts;

    public TextParser(EcsWorld ecsWorld, 
                      IReadOnlyDictionary<string, SpriteFont> spriteFonts) : base(ecsWorld)
    {
        _fontPool = ecsWorld.GetPool<FontComponent>();

        _spriteFonts = spriteFonts;
    }

    public override void Parse(XElement element, int entity)
    {
        ParseText(element, entity);
        ParseFont(element, entity);
    }

    private void ParseText(XElement element, int entity)
    {
        if (string.IsNullOrWhiteSpace(element.Value))
            return;

        ref TextComponent component = ref Pool.GetSafe(entity);

        component.Text = element.Value.Trim();
    }

    private void ParseFont(XElement element, int entity)
    {
        var attribute = element.Attribute("font");

        if (attribute == null)
            return;

        ref FontComponent component = ref _fontPool.GetSafe(entity);

        string fontName = attribute.Value;

        component.Font = _spriteFonts[fontName];
    }
}