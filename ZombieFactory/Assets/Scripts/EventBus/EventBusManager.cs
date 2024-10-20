using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class EventBusManager : Singleton<EventBusManager>
{
    public MainEventBus MainEventBus { get; private set; }
    public ObserverEventBus ObserverEventBus { get; private set; }

    public void Initialize(MainEventBus mainEventBus, ObserverEventBus observerEventBus)
    {
        MainEventBus = mainEventBus;
        ObserverEventBus = observerEventBus;
    }
}
