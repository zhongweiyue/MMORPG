using SkillBridge.Message;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIFriendItem : ListView.ListViewItem
{
    public Text nickname;
    public Text @class;
    public Text level;
    public Text status;

    public Image background;
    public Sprite normalBg;
    public Sprite selectedBg;

    public override void onSelected(bool selected)
    {
        this.background.overrideSprite = selected ? selectedBg : normalBg;
    }

    public NFriendInfo Info;
    private void Start()
    {
    }

    public void SetFriendInfo(NFriendInfo item) 
    {
        this.Info = item;
        if (nickname != null) nickname.text = Info.friendInfo.Name;
        if (@class != null) @class.text = Info.friendInfo.Class.ToString();
        if (level != null) level.text = Info.friendInfo.Level.ToString();
        if (status != null) status.text = Info.Status == 1 ? "在线" : "离线";
    }
}
