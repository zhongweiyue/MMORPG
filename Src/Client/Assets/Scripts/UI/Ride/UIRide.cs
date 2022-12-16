using Models;
using SkillBridge.Message;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIRide : UIWindow
{
    public Text descript;
    public GameObject itemPrefab;
    public ListView listMain;
    private UIRideItem selectedItem;

    private void Start()
    {
        RefreshUI();
        listMain.onItemSelected += this.OnItemSelected;
    }

    private void OnDestroy()
    {
        
    }

    public void OnItemSelected(ListView.ListViewItem item) 
    {
        selectedItem = item as UIRideItem;
        descript.text = selectedItem.item.Define.Description;
    }

    void RefreshUI() 
    {
        ClearItems();
        InitItems();
    }

    void InitItems() 
    {
        foreach (var kv in ItemManager.Instance.ItemsDict)
        {
            if (kv.Value.Define.Type == ItemType.Ride && (kv.Value.Define.LimitClass == CharacterClass.None || kv.Value.Define.LimitClass == User.Instance.CurrentCharacter.Class)) 
            {
                GameObject go = Instantiate(itemPrefab, listMain.transform);
                UIRideItem ui = go.GetComponent<UIRideItem>();
                ui.SetRideItem(kv.Value, this);
                listMain.AddItem(ui);
            }
        }
    }

    void ClearItems() 
    {
        listMain.RemoveAll();
    }

    public void DoRide() 
    {
        if (selectedItem == null) 
        {
            MessageBox.Show("请选择要召唤的坐骑", "提示");
            return;
        }
        User.Instance.Ride(selectedItem.item.Id);
    }
}
