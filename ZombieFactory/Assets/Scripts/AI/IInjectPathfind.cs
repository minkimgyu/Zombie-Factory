using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IInjectPathfind
{
   void AddPathfind(Func<Vector3, Vector3, List<Vector3>> FindPath);
}
