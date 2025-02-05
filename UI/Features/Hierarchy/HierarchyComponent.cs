using Leopotam.EcsLite;

namespace EcsUI;
public struct HierarchyComponent : IEcsAutoReset<HierarchyComponent>
{
    public List<int> Children;

    public void Add(int entity) => Children.Add(entity);

    public void AutoReset(ref HierarchyComponent c)
    {
        c.Children = new();
    }
}