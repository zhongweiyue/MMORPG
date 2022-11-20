using Services;
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

    private void Start()
    {
        GuildService.Instance.OnGuildUpdate = UpdateUI;
        listMain.onItemSelected += OnGuildMemberSelected;
        UpdateUI();
    }

    private void OnDestroy()
    {
        GuildService.Instance.OnGuildUpdate = null;
    }

    void UpdateUI()
    {
        uiInfo.Info = GuildManager.Instance.guildInfo;
        ClearList();
        InitItems();
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

    public void OnClickAppliesList() { }

    public void OnClickLeave() { }

    public void OnClickChat() { }

    public void OnClickKickout() { }

    public void OnClickPromote() { }

    public void OnClickDepose() { }

    public void OnClickTransfer() { }

    public void OnClickSetNotice() { }
}
