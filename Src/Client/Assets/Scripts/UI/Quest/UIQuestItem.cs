using Models;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIQuestItem : ListView.ListViewItem
{
    public Text title;
    public Image background;
    public Sprite normalBg;
    public Sprite selectedBg;

    public override void onSelected(bool selected)
    {
        background.overrideSprite = selected ? selectedBg : normalBg;
    }

    public Quest quest;
    private void Start()
    {
        
    }

    public void SetQuestInfo(Quest item)
    {
        this.quest = item;
        if (title != null)
        {
            title.text = quest.Define.Name;
        }
    }
}
