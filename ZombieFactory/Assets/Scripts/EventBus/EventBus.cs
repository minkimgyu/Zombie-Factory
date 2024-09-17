using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IEvent { }

public class EventBinding<T> where T : IEvent
{
    Action<T> onEvent = (value) => { };
    public Action<T> OnEvent { get { return onEvent; } }

    public EventBinding(Action<T> onEvent) => this.onEvent = onEvent;

    public void Add(Action<T> onEvent) => this.onEvent += onEvent;
    public void Remove(Action<T> onEvent) => this.onEvent -= onEvent;
}

public class EventBus<Name, Type> where Type : IEvent
{
    Dictionary<Name, HashSet<EventBinding<Type>>> bindings = new Dictionary<Name, HashSet<EventBinding<Type>>>();

    public void Register(Name key, EventBinding<Type> binding)
    {
        if (!bindings.ContainsKey(key))
            bindings[key] = new HashSet<EventBinding<Type>>();

        bindings[key].Add(binding);
    }

    public void Deregister(Name key, EventBinding<Type> binding)
    {
        if (bindings.ContainsKey(key))
            bindings[key].Remove(binding);
    }

    public void Raise(Name key, Type @event)
    {
        if (bindings.ContainsKey(key))
        {
            foreach (var binding in bindings[key])
            {
                binding.OnEvent?.Invoke(@event);
            }
        }
    }

    public void Clear()
    {
        bindings.Clear();
    }
}