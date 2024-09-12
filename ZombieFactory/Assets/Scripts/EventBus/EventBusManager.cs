using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class EventBusManager : Singleton<EventBusManager>
{
    public ObserverEventBus ObserverEventBus { get; private set; }
    //public SubEventBus SubEventBus { get; private set; }

    public void Initialize(ObserverEventBus observerEventBus)
    {
        ObserverEventBus = observerEventBus;
    }
}
