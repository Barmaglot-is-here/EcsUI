using Leopotam.EcsLite;
using Microsoft.Xna.Framework;

namespace EcsUI;

public partial class UIParser
{
    private Dictionary<Type, Action<int>> _textComponents;
    private Dictionary<Type, Action<int>> _containerComponents;
    private Dictionary<Type, Action<int>> _imageComponents;

    private void InitFinalization()
    { 
        _textComponents = new();
        _textComponents.Add(typeof(TextComponent),      DefaultText);
        _textComponents.Add(typeof(PositionComponent),  DefaultPosition);
        _textComponents.Add(typeof(SizeComponent),      DefaultSize);
        _textComponents.Add(typeof(FontComponent),      DefaultFont);
        _textComponents.Add(typeof(ColorComponent),     DefaultColor);

        _containerComponents = new();
        _containerComponents.Add(typeof(PositionComponent),  DefaultPosition);
        _containerComponents.Add(typeof(SizeComponent),      DefaultSize);

        _imageComponents = new();
        _imageComponents.Add(typeof(PositionComponent),  DefaultPosition);
        _imageComponents.Add(typeof(SizeComponent),      DefaultSize);
        _imageComponents.Add(typeof(ColorComponent),     DefaultColor);
    }

    private void FinalyzeText(EcsWorld ecsWorld, int entity)
        => Finalyze(ecsWorld, entity, _textComponents);

    private void FinalyzeContainer(EcsWorld ecsWorld, int entity)
        => Finalyze(ecsWorld, entity, _containerComponents);

    private void FinalyzeImage(EcsWorld ecsWorld, int entity)
        => Finalyze(ecsWorld, entity, _imageComponents);

    //Тут более адекватный алгоритм прикрутить
    //И названия словарей поменять 
    private void Finalyze(EcsWorld ecsWorld, int entity,
                          IReadOnlyDictionary<Type, Action<int>> components)
    {
        Type[] entityComponents = null;

        ecsWorld.GetComponentTypes(entity, ref entityComponents);

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
}