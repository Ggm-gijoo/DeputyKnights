using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoSingleton<GameManager>
{
    [HideInInspector] public float timeScale = 1f;
    [SerializeField] private CinemachineVirtualCamera endVCam;
    [SerializeField] private GameObject resultPanel;

    public void SetTimeScale(float value)
    {
        if (Time.timeScale != timeScale) return;

        Time.timeScale = value;
        timeScale = value;
    }
    public void Victory(Transform vCamFollow = null)
    {
        Debug.Log("½Â¸®!");
        StartCoroutine(ResultAction(vCamFollow, true));
    }

    public void Defeat(Transform vCamFollow = null)
    {
        Debug.Log("ÆÐ¹è...");
        StartCoroutine(ResultAction(vCamFollow, false));
    }
    private IEnumerator ResultAction(Transform vCamFollow, bool isVictory)
    {
        endVCam.Follow = vCamFollow;
        endVCam.Priority = 11;
        SetTimeScale(0.1f);

        yield return new WaitForSecondsRealtime(2f);

        SetTimeScale(0f);
        resultPanel.SetActive(true);
        ResultPanel.Instance.SetMVPIcon();

    }

}
