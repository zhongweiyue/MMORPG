using SkillBridge.Message;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UITeamItem : ListView.ListViewItem
{
    public Text nickname;
    public Image classIcon;
    public Image leaderIcon;
    public Image background;

    public override void onSelected(bool selected)
    {
        this.background.enabled = selected ? true : false;
    }

    public int idx;
    public NCharacterInfo Info;

    private void Start()
    {
        background.enabled = false;
    }

    public void SetMemberInfo(int idex, NCharacterInfo item, bool isLeader)
    {
        this.idx = idex;
        this.Info = item;
        if (nickname != null) nickname.text = this.Info.Level.ToString().PadRight(4) + this.Info.Name;
        if (classIcon != null) classIcon.overrideSprite = SpriteManager.Instance.classIcons[(int)this.Info.Class - 1];
        if (leaderIcon != null) leaderIcon.gameObject.SetActive(isLeader);
    }
}
