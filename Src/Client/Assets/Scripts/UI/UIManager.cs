using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : Singleton<UIManager>
{
    class UIElement
    {
        public string Resources;
        public bool Cache;
        public GameObject Instance;
    }

    private Dictionary<Type, UIElement> UIResourcesDict = new Dictionary<Type, UIElement>();
    public UIManager()
    {
        UIResourcesDict.Add(typeof(UITest), new UIElement() { Resources = "UI/UITest", Cache = true });
        UIResourcesDict.Add(typeof(UIBag), new UIElement() { Resources = "UI/UIBag", Cache = false });
        UIResourcesDict.Add(typeof(UIShop), new UIElement() { Resources = "UI/UIShop", Cache = false });
        UIResourcesDict.Add(typeof(UICharEquip), new UIElement() { Resources = "UI/UICharEquip", Cache = false });
        UIResourcesDict.Add(typeof(UIQuestSystem), new UIElement() { Resources = "UI/UIQuestSystem", Cache = false });
        UIResourcesDict.Add(typeof(UIQuestDialog), new UIElement() { Resources = "UI/UIQuestDialog", Cache = false });
        UIResourcesDict.Add(typeof(UIFriends), new UIElement() { Resources = "UI/UIFriends", Cache = false });
        UIResourcesDict.Add(typeof(UIGuild), new UIElement() { Resources = "UI/Guild/UIGuild", Cache = false });
        UIResourcesDict.Add(typeof(UIGuildList), new UIElement() { Resources = "UI/Guild/UIGuildList", Cache = false });
        UIResourcesDict.Add(typeof(UIGuildPopNoGuild), new UIElement() { Resources = "UI/Guild/UIGuildPopNoGuild", Cache = false });
        UIResourcesDict.Add(typeof(UIGuildPopCreate), new UIElement() { Resources = "UI/Guild/UIGuildPopCreate", Cache = false });
    }

    ~UIManager() { }

    public T Show<T>()
    {
        Type type = typeof(T);
        if (UIResourcesDict.ContainsKey(type))
        {
            UIElement info = UIResourcesDict[type];
            if (info.Instance != null)
            {
                info.Instance.SetActive(true);
            }
            else
            {
                UnityEngine.Object prefab = Resources.Load(info.Resources);
                if (prefab == null)
                {
                    return default(T);
                }
                info.Instance = (GameObject)GameObject.Instantiate(prefab);
            }
            return info.Instance.GetComponent<T>();
        }
        return default(T);
    }

    public void Close(Type type)
    {
        if (UIResourcesDict.ContainsKey(type))
        {
            UIElement info = UIResourcesDict[type];
            if (info.Cache)
            {
                info.Instance.SetActive(false);
            }
            else
            {
                GameObject.Destroy(info.Instance);
                info.Instance = null;
            }
        }
    }
}
