using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ITarget : IIdentifiable, IPoint
{
    // �ٴ� ��ġ - ������Ʈ�� �̵��� ���� ��ġ
    // �þ� ��ġ - Ž���� ������ ���� �ٶ󺸱� ��ġ

    // �þ� ����, ���� ������ ����� ������Ʈ ��ġ ��ȯ
    //Vector3 ReturnSightPoint();
}