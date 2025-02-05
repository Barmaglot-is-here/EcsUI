using EcsUI.Parsing;
using Leopotam.EcsLite;
using Microsoft.Xna.Framework.Graphics;
using System.Xml.Linq;

namespace EcsUI;

public class UIParser
{
    private readonly GraphicsDevice _graphicsDevice;
    private readonly IReadOnlyDictionary<string, SpriteFont> _spriteFonts;

    public UIParser(GraphicsDevice graphicsDevice, 
                    IReadOnlyDictionary<string, SpriteFont> spriteFonts)
    {
        _graphicsDevice = graphicsDevice;
        _spriteFonts    = spriteFonts;
    }

    public void Parse(string path, EcsWorld ecsWorld)
    {
        XDocument document = XDocument.Load(path);

        if (document.Root == null)
            throw new Exception("Document has no root");
        
        var resources       = ResourcesParser.Parse(document, path);
        var markupParser    = new MarkupParser(ecsWorld, _graphicsDevice, _spriteFonts);

        markupParser.Parse(document, resources);
    }
}