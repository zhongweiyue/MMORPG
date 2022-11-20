using SkillBridge.Message;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIGuildInfo : MonoBehaviour
{
    public Text guildName;
    public Text guildID;
    public Text leader;

    public Text notice;
    public Text memberNumber;

    private NGuildInfo info;
    public NGuildInfo Info
    {
        get { return this.info; }
        set { this.info = value; this.UpdateUI(); }
    }

    void UpdateUI()
    {
        if (info == null)
        {
            guildName.text = "无";
            guildID.text = "ID:0";
            leader.text = "会长：无";
            notice.text = "";
            memberNumber.text = string.Format("成员数量：0/{0}", GameDefine.GuildMaxMemberCount);
        }
        else
        {
            guildName.text = Info.GuildName;
            guildID.text = "ID:"+this.Info.Id;
            leader.text = "会长:"+this.Info.leaderName;
            notice.text = Info.Notice;
            memberNumber.text = string.Format("成员数量：{0}/{1}", this.Info.memberCount,GameDefine.GuildMaxMemberCount);
        }
    }
}
