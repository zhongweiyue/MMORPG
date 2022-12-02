using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public abstract class UIWindow : MonoBehaviour
{
    public delegate void CloseHandler(UIWindow sender, WindowResult result);
    public event CloseHandler OnClose;
    public virtual Type Type { get { return this.GetType(); } }

    public GameObject Root;
    public enum WindowResult
    {
        None = 0,
        Yes,
        No
    }

    public void Close(WindowResult result = WindowResult.None)
    {
        UIManager.Instance.Close(Type);
        if (OnClose != null)
        {
            OnClose(this,result);
        }
        OnClose = null;
    }

    public virtual void OnCloseClick()
    {
        Close();
    }

    public virtual void OnYesClick()
    {
        Close(WindowResult.Yes);
    }

    public virtual void OnNoClick()
    {
        Close(WindowResult.No);
    }

    void OnMouseDown()
    {
        Debug.LogFormat(name + "clicked");
    }
}
