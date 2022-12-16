using Models;
using Services;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class UIMain : MonoSingleton<UIMain>
{

    public Text avatarName;
    public Text avatarLevel;

    public UITeam TeamWindow;

     protected override void OnStart()
    {
        UpdateAvatarInfo();
    }

    void UpdateAvatarInfo()
    {
        avatarName.text = string.Format("{0}:{1}", User.Instance.CurrentCharacter.Name, User.Instance.CurrentCharacter.Id);
        avatarLevel.text = User.Instance.CurrentCharacter.Level.ToString();
    }

    //public void BackToCharacterSelect()
    //{
    //    SceneManager.Instance.LoadScene("CharSelect");
    //    UserService.Instance.SendGameLeave();
    //}

    public void OnClickTest()
    {
        UITest uitest = UIManager.Instance.Show<UITest>();
        uitest.SetTitle("这是一个测试标题");
        uitest.OnClose += Uitest_OnClose;
    }

    private void Uitest_OnClose(UIWindow sender, UIWindow.WindowResult result)
    {
        //(sender as UITest).Title
        MessageBox.Show("点击了对话框" + result, "对话框响应结果", MessageBoxType.Information);
    }

    public void OnClickBag() 
    {
        UIBag uiBag = UIManager.Instance.Show<UIBag>();
        uiBag.RefreshGold();
    }

    public void OnClickCharEquip()
    {
        UIManager.Instance.Show<UICharEquip>();
    }

    public void OnClickQuest()
    {
        UIManager.Instance.Show<UIQuestSystem>();
    }

    public void OnClickFriend()
    {
        UIManager.Instance.Show<UIFriends>();
    }

    public void ShowTeamUI(bool show)
    {
        TeamWindow.ShowTeam(show);
    }

    public void OnClickGuild()
    {
        GuildManager.Instance.ShowGuild();
    }

    public void OnClickRide()
    {
        UIManager.Instance.Show<UIRide>();
    }

    public void OnClickSetting()
    {
        UIManager.Instance.Show<UISetting>();
    }

    public void OnClickSkill()
    {

    }
}
