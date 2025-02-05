using System.Xml.Linq;

namespace EcsUI.Parsing;

internal class ParsingPreset
{
    private readonly List<Action<XElement, int>> _actions;
    private Action<int> _finalizationAction;

    public ParsingPreset()
    {
        _actions = new();
    }

    public ParsingPreset Add(Action<XElement, int> action)
    {
        _actions.Add(action);

        return this;
    }

    public void Finalyze(Action<int> finalizationAction) 
        => _finalizationAction = finalizationAction;

    public void Run(XElement element, int entity)
    {
        foreach (var action in _actions)
            action.Invoke(element, entity);

        _finalizationAction.Invoke(entity);
    }
}