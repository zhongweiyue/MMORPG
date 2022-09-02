using Models;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinimapManager : Singleton<MinimapManager>
{
    public Transform PlayerTransform
    {
        get
        {
            if (User.Instance.currentCharacterObject == null)
                return null;
            return User.Instance.currentCharacterObject.transform;
        }
    }

    public Sprite LoadCurrentMinimap()
    {
        return Resloader.Load<Sprite>("UI/Minimap/" + User.Instance.currentMapData.MiniMap);
    }
}
