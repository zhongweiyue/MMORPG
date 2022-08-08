using Entities;
using Models;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameObjectManager : MonoBehaviour
{
    Dictionary<int, GameObject> CharacterDict = new Dictionary<int, GameObject>();
    private void Start()
    {
        StartCoroutine(InitGameObjects());
        CharacterManager.Instance.OnCharacterEnterAction = OnCharacterEnter;
    }

    private void OnCharacterEnter(Character cha)
    {
        CreateCharacterObject(cha);
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
        if (!CharacterDict.ContainsKey(cha.Info.Id) || CharacterDict[cha.Info.Id] == null)
        {
            Object obj = Resloader.Load<Object>(cha.Define.Resource);
            if (obj == null)
            {
                Debug.LogFormat("CharacterID:{0} Resource:{1} not exit", cha.Define.TID, cha.Define.Resource);
                return;
            }
            GameObject go = (GameObject)Instantiate(obj);
            go.name = "character_" + cha.Info.Id + "_" + cha.Info.Name;
            go.transform.position = GameObjectTool.LogicToWorld(cha.position);
            go.transform.forward = GameObjectTool.LogicToWorld(cha.direction);
            CharacterDict[cha.Info.Id] = go;
            EntityController entityController = go.GetComponent<EntityController>();
            if (entityController != null)
            {
                entityController.entity = cha;
                entityController.isPlayer = cha.IsPlayer;
            }

            PlayerInputController playerInputController = go.GetComponent<PlayerInputController>();
            if (playerInputController != null)
            {
                if (cha.Info.Id == User.Instance.CurrentCharacter.Id)
                {
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
            UIWorldElementManager.Instance.AddCharacterNameBar(go.transform, cha);
        }
    }

    private void OnDestroy()
    {
        CharacterManager.Instance.OnCharacterEnterAction = null;
    }

   
}
