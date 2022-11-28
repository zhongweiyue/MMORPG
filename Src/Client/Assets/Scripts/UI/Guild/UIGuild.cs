using Services;
using SkillBridge.Message;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIGuild : UIWindow
{
    public GameObject itemPrefab;
    public ListView listMain;
    public Transform itemRoot;
    public UIGuildInfo uiInfo;
    public UIGuildMemberItem selectedItem;

    public GameObject panelAdmin;
    public GameObject panelLeader;

    private void Start()
    {
        GuildService.Instance.OnGuildUpdate += UpdateUI;
        listMain.onItemSelected += OnGuildMemberSelected;
        UpdateUI();
    }

    private void OnDestroy()
    {
        GuildService.Instance.OnGuildUpdate -= UpdateUI;
    }

    void UpdateUI()
    {
        uiInfo.Info = GuildManager.Instance.guildInfo;
        ClearList();
        InitItems();
        this.panelAdmin.SetActive(GuildManager.Instance.myMemberInfo.Title > GuildTitle.None);
        this.panelLeader.SetActive(GuildManager.Instance.myMemberInfo.Title == GuildTitle.President);
    }

    public void OnGuildMemberSelected(ListView.ListViewItem item)
    {
        selectedItem = item as UIGuildMemberItem;
    }

    void InitItems()
    {
        foreach (var item in GuildManager.Instance.guildInfo.Members)
        {
            GameObject go = Instantiate(itemPrefab, listMain.transform);
            UIGuildMemberItem ui = go.GetComponent<UIGuildMemberItem>();
            ui.setGuildMemberInfo(item);
            listMain.AddItem(ui);
        }
    }

    void ClearList()
    {
        listMain.RemoveAll();
    }

    public void OnClickAppliesList()
    {
        UIManager.Instance.Show<UIGuildApplyList>();
    }

    public void OnClickLeave()
    {
        GuildService.Instance.SendGuildLeaveRequest();
    }
    
    public void OnClickChat() { }

    public void OnClickKickout()
    {
        if (selectedItem == null)
        {
            MessageBox.Show("请选择要踢出的成员");
            return;
        }
        MessageBox.Show(string.Format("要踢【{0}】出公会么", this.selectedItem.Info.Info.Name), "踢出公会", MessageBoxType.Confirm).OnYes = () =>
        {
            GuildService.Instance.SendAdminCommand(GuildAdminCommand.Kickout, this.selectedItem.Info.Info.Id);
        };
    }

    public void OnClickPromote()
    {
        if (selectedItem == null)
        {
            MessageBox.Show("请选择要晋升的成员");
            return;
        }
        if (selectedItem.Info.Title != GuildTitle.None)
        {
            MessageBox.Show("对方已经身份尊贵");
            return;
        }
        MessageBox.Show(string.Format("要晋升【{0}】为副会长么", this.selectedItem.Info.Info.Name), "晋升", MessageBoxType.Confirm).OnYes = () =>
        {
            GuildService.Instance.SendAdminCommand(GuildAdminCommand.Promote, this.selectedItem.Info.Info.Id);
        };
    }

    public void OnClickDepose()
    {
        if (selectedItem == null)
        {
            MessageBox.Show("请选择要罢免的成员");
            return;
        }
        if (selectedItem.Info.Title == GuildTitle.None)
        {
            MessageBox.Show("对方无职位可罢免");
            return;
        }
        MessageBox.Show(string.Format("要罢免【{0}】的公会职务么", this.selectedItem.Info.Info.Name), "罢免", MessageBoxType.Confirm).OnYes = () =>
        {
            GuildService.Instance.SendAdminCommand(GuildAdminCommand.Depost, this.selectedItem.Info.Info.Id);
        };
    }

    public void OnClickTransfer()
    {
        if (selectedItem == null)
        {
            MessageBox.Show("请选择要把会长转让给的成员");
            return;
        }
        MessageBox.Show(string.Format("要把会长转给【{0}】么", this.selectedItem.Info.Info.Name), "转让会长", MessageBoxType.Confirm).OnYes = () =>
        {
            GuildService.Instance.SendAdminCommand(GuildAdminCommand.Transfer, this.selectedItem.Info.Info.Id);
        };
    }

    public void OnClickSetNotice() { }
}
