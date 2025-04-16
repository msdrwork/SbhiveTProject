using System.Collections.Generic;
using UnityEngine;

public class EventManager
{
    private static EventManager instance = null;
    public static EventManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new EventManager();
            }
            return instance;
        }
    }

    private Dictionary<EventId, List<IEventObserver>> eventObservers;

    public EventManager()
    {
        eventObservers = new Dictionary<EventId, List<IEventObserver>>();
    }

    public void AddEventListener(EventId eventId, IEventObserver observer)
    {
        if (eventObservers.ContainsKey(eventId))
        {
            if (eventObservers[eventId] == null)
            {
                eventObservers[eventId] = new List<IEventObserver>();
            }
        }
        else
        {
            eventObservers[eventId] = new List<IEventObserver>();
        }

        eventObservers[eventId].Add(observer);
    }

    public void RemoveEventListener(EventId eventId, IEventObserver eventObserver)
    {
        if (eventObservers.ContainsKey(eventId))
        {
            int idx = eventObservers[eventId].FindIndex(x => x == eventObserver);

            if (idx != -1)
            {
                eventObservers[eventId].RemoveAt(idx);
            }
        }
    }

    public void SendEvent(EventId eventId, object payload)
    {
        if (eventObservers.ContainsKey(eventId))
        {
            foreach (IEventObserver eventObserver in eventObservers[eventId])
            {
                eventObserver.OnEvent(eventId, payload);
            }
        }
        else
        {
            Debug.LogWarning("No events are observing ID: " + eventId);
        }
    }

    public void Destroy()
    {
        eventObservers.Clear();
        eventObservers = null;
    }
}
