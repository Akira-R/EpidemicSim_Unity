using System;
using System.Collections.Generic;

public class EventManager
{
    private static EventManager _instance;

    public static EventManager Instance 
    { 
        get
        {
            if (_instance is null)
            {
                _instance = new EventManager();
            }
            return _instance;
        }
    }

    private readonly Dictionary<Type, HashSet<Action<IEvent>>> _listeners;

    public EventManager()
    {
        _listeners = new Dictionary<Type, HashSet<Action<IEvent>>>();
    }

    public void AddListener<T>(Action<IEvent> action) where T : IEvent
    {
        if (!HasListener<T>())
        {
            AddNewListenerType<T>();
        }
        GetListeners<T>().Add(action);
    }

    public void RemoveListener<T>(Action<IEvent> action) where T : IEvent
    {
        if (!HasListener<T>())
        {
            return;
        }
        GetListeners<T>().Remove(action);
    }

    public void Dispatch<T>() where T : IEvent
    {
        Dispatch(default(T));
    }

    public void Dispatch<T>(T eventData) where T : IEvent
    {
        HashSet<Action<IEvent>> listeners = GetListeners<T>(copy: true);

        if (listeners is null)
        {
            return;
        }

        foreach (Action<IEvent> action in listeners)
        {
            action(eventData);
        }
    }

    private void AddNewListenerType<T>() where T : IEvent
    {
        _listeners.Add(typeof(T), new HashSet<Action<IEvent>>());
    }

    private HashSet<Action<IEvent>> GetListeners<T>(bool copy = false)
    {
        if (_listeners.TryGetValue(typeof(T), out var listeners))
        {
            if (copy)
            {
                return new HashSet<Action<IEvent>>(listeners);
            }
            return listeners;
        }
        return null;
    }

    private bool HasListener<T>() => _listeners.ContainsKey(typeof(T));
}
