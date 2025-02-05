using Leopotam.EcsLite;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace EcsUI;

public class ComponentFinalyzer
{
    private readonly EcsWorld _ecsWorld;
    private readonly IReadOnlyDictionary<string, SpriteFont> _spriteFonts;

    private readonly EcsPool<TextComponent> _textPool;
    private readonly EcsPool<PositionComponent> _positionPool;
    private readonly EcsPool<SizeComponent> _sizePool;
    private readonly EcsPool<FontComponent> _fontPool;
    private readonly EcsPool<ColorComponent> _colorPool;
    private readonly EcsPool<ScaleComponent> _scalePool;

    private readonly Dictionary<Type, Action<int>> _textComponents;
    private readonly Dictionary<Type, Action<int>> _containerComponents;
    private readonly Dictionary<Type, Action<int>> _imageComponents;
    private readonly Dictionary<Type, Action<int>> _buttonComponents;
    
    public ComponentFinalyzer(EcsWorld world, 
                              IReadOnlyDictionary<string, SpriteFont> spriteFonts)
    {
        _ecsWorld       = world;
        _spriteFonts    = spriteFonts;

        _textPool       = world.GetPool<TextComponent>();
        _positionPool   = world.GetPool<PositionComponent>();
        _sizePool       = world.GetPool<SizeComponent>();
        _fontPool       = world.GetPool<FontComponent>();
        _colorPool      = world.GetPool<ColorComponent>();
        _scalePool      = world.GetPool<ScaleComponent>();

        _textComponents = new();
        _textComponents.Add(typeof(TextComponent),      DefaultText);
        _textComponents.Add(typeof(PositionComponent),  DefaultPosition);
        _textComponents.Add(typeof(SizeComponent),      DefaultSize);
        _textComponents.Add(typeof(FontComponent),      DefaultFont);
        _textComponents.Add(typeof(ColorComponent),     DefaultColor);

        _containerComponents = new();
        _containerComponents.Add(typeof(PositionComponent), DefaultPosition);
        _containerComponents.Add(typeof(SizeComponent),     DefaultSize);

        _imageComponents = new();
        _imageComponents.Add(typeof(PositionComponent), DefaultPosition);
        _imageComponents.Add(typeof(SizeComponent),     DefaultSize);
        _imageComponents.Add(typeof(ColorComponent),    DefaultColor);
        _imageComponents.Add(typeof(ScaleComponent),    DefaultScale);

        _buttonComponents = _containerComponents;
    }

    public void FinalyzeText(int entity)
        => Finalyze(entity, _textComponents);

    public void FinalyzeContainer(int entity)
        => Finalyze(entity, _containerComponents);

    public void FinalyzeImage(int entity)
        => Finalyze(entity, _imageComponents);

    public void FinalyzeBytton(int entity)
        => Finalyze(entity, _buttonComponents);

    //Тут более адекватный алгоритм прикрутить
    //И названия словарей поменять 
    private void Finalyze(int entity,
                          IReadOnlyDictionary<Type, Action<int>> components)
    {
        Type[]? entityComponents = null;

        _ecsWorld.GetComponentTypes(entity, ref entityComponents!);

        foreach (var typeFuntionPair in components)
        {
            var type        = typeFuntionPair.Key;
            var function    = typeFuntionPair.Value;

            if (!entityComponents.Contains(type))
                function.Invoke(entity);
        }
    }

    private void DefaultText(int entity)
    {
        ref TextComponent component = ref _textPool.Add(entity);

        component.Text = "";
    }

    private void DefaultPosition(int entity)
    {
        ref PositionComponent component = ref _positionPool.Add(entity);

        component.Position = Vector2.Zero;
    }

    private void DefaultSize(int entity)
    {
        ref SizeComponent component = ref _sizePool.Add(entity);

        component.Size = Vector2.Zero;
    }

    private void DefaultFont(int entity)
    {
        ref FontComponent component = ref _fontPool.Add(entity);

        component.Font = _spriteFonts.First().Value;
    }

    private void DefaultColor(int entity)
    {
        ref ColorComponent component = ref _colorPool.Add(entity);

        component.Color = Color.White;
    }

    private void DefaultScale(int entity)
    {
        ref ScaleComponent component = ref _scalePool.Add(entity);

        component.Scale = Vector2.One;
    }
}