using Leopotam.EcsLite;

namespace EcsUI;
public class InitStateSystem : IEcsInitSystem
{
    private EcsPool<DisabledMarker> _disabledMarkerPool;
    private EcsPool<HierarchyComponent> _hierarchyPool;

    public void Init(IEcsSystems systems)
    {
        var world = systems.GetWorld();
        
        var filter = world  .Filter<DocumentRootComponent>()
                            .End();

        _disabledMarkerPool   = world.GetPool<DisabledMarker>();
        _hierarchyPool  = world.GetPool<HierarchyComponent>();

        foreach (int entity in filter)
        {
            ref HierarchyComponent hierarchy = ref _hierarchyPool.Get(entity);

            bool disable = _disabledMarkerPool.Has(entity);
            
            UpdateState(hierarchy.Children, disable);
        }
    }

    private void UpdateState(IReadOnlyList<int> children, bool disable)
    {
        foreach (int child in children)
        {
            ref HierarchyComponent hierarchy = ref _hierarchyPool.Get(child);

            if (disable && !_disabledMarkerPool.Has(child))
                _disabledMarkerPool.Add(child);

            disable = _disabledMarkerPool.Has(child);
            
            UpdateState(hierarchy.Children, disable);
        }
    }
}