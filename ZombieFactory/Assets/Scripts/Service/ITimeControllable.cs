using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ITimeControllable
{
    void ControllTime(bool stop);
}

public class NullTimeControllable : ITimeControllable
{
    public void ControllTime(bool stop) { }
}

public class TimeController : ITimeControllable
{
    public void ControllTime(bool stop) 
    { 
        if(stop == true)
        {
            Time.timeScale = 0;
        }
        else
        {
            Time.timeScale = 1;
        }
    }
}