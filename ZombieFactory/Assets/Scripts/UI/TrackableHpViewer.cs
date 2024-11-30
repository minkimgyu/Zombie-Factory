using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TrackableHpViewer : HpViewer
{
    [SerializeField] TMP_Text _nameTxt;

    public void ResetName(string name)
    {
        _nameTxt.text = name;
    }

    private void Update()
    {
        transform.LookAt(Camera.main.transform);
    }
}
