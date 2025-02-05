using Leopotam.EcsLite;
using Microsoft.Xna.Framework.Graphics;
using System.Xml.Linq;

namespace EcsUI.Parsing.Subparsing;

internal class SpriteParser : BaseParser<SpriteComponent>
{
    private readonly GraphicsDevice _graphicsDevice;

    public SpriteParser(EcsWorld ecsWorld, GraphicsDevice graphicsDevice) : base(ecsWorld)
    {
        _graphicsDevice = graphicsDevice;
    }

    public override void Parse(XElement element, int entity)
    {
        var attribute = element.Attribute("sprite");

        if (attribute == null)
            throw new Exception("Image hasn't sprite");

        ref SpriteComponent component = ref Pool.GetSafe(entity);

        component.Sprite = Texture2D.FromFile(_graphicsDevice, attribute.Value);
    }
}