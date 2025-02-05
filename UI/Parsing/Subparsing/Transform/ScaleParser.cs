using Leopotam.EcsLite;
using Microsoft.Xna.Framework;
using System.Xml.Linq;

namespace EcsUI.Parsing.Subparsing;
internal class ScaleParser : BaseParser <ScaleComponent>
{
    private readonly EcsPool<SizeComponent> _sizePool;
    private readonly EcsPool<SpriteComponent> _spritePool;

    public ScaleParser(EcsWorld ecsWorld) : base(ecsWorld)
    {
        _sizePool   = ecsWorld.GetPool<SizeComponent>();
        _spritePool = ecsWorld.GetPool<SpriteComponent>();
    }

    public override void Parse(XElement element, int entity) => ParseScale(entity);

    private void ParseScale(int entity)
    {
        if (!_sizePool.Has(entity) || !_spritePool.Has(entity))
            return;

        ref SizeComponent sizeComponent     = ref _sizePool.Get(entity);
        ref SpriteComponent spriteComponent = ref _spritePool.Get(entity);
        ref ScaleComponent scaleComponent   = ref Pool.GetSafe(entity);

        Vector2 elementSize = sizeComponent.Size;
        Vector2 spriteSize = new(spriteComponent.Sprite.Width,
                                  spriteComponent.Sprite.Height);

        scaleComponent.Scale = elementSize / spriteSize;
    }
}