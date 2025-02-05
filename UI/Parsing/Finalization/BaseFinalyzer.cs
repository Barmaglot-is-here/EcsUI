using Leopotam.EcsLite;

namespace EcsUI.Parsing.Finalization;

internal abstract class BaseFinalyzer
{
    public abstract void FinalyzeText(EcsWorld ecsWorld, int entity);
}