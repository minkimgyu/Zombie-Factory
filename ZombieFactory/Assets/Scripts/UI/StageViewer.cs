using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using DG.Tweening;

public class StageViewer : MonoBehaviour
{
    [SerializeField] TMP_Text _stageTitleTxt;
    [SerializeField] TMP_Text _stageClearTxt;
    const float _fadeDuration = 1f;

    public void Initialize()
    {
        _stageTitleTxt.text = "";
        _stageClearTxt.alpha = 0f;
    }

    public void OnStageChange(int stageCount)
    {
        _stageTitleTxt.text = stageCount.ToString();
    }

    public void OnStageClear()
    {
        _stageClearTxt.DOFade(1, _fadeDuration).OnComplete(() => { _stageClearTxt.DOFade(0, _fadeDuration); });
    }
}
