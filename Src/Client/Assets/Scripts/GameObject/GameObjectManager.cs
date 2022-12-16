using Entities;
using Managers;
using Models;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameObjectManager : MonoSingleton<GameObjectManager>
{
    Dictionary<int, GameObject> CharacterDict = new Dictionary<int, GameObject>();
    protected override void OnStart()
    {
        StartCoroutine(InitGameObjects());
        CharacterManager.Instance.OnCharacterEnterAction += OnCharacterEnter;
        CharacterManager.Instance.OnCharacterLeaveAction += OnCharacterLeave;
    }

    private void OnDestroy()
    {
        CharacterManager.Instance.OnCharacterEnterAction -= OnCharacterEnter;
        CharacterManager.Instance.OnCharacterLeaveAction -= OnCharacterLeave;
    }

    private void OnCharacterEnter(Character cha)
    {
        CreateCharacterObject(cha);
    }

    private void OnCharacterLeave(Character cha)
    {
        if (!CharacterDict.ContainsKey(cha.entityId))
            return;
        if (CharacterDict[cha.entityId] != null)
        {
            Destroy(CharacterDict[cha.entityId]);
            CharacterDict.Remove(cha.entityId);
        }
    }
    

    IEnumerator InitGameObjects()
    {
        foreach (var cha in CharacterManager.Instance.CharactersDict.Values)
        {
            CreateCharacterObject(cha);
            yield return null;
        }
    }

    private void CreateCharacterObject(Character cha)
    {
        if (!CharacterDict.ContainsKey(cha.entityId) || CharacterDict[cha.entityId] == null)
        {
            Object obj = Resloader.Load<Object>(cha.Define.Resource);
            if (obj == null)
            {
                Debug.LogFormat("CharacterID:{0} Resource:{1} not exit", cha.Define.TID, cha.Define.Resource);
                return;
            }
            GameObject go = (GameObject)Instantiate(obj,transform);
            go.name = "character_" + cha.Id + "_" + cha.Name;
            CharacterDict[cha.entityId] = go;
            UIWorldElementManager.Instance.AddCharacterNameBar(go.transform, cha);
        }
        initGameObject(CharacterDict[cha.entityId], cha);
    }

    private void initGameObject(GameObject go,Character cha)
    {
        go.transform.position = GameObjectTool.LogicToWorld(cha.position);
        go.transform.forward = GameObjectTool.LogicToWorld(cha.direction);
        EntityController entityController = go.GetComponent<EntityController>();
        if (entityController != null)
        {
            entityController.entity = cha;
            entityController.isPlayer = cha.IsCurrentPlayer;
            entityController.Ride(cha.Info.Ride);
        }

        PlayerInputController playerInputController = go.GetComponent<PlayerInputController>();
        if (playerInputController != null)
        {
            if (cha.IsCurrentPlayer)
            {
                User.Instance.currentCharacterObject = playerInputController;
                MainPlayerCamera.Instance.player = go;
                playerInputController.enabled = true;
                playerInputController.character = cha;
                playerInputController.entityController = entityController;
            }
            else
            {
                playerInputController.enabled = false;
            }
        }
    }

    public RideController LoadRide(int rideId,Transform parent) 
    {
        var rideDefine = DataManager.Instance.Rides[rideId];
        Object obj = Resloader.Load<Object>(rideDefine.Resource);
        if (obj == null) 
        {
            Debug.LogErrorFormat("Ride[{0}] Resource[{1}] not exit", rideDefine.ID, rideDefine.Resource);
            return null;
        }
        GameObject go = (GameObject)Instantiate(obj, parent);
        go.name = "Ride_" + rideDefine.ID + "_" + rideDefine.Name;
        return go.GetComponent<RideController>();
    }
   

   
}
