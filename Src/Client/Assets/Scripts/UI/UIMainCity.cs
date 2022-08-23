using Models;
using Services;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class UIMainCity : MonoBehaviour
{

    public Text avatarName;
    public Text avatarLevel;

    void Start()
    {
        UpdateAvatarInfo();
    }

    void UpdateAvatarInfo()
    {
        avatarName.text = string.Format("{0}:{1}", User.Instance.CurrentCharacter.Name, User.Instance.CurrentCharacter.Id);
        avatarLevel.text = User.Instance.CurrentCharacter.Level.ToString();
    }

    public void BackToCharacterSelect()
    {
        SceneManager.Instance.LoadScene("CharSelect");
        UserService.Instance.SendGameLeave();
    }
}
