using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UICharInfo : MonoBehaviour
{
    public SkillBridge.Message.NCharacterInfo info;
    public Text charClass;
    public Text charName;
    public Image selectbg;
    public bool Select
    {
        get
        {
            return selectbg.IsActive();
        }
        set
        {
            selectbg.gameObject.SetActive(value);
        }
    }

    void Start()
    {
        if (info!=null)
        {
            charName.text = info.Name;
            charClass.text = info.Class.ToString();
        }
    }

    
}
