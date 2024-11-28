using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ITarget : IIdentifiable
{
    // 바닥 위치 - 오브젝트의 이동을 위한 위치
    // 시아 위치 - 탐지나 공격을 위한 바라보기 위치

    // 시아 정보, 공격 정보로 사용할 오브젝트 위치 반환
    Transform ReturnSightPoint();
    Transform ReturnTargetPoint();
}