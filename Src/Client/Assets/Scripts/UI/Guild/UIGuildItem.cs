using SkillBridge.Message;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIGuildItem : ListView.ListViewItem
{
    public Text GuildId;
    public Text GuildName;
    public Text MemberNum;
    public Text Leader;

    public Image background;
    public Sprite normalBg;
    public Sprite selectedBg;

    public override void onSelected(bool selected)
    {
        background.overrideSprite = selected ? selectedBg : normalBg;
    }

    public NGuildInfo Info;

    public void setGuildInfo(NGuildInfo item)
    {
        this.Info = item;
        if (GuildId != null) GuildId.text = Info.Id.ToString();
        if (GuildName != null) GuildName.text = Info.GuildName;
        if (MemberNum != null) MemberNum.text = Info.memberCount.ToString();
        if (Leader != null) Leader.text = Info.leaderName;
    }
}
