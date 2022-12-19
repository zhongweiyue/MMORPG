using Services;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UISetting : UIWindow
{
    public void ExitToCharSelect() 
    {
        SoundManager.Instance.PlayMusic(SoundDefine.Music_Select);
        SceneManager.Instance.LoadScene("CharSelect");
        UserService.Instance.SendGameLeave();
    }

    public void SystemConfig() 
    {
        UIManager.Instance.Show<UISystemConfig>();
        this.Close();
    }

    public void ExitGame() 
    {
        UserService.Instance.SendGameLeave(true);
    }
}
