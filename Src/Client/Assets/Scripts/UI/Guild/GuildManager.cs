using Models;
using SkillBridge.Message;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GuildManager : Singleton<GuildManager>
{
    public NGuildInfo guildInfo;
    public bool HasGuild
    {
        get { return guildInfo != null; }
    }

    public NGuildMemberInfo myMemberInfo;

    public void Init(NGuildInfo guild)
    {
        guildInfo = guild;
        if (guild == null)
        {
            myMemberInfo = null;
            return;
        }
        foreach (var member in guild.Members)
        {
            if (member.characterId == User.Instance.CurrentCharacter.Id)
            {
                myMemberInfo = member;
                break;
            }
        }
    }

    public void ShowGuild()
    {
        if (HasGuild)
            UIManager.Instance.Show<UIGuild>();
        else
        {
            var win = UIManager.Instance.Show<UIGuildPopNoGuild>();
            win.OnClose += PopNoGuild_OnClose;
        }
    }

    private void PopNoGuild_OnClose(UIWindow sender, UIWindow.WindowResult result)
    {
        if (result == UIWindow.WindowResult.Yes)
        {
            //创建公会
            UIManager.Instance.Show<UIGuildPopCreate>();
        }
        else if (result == UIWindow.WindowResult.No)
        {
            //加入公会
            UIManager.Instance.Show<UIGuildList>();
        }
    }
}
