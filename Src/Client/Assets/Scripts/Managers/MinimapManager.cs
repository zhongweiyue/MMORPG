using Models;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinimapManager : Singleton<MinimapManager>
{
    public UIMinimap minimap;
    private Collider minimapBoundingBox;
    public Collider MinimapBoundingBox
    {
        get
        {
            return minimapBoundingBox;
        }
    }

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

    public void UpdateMinimap(Collider minimapBoundingBox)
    {
        this.minimapBoundingBox = minimapBoundingBox;
        if (minimap != null)
        {
            minimap.UpdateMap();
        }
    }
}
