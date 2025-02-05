using Leopotam.EcsLite;
using System.Xml.Linq;

namespace EcsUI.Parsing.Subparsing;

internal class SizeParser : BaseParser<SizeComponent>
{
    public SizeParser(EcsWorld ecsWorld) : base(ecsWorld)
    {
    }

    public override void Parse(XElement element, int entity)
    {
        ref SizeComponent component = ref Pool.GetSafe(entity);

        Utils.ParseVector(element, "width", "height", ref component.Size);
    }
}