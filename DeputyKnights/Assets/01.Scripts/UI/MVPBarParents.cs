using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MVPBarParents : MonoSingleton<MVPBarParents>
{
    [SerializeField] private GameObject mvpBarPrefab;
    private List<MVPBar> mvpBars = new List<MVPBar>();

    private void Start()
    {
        Init();
    }
    private void Init()
    {
        for (int i = 0; i < TeamManager.Instance.playerTeamList.Count; i++)
        {
            int idx = i;

            mvpBars.Add(Instantiate(mvpBarPrefab, transform).GetComponent<MVPBar>());
            mvpBars[idx].SetCharIcon(idx);
            TeamManager.Instance.OnHitEvent.AddListener(()=>mvpBars[idx].SetMvpBarValue(idx));
        }
    }
}
