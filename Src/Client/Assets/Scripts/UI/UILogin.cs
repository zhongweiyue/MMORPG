using Services;
using SkillBridge.Message;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UILogin : MonoBehaviour
{
    public Button loginBtn;
    public InputField username;
    public InputField password;

    private void Start()
    {
        UserService.Instance.OnLogin = this.OnLogin;
    }

    void OnLogin(Result result, string msg)
    {
        if (result == Result.Success)
        {
            SceneManager.Instance.LoadScene("CharSelect");
        }
        else
        {
            MessageBox.Show(msg,"错误",MessageBoxType.Error);
        }
    }

    public void OnClickLogin()
    {
        if (string.IsNullOrEmpty(this.username.text))
        {
            MessageBox.Show(string.Format("请输入账号"));
            return;
        }
        if (string.IsNullOrEmpty(this.password.text))
        {
            MessageBox.Show(string.Format("请输入密码"));
            return;
        }
        UserService.Instance.SendLogin(this.username.text,this.password.text);
    }

}
