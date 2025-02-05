using Leopotam.EcsLite;
using System.Xml.Linq;

namespace EcsUI.Parsing.Subparsing;

internal class IDParser : BaseParser<IDComponent>
{
    public IDParser(EcsWorld ecsWorld) : base(ecsWorld) 
    { 
    }
    
    public override void Parse(XElement element, int entity)
    {
        var attribute = element.Attribute("id");

        if (attribute == null)
            return;

        ref IDComponent component = ref Pool.Add(entity);

        component.ID = attribute.Value;
    }
}