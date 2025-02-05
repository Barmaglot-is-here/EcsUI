//using Leopotam.EcsLite;
//using System.Diagnostics;

//namespace EcsUI.Systems;
//public class HoverHandleSystem : IEcsInitSystem, IEcsRunSystem
//{
//    private EcsFilter _hoverFilter;
//    private EcsFilter _leaveFilter;

//    private EcsPool<HoverMarker> _hoverMarkerPool;
//    private EcsPool<DefaultMarker> _defaultMarkerPool;
//    private EcsPool<DisabledMarker> _disabledMarkerPool;

//    private EcsPool<HoverEvent> _hoverEventPool;
//    private EcsPool<LeaveEvent> _leaveEventPool;
//    private EcsPool<HierarchyComponent> _hierarchyPool;

//    public void Init(IEcsSystems systems)
//    {
//        var world = systems.GetWorld();

//        _hoverFilter = world.Filter<HoverEvent>()
//                            .End();
//        _leaveFilter = world.Filter<LeaveEvent>()
//                            .End();

//        _hoverMarkerPool    = world.GetPool<HoverMarker>();
//        _defaultMarkerPool  = world.GetPool<DefaultMarker>();
//        _disabledMarkerPool = world.GetPool<DisabledMarker>();

//        _hoverEventPool     = world.GetPool<HoverEvent>();
//        _leaveEventPool     = world.GetPool<LeaveEvent>();
//        _hierarchyPool      = world.GetPool<HierarchyComponent>();
//    }

//    public void Run(IEcsSystems systems)
//    {
//        foreach (int entity in _hoverFilter)
//        {
//            ApplyHoverStyle(entity);
            
//            _hoverEventPool.Del(entity);
//        }

//        foreach (int entity in _leaveFilter)
//        {
//            ApplyDefaultStyle(entity);
            
//            _leaveEventPool.Del(entity);
//        }
//    }

//    private void ApplyHoverStyle(int entity)
//    {
//        int currentStyle = -1;
//        int hoverStyle = -1;

//        ref HierarchyComponent hierarchy = ref _hierarchyPool.Get(entity);

//        Debug.WriteLine(hierarchy.Children.Count);

//        foreach (int child in hierarchy.Children)
//        {
//            //Debug.WriteLine(child);

//            if (_defaultMarkerPool.Has(child))
//                currentStyle = child;
//            if (_hoverMarkerPool.Has(child))
//                hoverStyle = child;
//        }

//        if (currentStyle != -1)
//            _disabledMarkerPool.Add(currentStyle);
//        if (hoverStyle != -1)
//            _disabledMarkerPool.Del(hoverStyle);
//    }

//    private void ApplyDefaultStyle(int entity)
//    {
//        int currentStyle = -1;
//        int defaultStyle = -1;

//        ref HierarchyComponent hirarchy = ref _hierarchyPool.Get(entity);

//        foreach (int child in hirarchy.Children)
//        {
//            if (_hoverMarkerPool.Has(child))
//                currentStyle = child;
//            if (_defaultMarkerPool.Has(child))
//                defaultStyle = child;
//        }

//        if (currentStyle != -1)
//            _disabledMarkerPool.Add(currentStyle);
//        if (defaultStyle != -1)
//            _disabledMarkerPool.Del(defaultStyle);
//    }
//}