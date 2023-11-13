using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TitleSceneManager : MonoBehaviour
{
    private void Update()
    {
        if (Input.touchCount > 0)
        {
            if (Input.GetTouch(0).phase == TouchPhase.Began)
            {
                Debug.Log("aa");
                ToMainScene();
            }
        }
        if (Input.GetMouseButtonDown(0))
        {
            Debug.Log("aa");
            ToMainScene();
        }
    }
    public void ToMainScene()
    {
        Managers.Scene.LoadScene(Define.Scene.Main);
    }
}
