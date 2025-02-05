//using Leopotam.EcsLite;
//using Microsoft.Xna.Framework.Input;

//namespace EcsUI.Systems;
//public class HoverDetectionSystem : IEcsInitSystem, IEcsRunSystem
//{
//    private EcsFilter _filter;
//    private EcsPool<SizeComponent> _sizePool;
//    private EcsPool<PositionComponent> _positionPool;

//    private EcsPool<HoverEvent> _hoverEventPool;
//    private EcsPool<LeaveEvent> _leaveEventPool;
//    private EcsPool<VisualStateComponent> _visualStatePool;

//    public void Init(IEcsSystems systems)
//    {
//        var world   = systems.GetWorld();

//        _filter     = world.Filter<HoverMarker>()
//                           .Inc<SizeComponent>()
//                           .Inc<PositionComponent>()
//                           .Exc<DisabledMarker>()
//                           .End();

//        _sizePool       = world.GetPool<SizeComponent>();
//        _positionPool   = world.GetPool<PositionComponent>();

//        _hoverEventPool     = world.GetPool<HoverEvent>();
//        _leaveEventPool     = world.GetPool<LeaveEvent>();
//        _visualStatePool    = world.GetPool<VisualStateComponent>();
//    }

//    public void Run(IEcsSystems systems)
//    {
//        foreach (int entity in _filter)
//        {
//            ref PositionComponent position          = ref _positionPool.Get(entity);
//            ref SizeComponent size                  = ref _sizePool.Get(entity);
//            ref VisualStateComponent visualState    = ref _visualStatePool.Get(entity);

//            var mousePosition = Mouse.GetState().Position;
            
//            var leftBound   = position.Position.X;
//            var rightBound  = position.Position.X + size.Size.X;
//            var topBound    = position.Position.Y;
//            var bottomBound = position.Position.Y + size.Size.Y;

//            bool crossedByX = mousePosition.X >= leftBound
//                           && mousePosition.X <= rightBound;
//            bool crossedByY = mousePosition.Y >= topBound
//                           && mousePosition.Y <= bottomBound;

//            if (crossedByX && crossedByY)
//            {
//                if (visualState.State != VisualState.Hover)
//                {
//                    _hoverEventPool.Add(entity);

//                    visualState.State = VisualState.Hover;
//                }
//            }
//            else if (visualState.State == VisualState.Hover)
//            {
//                _leaveEventPool.Add(entity);

//                visualState.State = VisualState.Default;
//            }
//        }
//    }
//}