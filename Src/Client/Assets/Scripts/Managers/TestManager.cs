using Common.Data;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestManager : Singleton<TestManager>
{
    public void Init()
    {
        NpcManager.Instance.RegisterNpcEvent(NpcFunction.InvokeShop, OnNpcInvokeShop);
        NpcManager.Instance.RegisterNpcEvent(NpcFunction.InvokeInsrance, OnNpcInvokeShop);
    }

    private bool OnNpcInvokeShop(NpcDefine npc)
    {
        Debug.LogFormat("TestManager.OnNpcInvokeShop Npc:[{0} {1}] Type:{2} Func:{3}", npc.ID, npc.Name, npc.Type, npc.Function);
        UITest uiTest = UIManager.Instance.Show<UITest>();
        uiTest.SetTitle(npc.Name);
        return true;
    }

    private bool OnNpcInvokeInsrance(NpcDefine npc)
    {

        Debug.LogFormat("TestManager.OnNpcInvokeInsrance Npc:[{0} {1}] Type:{2} Func:{3}", npc.ID, npc.Name, npc.Type, npc.Function);
        MessageBox.Show("点击了NPC：" + npc.Name, "NPC对话");
        return true;
    }
}
