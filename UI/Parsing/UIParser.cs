using Leopotam.EcsLite;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using EcsUI.Parsing;
using System.Diagnostics;
using System.Globalization;
using System.Xml.Linq;

namespace EcsUI;

public partial class UIParser
{
    private readonly GraphicsDevice _graphicsDevice;
    private readonly IReadOnlyDictionary<string, SpriteFont> _spriteFonts;

    private EcsPool<TextComponent> _textPool;
    private EcsPool<PositionComponent> _positionPool;
    private EcsPool<SizeComponent> _sizePool;
    private EcsPool<FontComponent> _fontPool;
    private EcsPool<ColorComponent> _colorPool;
    private EcsPool<SpriteComponent> _spritePool;
    private EcsPool<ScaleComponent> _scalePool;
    private EcsPool<EnabledComponent> _enabledPool;
    private EcsPool<HierarchyComponent> _hierarchyPool;

    public UIParser(GraphicsDevice graphicsDevice, 
                    IReadOnlyDictionary<string, SpriteFont> spriteFonts)
    {
        _graphicsDevice     = graphicsDevice;
        _spriteFonts        = spriteFonts;

        InitFinalization();
    }

    public void Parse(string path, EcsWorld ecsWorld)
    {
        XDocument document = XDocument.Load(path);

        if (document.Root == null)
            throw new Exception("Incorrect markup");

        GetPools(ecsWorld);

        var resources   = ParseResources(document, path);
        int rootEntity  = NewRootEntity(ecsWorld);
        
        Parse(document.Root, ecsWorld, rootEntity, resources);
    }

    private void GetPools(EcsWorld ecsWorld)
    {
        _textPool       = ecsWorld.GetPool<TextComponent>();
        _positionPool   = ecsWorld.GetPool<PositionComponent>();
        _sizePool       = ecsWorld.GetPool<SizeComponent>();
        _fontPool       = ecsWorld.GetPool<FontComponent>();
        _colorPool      = ecsWorld.GetPool<ColorComponent>();
        _spritePool     = ecsWorld.GetPool<SpriteComponent>();
        _scalePool      = ecsWorld.GetPool<ScaleComponent>();
        _enabledPool    = ecsWorld.GetPool<EnabledComponent>();
        _hierarchyPool  = ecsWorld.GetPool<HierarchyComponent>();
    }

    private Resources? ParseResources(XDocument document, string uiDocumentPath)
    {
        if (!ContainsResources(document, out var resourcesContainer))
            return null;

        Resources resources = new();
        string? directory   = null;

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

    private bool ContainsResources(XDocument document, out XElement? resourcesContainer)
    {
        resourcesContainer = document.Root!.Element("resources");

        return resourcesContainer != null;
    }

    private int NewRootEntity(EcsWorld ecsWorld)
    {
        int rootEntity = ecsWorld.NewEntity();

        EcsPool<DocumentRootComponent> pool = ecsWorld.GetPool<DocumentRootComponent>();
        pool.Add(rootEntity);

        return rootEntity;
    }

    private void Parse(XElement element, EcsWorld ecsWorld, int parentEntity, 
                              Resources? resources)
    {
        ref HierarchyComponent hierarchy = ref _hierarchyPool.Add(parentEntity);

        foreach (var child in element.Elements())
        {
            int entity = ecsWorld.NewEntity();

            if (HasStyle(child, out var styleName))
                ApplyStyle(styleName!, ecsWorld, entity, resources);

            ParseEntity(child, ecsWorld, entity);

            hierarchy   .Add(entity);
            _enabledPool.Add(entity);

            Parse(child, ecsWorld, entity, resources);
        }
    }

    private bool HasStyle(XElement element, out string? styleName)
    {
        var styleAttribute = element.Attribute("style");

        styleName = styleAttribute?.Value;

        return styleAttribute != null;
    }

    private void ApplyStyle(string name, EcsWorld ecsWorld, int entity, Resources? resources)
    {
        if (resources == null)
            throw new Exception("Resources is null");


        foreach (var styleSheet in resources.StyleSheets)
        {
            var style = styleSheet[name];

            if (style == null)
                continue;

            ParseEntity(style, ecsWorld, entity);
        }
    }

    private void ParseEntity(XElement element, EcsWorld ecsWorld, int entity)
    {
        switch (element.Name.LocalName)
        {
            case "resources":
                throw new Exception("Sesources should be placed in the top of document");
            case "style":
                throw new Exception("Style should be placed in resources section");
            case "text":
                ParseText(element, entity);
                ParsePosition(element, entity);
                ParseSize(element, entity);
                ParseFont(element, entity);
                ParseColor(element, entity);

                FinalyzeText(ecsWorld, entity);

                break;
            case "container":
                ParsePosition(element, entity);
                ParseSize(element, entity);

                FinalyzeContainer(ecsWorld, entity);

                break;
            case "image":
                ParsePosition(element, entity);
                ParseSize(element, entity);
                ParseColor(element, entity);
                ParseSprite(element, entity);

                FinalyzeImage(ecsWorld, entity);

                ParseScale(entity);

                break;
            default:
                throw new NotImplementedException("Unimplemented component type: " +
                                                 $"{element.Name.LocalName}");
        }
    }

    private void ParseText(XElement element, int entity)
    {
        if (string.IsNullOrWhiteSpace(element.Value))
            return;

        ref TextComponent component = ref _textPool.GetSafe(entity);

        component.Text = element.Value.Trim();
    }

    private void ParsePosition(XElement element, int entity)
    {
        ref PositionComponent component = ref _positionPool.GetSafe(entity);

        ParseVector(element, "position_x", "position_y", ref component.Position);
    }

    private void ParseSize(XElement element, int entity)
    {
        ref SizeComponent component = ref _sizePool.GetSafe(entity);

        ParseVector(element, "width", "height", ref component.Size);
    }

    private void ParseVector(XElement element, string firstValueName, 
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

    private void ParseFont(XElement element, int entity)
    {
        var attribute = element.Attribute("font");

        if (attribute == null)
            return;

        ref FontComponent component = ref _fontPool.GetSafe(entity);

        string fontName = attribute.Value;

        Debug.WriteLine(fontName);

        component.Font  = _spriteFonts[fontName];
    }

    private void ParseColor(XElement element, int entity)
    {
        var attribute = element.Attribute("color");

        if (attribute == null)
            return;

        ref ColorComponent component = ref _colorPool.GetSafe(entity);

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

    private void ParseSprite(XElement element, int entity)
    {
        var attribute = element.Attribute("sprite");

        if (attribute == null)
            throw new Exception("Image hasn't sprite");

        ref SpriteComponent component = ref _spritePool.GetSafe(entity);

        component.Sprite = Texture2D.FromFile(_graphicsDevice, attribute.Value);
    }

    private void ParseScale(int entity)
    {
        ref SizeComponent sizeComponent     = ref _sizePool.Get(entity);
        ref SpriteComponent spriteComponent = ref _spritePool.Get(entity);
        ref ScaleComponent scaleComponent   = ref _scalePool.GetSafe(entity);

        Vector2 elementSize = sizeComponent.Size;
        Vector2 spriteSize  = new(spriteComponent.Sprite.Width, 
                                  spriteComponent.Sprite.Height);

        scaleComponent.Scale = elementSize / spriteSize;
    }
}