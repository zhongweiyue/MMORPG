using Models;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIQuestInfo : MonoBehaviour
{
    public Text title;
    public Text[] targets;
    public Text description;
    public UIIconItem rewardItems;
    public Text rewardMoney;
    public Text rewardExp;

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
    }
    private void Update()
    {
        
    }
    public void OnClickAbandon()
    {

    }
}
