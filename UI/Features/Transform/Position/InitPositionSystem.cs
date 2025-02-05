using Leopotam.EcsLite;
using Microsoft.Xna.Framework;

namespace EcsUI;
public class InitPositionSystem : IEcsInitSystem
{
    private EcsPool<PositionComponent> _positionPool;
    private EcsPool<HierarchyComponent> _hierarchyPool;

    public void Init(IEcsSystems systems)
    {
        var world = systems.GetWorld();

        var filter = world  .Filter<DocumentRootComponent>()
                            .End();

        _positionPool   = world.GetPool<PositionComponent>();
        _hierarchyPool  = world.GetPool<HierarchyComponent>();

        foreach (int entity in filter)
        {
            ref HierarchyComponent hierarchy = ref _hierarchyPool.Get(entity);

            UpdatePosition(hierarchy.Children, Vector2.Zero);
        }
    }

    private void UpdatePosition(IReadOnlyList<int> children, Vector2 offcet)
    {
        foreach (int child in children)
        {
            ref PositionComponent childPosition = ref _positionPool.Get(child);
            ref HierarchyComponent hierarchy    = ref _hierarchyPool.Get(child);

            childPosition.Position += offcet;

            UpdatePosition(hierarchy.Children, childPosition.Position);
        }
    }
}