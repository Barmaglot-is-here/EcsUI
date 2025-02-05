//using Leopotam.EcsLite;
//using System.Xml.Linq;

//namespace EcsUI.Parsing.Subparsing;

//internal class SelectionParser : BaseParser<VisualStateComponent>
//{
//    public SelectionParser(EcsWorld ecsWorld) : base(ecsWorld)
//    {
//    }

//    public override void Parse(XElement element, int entity)
//    {
//        var defaultMarker   = element.Element("default");
//        var hoverMarker     = element.Element("hover");
//        var pressMarker     = element.Element("press");
//        var lockMarker  = element.Element("lock");

//        if (defaultMarker == null && hoverMarker == null && pressMarker == null
//            && lockMarker == null) return;

//        ref VisualStateComponent visualState = ref Pool.Add(entity);

//        visualState.State = VisualState.Default;

//        if (defaultMarker != null)
//            _defaultMarkerPool.Add(entity);

//        if (hoverMarker != null)
//            _hoverMarkerPool.Add(entity);

//        if (pressMarker != null)
//            _pressMarkerPool.Add(entity);

//        if (lockMarker != null)
//            _lockMarkerPool.Add(entity);
//    }
//}