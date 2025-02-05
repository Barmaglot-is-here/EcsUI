using EcsUI.Parsing.Subparsing;
using Leopotam.EcsLite;
using Microsoft.Xna.Framework.Graphics;
using System.Xml.Linq;

namespace EcsUI.Parsing;

internal class MarkupParser
{
    private readonly EcsWorld _ecsWorld;
    private readonly EcsPool<HierarchyComponent> _hierarchyPool;
    private readonly Dictionary<string, ParsingPreset> _parsingPresets;

    public MarkupParser(EcsWorld ecsWorld, GraphicsDevice graphicsDevice,
                        IReadOnlyDictionary<string, SpriteFont> spriteFonts)
    {
        _ecsWorld       = ecsWorld;
        _hierarchyPool  = ecsWorld.GetPool<HierarchyComponent>();

        ColorParser colorParser         = new(ecsWorld);
        SpriteParser spriteParser       = new(ecsWorld, graphicsDevice);
        TextParser textParser           = new(ecsWorld, spriteFonts);
        PositionParser positionParser   = new(ecsWorld);
        SizeParser sizeParser           = new(ecsWorld);
        ScaleParser scaleParser         = new(ecsWorld);
        IDParser idParser               = new(ecsWorld);
        //SelectionParser selectionParser = new(ecsWorld);
        StateParser stateParser         = new(ecsWorld);
        ComponentFinalyzer finalyzer    = new(ecsWorld, spriteFonts);

        ParsingPreset textPreset        = new();
        ParsingPreset containerPreset   = new();
        ParsingPreset imagePreset       = new();
        ParsingPreset buttonPreset      = new();

        textPreset
            .Add(textParser.Parse)
            .Add(sizeParser.Parse)
            .Add(positionParser.Parse)
            .Add(colorParser.Parse)
            .Add(idParser.Parse)
            .Add(stateParser.Parse)
            .Finalyze(finalyzer.FinalyzeText);

        containerPreset
            .Add(sizeParser.Parse)
            .Add(positionParser.Parse)
            .Add(idParser.Parse)
            .Add(stateParser.Parse)
            .Finalyze(finalyzer.FinalyzeContainer);

        imagePreset
            .Add(sizeParser.Parse)
            .Add(positionParser.Parse)
            .Add(colorParser.Parse)
            .Add(spriteParser.Parse)
            .Add(idParser.Parse)
            .Add(scaleParser.Parse)
            .Add(stateParser.Parse)
            .Finalyze(finalyzer.FinalyzeImage);

        buttonPreset
            .Add(sizeParser.Parse)
            .Add(positionParser.Parse)
            .Add(idParser.Parse)
            .Add(stateParser.Parse)
            .Finalyze(finalyzer.FinalyzeBytton);

        _parsingPresets = new()
        {
            { "text",       textPreset},
            { "container",  containerPreset},
            { "image",      imagePreset},
            { "button",     buttonPreset},
        };
    }

    public void Parse(XDocument document, Resources? resources)
    {
        int rootEntity = NewRootEntity(_ecsWorld);

        Parse(document.Root!, _ecsWorld, rootEntity, resources);
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
                ApplyStyle(styleName!, entity, resources);

            ParseEntity(child, entity);

            hierarchy.Add(entity);

            Parse(child, ecsWorld, entity, resources);
        }
    }

    private bool HasStyle(XElement element, out string? styleName)
    {
        var styleAttribute = element.Attribute("style");

        styleName = styleAttribute?.Value;

        return styleAttribute != null;
    }

    private void ApplyStyle(string name, int entity, Resources? resources)
    {
        if (resources == null)
            throw new Exception("Resources is null");

        foreach (var styleSheet in resources.StyleSheets)
        {
            var style = styleSheet[name];

            if (style == null)
                continue;

            ParseEntity(style, entity);
        }
    }

    private void ParseEntity(XElement element, int entity)
    {
        string elementName = element.Name.LocalName;

        if (elementName == "resources")
            throw new Exception("Sesources should be placed in the top of document");
        if (elementName == "style")
            throw new Exception("Sesources should be placed in the top of document");
        if (!_parsingPresets.ContainsKey(elementName))
            throw new NotImplementedException("Unimplemented component type: " +
                                                 $"{element.Name.LocalName}");

        ParsingPreset parsingPreset = _parsingPresets[elementName];
        parsingPreset.Run(element, entity);
    }
}