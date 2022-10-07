using Common.Data;
using Managers;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIQuestSystem : UIWindow
{
    public Text title;
    public GameObject itemPrefab;

    public TabView Tabs;
    public ListView listMain;
    public ListView listBranch;

    public UIQuestInfo questInfo;
    private bool showAvailabaleList = false;//默认显示进行中，true是可接任务

    private void Start()
    {
        listMain.onItemSelected += OnQuestSelected;
        listBranch.onItemSelected += OnQuestSelected;
        Tabs.OnTabSelect += OnSelectTab;
        RefreshUI();
    }

    void OnSelectTab(int idx)
    {
        showAvailabaleList = idx == 1;//1是可接
        RefreshUI();
    }
    private void OnDestroy()
    {
        //QuestInfoManager.Instance.OnQuestChanged -= RefreshUI;
    }

    void RefreshUI()
    {
        ClearAllQuestList();
        InitAllQuestItems();
    }

    //初始化所有任务列表
    void InitAllQuestItems()
    {
        foreach (var kv in QuestManager.Instance.allQuests)
        {
            if (showAvailabaleList)
            {
                if (kv.Value.Info != null)//已经接了跳过
                    continue;
            }
            else
            {
                if (kv.Value.Info == null)//不可接(已完成)的跳过
                    continue;
            }
            GameObject go = Instantiate(itemPrefab, kv.Value.Define.Type == QuestType.Main ? listMain.transform : listBranch.transform);
            UIQuestItem ui = go.GetComponent<UIQuestItem>();
            ui.SetQuestInfo(kv.Value);
            if (kv.Value.Define.Type == QuestType.Main)
                listMain.AddItem(ui);
            else
                listBranch.AddItem(ui);
        }
    }

    void ClearAllQuestList()
    {
        listMain.RemoveAll();
        listBranch.RemoveAll();
    }

    public void OnQuestSelected(ListView.ListViewItem item)
    {
        UIQuestItem questItem = item as UIQuestItem;
        questInfo.SetQuestInfo(questItem.quest);
    }
}
