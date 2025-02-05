using Leopotam.EcsLite;
using System.Xml.Linq;

namespace EcsUI.Parsing.Subparsing;
internal class PositionParser : BaseParser<PositionComponent>
{
    public PositionParser(EcsWorld ecsWorld) : base(ecsWorld)
    {
    }

    public override void Parse(XElement element, int entity)
    {
        ref PositionComponent component = ref Pool.GetSafe(entity);

        Utils.ParseVector(element, "position_x", "position_y", ref component.Position);
    }
}