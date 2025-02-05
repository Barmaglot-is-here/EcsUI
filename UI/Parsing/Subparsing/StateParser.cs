using Leopotam.EcsLite;
using System.Xml.Linq;

namespace EcsUI.Parsing.Subparsing;

internal class StateParser : BaseParser<DisabledMarker>
{
    public StateParser(EcsWorld ecsWorld) : base(ecsWorld) 
    { 
    }

    public override void Parse(XElement element, int entity)
    {
        var enabledAttribute = element.Attribute("enabled");

        if (enabledAttribute == null)
            return;
        else if (enabledAttribute.Value == "false" && !Pool.Has(entity))
            Pool.Add(entity);
        else if (enabledAttribute.Value == "true" && Pool.Has(entity))
            Pool.Del(entity);
    }
}