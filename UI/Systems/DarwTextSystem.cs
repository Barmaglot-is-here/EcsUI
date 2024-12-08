using Leopotam.EcsLite;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace EcsUI;

public class DarwTextSystem : IEcsInitSystem, IEcsRunSystem
{
    private EcsFilter _filter;

    private EcsPool<TextComponent> _texts;
    private EcsPool<SizeComponent> _sizes;
    private EcsPool<PositionComponent> _positions;
    private EcsPool<FontComponent> _fonts;
    private EcsPool<ColorComponent> _colors;

    private SpriteBatch _spriteBatch;

    public void Init(IEcsSystems systems)
    {
        var world           = systems.GetWorld();
        _spriteBatch        = systems.GetShared<SpriteBatch>();

        _filter = world .Filter<TextComponent>()
                        .Inc<SizeComponent>()
                        .Inc<PositionComponent>()
                        .Inc<EnabledComponent>()
                        .End();

        _texts       = world.GetPool<TextComponent>();
        _sizes       = world.GetPool<SizeComponent>();
        _positions   = world.GetPool<PositionComponent>();
        _fonts       = world.GetPool<FontComponent>();
        _colors      = world.GetPool<ColorComponent>();
    }

    public void Run(IEcsSystems systems)
    {
        foreach (int entity in _filter)
        {
            ref TextComponent text          = ref _texts.Get(entity);
            ref PositionComponent position  = ref _positions.Get(entity);
            ref FontComponent font          = ref _fonts.Get(entity);
            ref ColorComponent color        = ref _colors.Get(entity);

            _spriteBatch.DrawString(font.Font, text.Text, position.Position, color.Color);
        }
    }
}