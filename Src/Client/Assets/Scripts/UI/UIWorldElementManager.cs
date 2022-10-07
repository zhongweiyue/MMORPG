using Entities;
using Managers;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIWorldElementManager : MonoSingleton<UIWorldElementManager>
{

    public GameObject nameBarPrefab;
    public GameObject npcStatusPrefab;
    private Dictionary<Transform, GameObject> elementNameDict = new Dictionary<Transform, GameObject>();
    private Dictionary<Transform, GameObject> elementStatusDict = new Dictionary<Transform, GameObject>();

    public void AddCharacterNameBar(Transform owner,Character character)
    {
        GameObject goNameBar = Instantiate(nameBarPrefab, transform);
        goNameBar.name = "NameBar" + character.entityId; 
        goNameBar.GetComponent<UIWorldElement>().owner = owner;
        goNameBar.GetComponent<UINameBar>().character = character;
        goNameBar.SetActive(true);
        elementNameDict[owner] = goNameBar;
    }

    public void RemoveCharacterNameBar(Transform owner)
    {
        if (elementNameDict.ContainsKey(owner))
        {
            Destroy(elementNameDict[owner]);
            elementNameDict.Remove(owner);
        }
    }

    public void AddNpcQuestStatus(Transform owner, NpcQuestStatus status)
    {
        if (elementStatusDict.ContainsKey(owner))
        {
            elementStatusDict[owner].GetComponent<UIQuestStatus>().SetQuestStatus(status);
        }
        else
        {
            GameObject go = Instantiate(npcStatusPrefab, transform);
            go.name = "NpcQuestStatus" + owner.name;
            go.GetComponent<UIWorldElement>().owner = owner;
            go.GetComponent<UIQuestStatus>().SetQuestStatus(status);
            go.SetActive(true);
            elementStatusDict[owner] = go;
        }
    }

    public void RemoveNpcQuestStatus(Transform owner)
    {
        if (elementStatusDict.ContainsKey(owner))
        {
            Destroy(elementStatusDict[owner]);
            elementStatusDict.Remove(owner);
        }
    }
}
