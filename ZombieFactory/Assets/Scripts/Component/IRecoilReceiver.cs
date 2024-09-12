using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IRecoilReceiver
{
    void OnRecoilRequested(Vector2 recoilForce);
}
