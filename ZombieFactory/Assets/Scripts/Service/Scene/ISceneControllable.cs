using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ISceneControllable
{
    public enum SceneName
    {
        InitScene,
        StartScene,
        PlayScene,
        GameOverScene,
        GameClearScene
    }

    void ChangeScene(SceneName name);
}