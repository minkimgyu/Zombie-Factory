using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ITarget : IIdentifiable
{
    // �ٴ� ��ġ - ������Ʈ�� �̵��� ���� ��ġ
    // �þ� ��ġ - Ž���� ������ ���� �ٶ󺸱� ��ġ

    // �þ� ����, ���� ������ ����� ������Ʈ ��ġ ��ȯ
    Transform ReturnSightPoint();
    Transform ReturnTargetPoint();
}