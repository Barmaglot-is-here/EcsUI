using Leopotam.EcsLite;
using Microsoft.Xna.Framework;
using System.Diagnostics;

namespace EcsUI;
public class InitPositionSystem : IEcsInitSystem
{
    private EcsFilter _filter;
    private EcsPool<PositionComponent> _positions;
    private EcsPool<HierarchyComponent> _hierarchy;

    public void Init(IEcsSystems systems)
    {
        var world = systems.GetWorld();

        _filter = world .Filter<DocumentRootComponent>()
                        .End();

        _positions = world.GetPool<PositionComponent>();
        _hierarchy = world.GetPool<HierarchyComponent>();

        foreach (int entity in _filter)
        {
            ref HierarchyComponent hierarchy = ref _hierarchy.Get(entity);

            UpdatePosition(hierarchy.Children, Vector2.Zero);
        }
    }

    private void UpdatePosition(IReadOnlyList<int> children, Vector2 offcet)
    {
        foreach (int child in children)
        {
            ref PositionComponent childPosition = ref _positions.Get(child);
            ref HierarchyComponent hierarchy    = ref _hierarchy.Get(child);

            childPosition.Position += offcet;

            UpdatePosition(hierarchy.Children, childPosition.Position);
        }
    }
}