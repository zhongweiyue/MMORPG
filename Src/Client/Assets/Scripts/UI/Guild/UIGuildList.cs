using Services;
using SkillBridge.Message;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIGuildList : UIWindow
{
    public GameObject itemPrefab;
    public ListView listMain;
    public Transform itemRoot;
    public UIGuildInfo uiInfo;
    public UIGuildItem selectedItem;

    private void Start()
    {
        this.listMain.onItemSelected += this.OnGuildMemberSelected;
        this.uiInfo.Info = null;
        GuildService.Instance.OnGuildListResult += UpdateGuildList;
        GuildService.Instance.SendGuildListRequest();
    }

    private void OnDestroy()
    {
        GuildService.Instance.OnGuildListResult -= UpdateGuildList;
    }

    void UpdateGuildList(List<NGuildInfo> guilds)
    {
        ClearList();
        InitItems(guilds);
    }

    public void OnGuildMemberSelected(ListView.ListViewItem item)
    {
        selectedItem = item as UIGuildItem;
        uiInfo.Info = selectedItem.Info;
    }

    void InitItems(List<NGuildInfo> guilds)
    {
        foreach (var item in guilds)
        {
            GameObject go = Instantiate(itemPrefab, listMain.transform);
            UIGuildItem ui = go.GetComponent<UIGuildItem>();
            ui.setGuildInfo(item);
            listMain.AddItem(ui);
        }
    }

    void ClearList()
    {
        listMain.RemoveAll();
    }

    public void OnClickJoin()
    {
        if (selectedItem == null)
        {
            MessageBox.Show("请选择要加入的公会");
            return;
        }
        else
        {
            MessageBox.Show(string.Format("确定要加入公会【{0}】么?", selectedItem.Info.GuildName), "申请加入公会", MessageBoxType.Confirm, "申请加入", "取消").OnYes=()=>{
                GuildService.Instance.SendGuildJoinRequest(selectedItem.Info.Id);
            };
        }
    }
}
