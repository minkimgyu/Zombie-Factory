using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneController : ISceneControllable
{
    public void ChangeScene(ISceneControllable.SceneName name)
    {
        SceneManager.LoadScene(name.ToString());
    }
}