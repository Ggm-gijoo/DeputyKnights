using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.ResourceLocations;
using UnityEngine.UI;

public class StageSetting : MonoSingleton<StageSetting>
{
    [SerializeField] private AssetLabelReference assetLabel;

    [SerializeField] private Transform[] enemyImageGroup;
    [SerializeField] private Transform[] playerImageGroup;
    [SerializeField] private Transform playerSelectDefaultGroup;

    private int unsettedPlayerCount = 0;

    private void Start()
    {
        SetPlayer();
    }
    private void OnEnable()
    {
        unsettedPlayerCount = 0;
        SetEnemy();
    }
    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            Disable();
            gameObject.SetActive(false);
        }
    }

    public void SetEnemy()
    {
        StageSO stageSO = Managers.Resource.Load<StageSO>($"Stage_{Base.stageIdx}");
        foreach(var pos in stageSO.EnemyPositionDict.Keys)
        {
            foreach(var enemy in stageSO.EnemyPositionDict[pos])
            {
                GameObject clone = Managers.Resource.Instantiate("SelectIcon", enemyImageGroup[(int)pos]);
                clone.GetComponent<SelectIcon>().SetCharSO(enemy);
                Destroy(clone.GetComponent<IconDrag>());

                Image cloneImg = clone.transform.Find("Icon").GetComponent<Image>();
                cloneImg.sprite = enemy.CharIcon;

                clone.transform.localScale = Vector3.one;
            }
        }
    }
    public void SetPlayer()
    {
        unsettedPlayerCount = 0;
        Addressables.LoadAssetsAsync<CharacterSO>(assetLabel, null).Completed +=
            objects =>
            {
                foreach(var character in objects.Result)
                {
                    unsettedPlayerCount++;

                    GameObject clone = Managers.Resource.Instantiate("SelectIcon", playerSelectDefaultGroup);
                    Image cloneIcon = clone.transform.Find("Icon").GetComponent<Image>();

                    clone.GetComponent<SelectIcon>().SetCharSO(character);

                    clone.SetActive(false);
                    clone.SetActive(true);

                    clone.transform.localScale = Vector3.one;
                    cloneIcon.sprite = character.CharIcon;
                }
            };
    }
    public void GetEnemy()
    {
        StageSO stageSO = Managers.Resource.Load<StageSO>($"Stage_{Base.stageIdx}");

        foreach (var enemyPos in stageSO.EnemyPositionDict.Keys)
        {
            foreach (var enemy in stageSO.EnemyPositionDict[enemyPos])
            {
                SetManager.Instance.SetPosition(enemyPos, enemy);
            }
        }
    }
    public void GetPlayer()
    {
        for(int i = 0; i < playerImageGroup.Length; i++)
        {
            foreach(var player in playerImageGroup[i].GetComponentsInChildren<SelectIcon>())
            {
                SetManager.Instance.SetPosition((position)i, player.GetCharSO());
            }
        }
    }

    public bool CheckNotMatchedPlayer()
    {
        return unsettedPlayerCount == playerSelectDefaultGroup.childCount;
    }

    public void Disable()
    {
        foreach(Transform imageGroup in enemyImageGroup)
        {
            foreach(var icon in imageGroup.GetComponentsInChildren<Poolable>())
            {
                Managers.Pool.Push(icon);
            }
        }
    }
}
