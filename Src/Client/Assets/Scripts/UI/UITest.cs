using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UITest : UIWindow
{
    public Text Title;
    public void SetTitle(string title)
    {
        Title.text = title;
    }
}
