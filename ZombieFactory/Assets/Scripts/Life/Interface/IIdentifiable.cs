using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IIdentifiable
{
    public enum Type
    {
        Sound,
        Human,
        Zombie
    }

    bool IsOpponent(List<Type> types);
}