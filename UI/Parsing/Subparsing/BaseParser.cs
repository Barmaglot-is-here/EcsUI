using Leopotam.EcsLite;
using System.Xml.Linq;

namespace EcsUI.Parsing.Subparsing;

internal abstract class BaseParser<T> where T : struct
{
    protected readonly EcsPool<T> Pool;

    public BaseParser(EcsWorld ecsWorld)
    {
        Pool = ecsWorld.GetPool<T>();
    }

    public abstract void Parse(XElement element, int entity);
}