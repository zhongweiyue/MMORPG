using Entities;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIWorldElementManager : MonoSingleton<UIWorldElementManager>
{

    public GameObject nameBarPrefab;
    private Dictionary<Transform, GameObject> elementDict = new Dictionary<Transform, GameObject>();

    public void AddCharacterNameBar(Transform owner,Character character)
    {
        GameObject goNameBar = Instantiate(nameBarPrefab, transform);
        goNameBar.name = "NameBar" + character.entityId; 
        goNameBar.GetComponent<UIWorldElement>().owner = owner;
        goNameBar.GetComponent<UINameBar>().character = character;
        goNameBar.SetActive(true);
        elementDict[owner] = goNameBar;
    }

    public void RemoveCharacterNameBar(Transform owner)
    {
        if (elementDict.ContainsKey(owner))
        {
            Destroy(elementDict[owner]);
            elementDict.Remove(owner);
        }
    }
}
