using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneControl : MonoBehaviour {

    public void SceneSelection (int SceneIndex)
    {
        SceneManager.LoadScene(SceneIndex);
    }

    void Update()
    {
        if (Input.GetKey("escape"))
            Application.Quit();

    }
}
