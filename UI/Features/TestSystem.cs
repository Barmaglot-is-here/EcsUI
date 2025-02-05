using Leopotam.EcsLite;
using System.Diagnostics;

namespace EcsUI;
public class TestSystem : IEcsInitSystem
{
    private EcsPool<HierarchyComponent> _hierarchyPool;
    private EcsPool<IDComponent> _idPool;

    public void Init(IEcsSystems systems)
    {
        var world   = systems.GetWorld();
        var filter  = world.Filter<DocumentRootComponent>()
                           .End();

        _hierarchyPool  = world.GetPool<HierarchyComponent>();
        _idPool         = world.GetPool<IDComponent>();

        foreach (int entity in filter)
        {
            ref HierarchyComponent hierarchy = ref _hierarchyPool.Get(entity);

            UpdatePosition(hierarchy.Children);
        }
    }

    private void UpdatePosition(IReadOnlyList<int> children)
    {
        foreach (int child in children)
        {
            if (_idPool.Has(child))
            {
                ref IDComponent id = ref _idPool.Get(child);
                ref HierarchyComponent hierarchy1 = ref _hierarchyPool.Get(child);

                Debug.WriteLine("Has id: " + id.ID);
                Debug.WriteLine("Child count: " + hierarchy1.Children.Count);
            }

            ref HierarchyComponent hierarchy = ref _hierarchyPool.Get(child);

            UpdatePosition(hierarchy.Children);
        }
    }
}