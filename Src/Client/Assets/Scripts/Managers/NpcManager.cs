using Common.Data;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NpcManager : Singleton<NpcManager>
{
    public delegate bool NpcActionHandler(NpcDefine npc);
    Dictionary<NpcFunction, NpcActionHandler> eventNpcDict = new Dictionary<NpcFunction, NpcActionHandler>();

    public void RegisterNpcEvent(NpcFunction function, NpcActionHandler action)
    {
        if (!eventNpcDict.ContainsKey(function))
        {
            eventNpcDict[function] = action;
        }
        else
        {
            eventNpcDict[function] += action;
        }
    }

    public NpcDefine GetNpcDefine(int npcId)
    {
        NpcDefine npc = null;
        DataManager.Instance.Npcs.TryGetValue(npcId,out npc);
        return npc;
    }

    public bool Interactive(int npcId)
    {
        if (DataManager.Instance.Npcs.ContainsKey(npcId))
        {
            var npc = DataManager.Instance.Npcs[npcId];
            return Interactive(npc);
        }
        return false;
    }

    public bool Interactive(NpcDefine npc)
    {
        if (npc.Type == NpcType.Task)
        {
            return DoTaskInteractive(npc);
        }
        else if(npc.Type == NpcType.Functional)
        {
            return DoFunctionInteractive(npc);
        }
        return false;
    }

    private bool DoTaskInteractive(NpcDefine npc)
    {
        MessageBox.Show("点击了NPC:" + npc.Name, "NPC对话");
        return true;
    }

    private bool DoFunctionInteractive(NpcDefine npc)
    {
        if (npc.Type != NpcType.Functional)
        {
            return false;
        }
        if (!eventNpcDict.ContainsKey(npc.Function))
        {
            return false;
        }
        return eventNpcDict[npc.Function](npc);
    }
}
