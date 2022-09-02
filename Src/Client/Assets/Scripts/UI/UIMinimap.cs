using Models;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class UIMinimap : MonoBehaviour
{
    public BoxCollider minimapBoundingBox;
    public Image arrow;
    public Image miniMap;
    public Text mapName;
    private Transform playerTransform;

    void Start()
    {
        InitMap();
    }

    void InitMap()
    {
        mapName.text = User.Instance.currentMapData.Name;
        if (miniMap.overrideSprite == null)
        {
            miniMap.overrideSprite = MinimapManager.Instance.LoadCurrentMinimap();
        }
        miniMap.SetNativeSize();
        miniMap.transform.localPosition = Vector3.zero;
    }

    void Update()
    {
        if (playerTransform == null)
        {
            playerTransform = MinimapManager.Instance.PlayerTransform;
        }
        if (minimapBoundingBox == null || playerTransform == null) return;
        float realWidth = minimapBoundingBox.bounds.size.x;
        float realHeight = minimapBoundingBox.bounds.size.z;
        float relaX = playerTransform.position.x - minimapBoundingBox.bounds.min.x;
        float relaY = playerTransform.position.z - minimapBoundingBox.bounds.min.z;
        float pivotX = relaX / realWidth;
        float pivotY = relaY / realHeight;
        miniMap.rectTransform.pivot = new Vector2(pivotX, pivotY);
        miniMap.rectTransform.localPosition = Vector2.zero;
        arrow.transform.eulerAngles = new Vector3(0, 0, -playerTransform.eulerAngles.y);

    }
}
