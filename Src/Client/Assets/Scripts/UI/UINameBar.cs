using Entities;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class UINameBar : MonoBehaviour
{
    public Text avatarName;
    public Character character;

    void Start()
    {
        
    }

   
    void Update()
    {
        UpdateInfo();
        transform.forward = Camera.main.transform.forward;
    }

    void UpdateInfo()
    {
        if (character != null)
        {
            string name = string.Format("{0} Lv.{1}", character.Name, character.Info.Level);
            if (name != avatarName.text)
            {
                avatarName.text = name;
            }
        }
    }
}
