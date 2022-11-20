using Common.Utils;
using SkillBridge.Message;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIGuildMemberItem : ListView.ListViewItem
{
    public Text nickname;
    public Text @class;
    public Text level;
    public Text title;
    public Text joinTime;
    public Text status;

    public Image background;
    public Sprite normalBg;
    public Sprite selectedBg;

    public override void onSelected(bool selected)
    {
        background.overrideSprite = selected ? selectedBg : normalBg;
    }

    public NGuildMemberInfo Info;

    public void setGuildMemberInfo(NGuildMemberInfo item)
    {
        this.Info = item;
        if (nickname != null) nickname.text = Info.Info.Name;
        if (@class != null) @class.text = Info.Info.Class.ToString();
        if (level != null) level.text = Info.Info.Level.ToString();
        if (title != null) title.text = Info.Title.ToString();
        if (joinTime != null) joinTime.text = Info.joinTime.ToString();
        if (status != null) status.text = Info.Status ==1?"在线":TimeUtil.GetTime(Info.lastTime).ToString();
    }
}
