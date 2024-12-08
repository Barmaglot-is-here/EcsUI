using Leopotam.EcsLite;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace EcsUI.Systems;
public class DrawSpriteSystem : IEcsInitSystem, IEcsRunSystem
{
    private EcsFilter _filter;
    private EcsPool<SizeComponent> _sizePool;
    private EcsPool<PositionComponent> _positionPool;
    private EcsPool<SpriteComponent> _spritePool;
    private EcsPool<ColorComponent> _colorPool;
    private EcsPool<ScaleComponent> _scalePool;

    private SpriteBatch _batch;

    public void Init(IEcsSystems systems)
    {
        var world           = systems.GetWorld();
        _batch              = systems.GetShared<SpriteBatch>();

        _filter     = world.Filter<SpriteComponent>()
                           .Inc<PositionComponent>()
                           .Inc<SizeComponent>()
                           .Inc<EnabledComponent>()
                           .End();

        _sizePool       = world.GetPool<SizeComponent>();
        _positionPool   = world.GetPool<PositionComponent>();
        _spritePool     = world.GetPool<SpriteComponent>();
        _colorPool      = world.GetPool<ColorComponent>();
        _scalePool      = world.GetPool<ScaleComponent>();
    }

    public void Run(IEcsSystems systems)
    {
        foreach (int entity in _filter)
        {
            ref SpriteComponent sprite      = ref _spritePool.Get(entity);
            ref PositionComponent position  = ref _positionPool.Get(entity);
            ref SizeComponent size          = ref _sizePool.Get(entity);
            ref ColorComponent color        = ref _colorPool.Get(entity);
            ref ScaleComponent scale        = ref _scalePool.Get(entity);

            _batch.Draw(sprite.Sprite, position.Position, null, color.Color, 0, 
                        Vector2.Zero, scale.Scale, SpriteEffects.None, 1);
        }
    }
}