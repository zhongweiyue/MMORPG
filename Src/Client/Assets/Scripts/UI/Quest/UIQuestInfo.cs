using Common.Data;
using Models;
using SkillBridge.Message;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Managers;
public class UIQuestInfo : MonoBehaviour
{
    public Text title;
    public Text[] targets;
    public Text description;
    public UIIconItem rewardItems;
    public Text rewardMoney;
    public Text rewardExp;

    public Button navButton;
    private int npc = 0;

    private void Start()
    {

    }
    public void SetQuestInfo(Quest quest)
    {
        this.title.text = string.Format("[{0}]{1}",quest.Define.Type,quest.Define.Name);
        if (quest.Info == null)
        {
            description.text = quest.Define.Dialog;
        }
        else
        {
            if (quest.Info.Status == SkillBridge.Message.QuestStatus.Complated)
            {
                description.text = quest.Define.DialogFinish;
            }
        }
        rewardMoney.text = quest.Define.RewardGold.ToString();
        rewardExp.text = quest.Define.RewardExp.ToString();
        foreach (var filter in GetComponentsInChildren<ContentSizeFitter>())
        {
            filter.SetLayoutVertical();
        }

        if (quest.Info == null)
        {
            NavManager.Instance.NavTargetNpc = quest.Define.AcceptNPC;
        }
        else if (quest.Info.Status == QuestStatus.Complated) 
        {
            NavManager.Instance.NavTargetNpc = quest.Define.SubmitNPC;
        }
        //this.navButton.gameObject.SetActive(NavManager.Instance.NavTargetNpc > 0);
    }
    private void Update()
    {
        
    }
    public void OnClickAbandon()
    {

    }

    public void OnClickNav() 
    {
        Vector3 pos = NavManager.Instance.GetNpcPos(NavManager.Instance.NavTargetNpc);
        NavManager.Instance.NavState = true;
       // NavManager.Instance.NavTargetNpc = npc;
        User.Instance.currentCharacterObject.StartNav(pos);
        UIManager.Instance.Close<UIQuestSystem>();
    }
}
