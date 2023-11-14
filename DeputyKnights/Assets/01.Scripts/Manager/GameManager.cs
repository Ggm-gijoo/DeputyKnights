using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoSingleton<GameManager>
{
    [HideInInspector] public float timeScale = 1f;
    [SerializeField] private CinemachineVirtualCamera endVCam;

    [Header("결과 패널")]
    [SerializeField] private GameObject resultPanel;
    [Header("일시정지 패널")]
    [SerializeField] private GameObject pausePanel;

    private void Awake()
    {
        Application.targetFrameRate = 60;
    }
    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            if (resultPanel.activeSelf) return;

            if (!pausePanel.activeSelf)
                ActivePausePanel();
            else
                DisablePausePanel();
        }
    }

    public void SetTimeScale(float value)
    {
        if (Time.timeScale != timeScale) return;

        Time.timeScale = value;
        timeScale = value;
    }
    public void Result(Transform vCamFollow = null, bool isVictory = true)
    {
        StartCoroutine(ResultAction(vCamFollow, isVictory));
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
        ResultPanel.Instance.SetMainText(isVictory);
        StageSelect.Instance.SetClear(Base.stageIdx - 1);
    }

    public void Restart() => Managers.Scene.LoadScene(Define.Scene.Ingame, Base.stageIdx);
    public void Exit()
    {
        SetManager.Instance.Clear();
        Managers.Scene.LoadScene(Define.Scene.Main);
    }
    public void Quit() => Application.Quit();

    public void ActivePausePanel()
    {
        Time.timeScale = 0;
        pausePanel.SetActive(true);
    }
    public void DisablePausePanel()
    {
        Time.timeScale = timeScale;
        pausePanel.SetActive(false);
    }

}
