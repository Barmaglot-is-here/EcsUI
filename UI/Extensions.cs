using Leopotam.EcsLite;

namespace EcsUI;
internal static class Extensions
{
    public static ref T GetSafe<T>(this EcsPool<T> pool, int entity) where T : struct
    {
        ref T component = ref pool.Has(entity)
                        ? ref pool.Get(entity)
                        : ref pool.Add(entity);

        return ref component;
    }
}