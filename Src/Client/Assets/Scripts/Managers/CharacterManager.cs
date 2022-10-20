using Entities;
using SkillBridge.Message;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

namespace Managers
{
    public class CharacterManager : Singleton<CharacterManager>, IDisposable
    {

        public Dictionary<int, Character> CharactersDict = new Dictionary<int, Character>();
        public UnityAction<Character> OnCharacterEnterAction;
        public UnityAction<Character> OnCharacterLeaveAction;

        public CharacterManager()
        {

        }

        public void Dispose()
        {

        }

        public void Init()
        {

        }
        public void Clear()
        {
            int[] keys = CharactersDict.Keys.ToArray();
            foreach (var key in keys)
            {
                RemoveCharacter(key);
            }
            CharactersDict.Clear();
        }
        public void AddCharacter(NCharacterInfo cha)
        {
            Debug.LogFormat("AddCharacter: characterId:{0},characterName:{1},Mapid:{2},Entity:{3}", cha.Id, cha.Name, cha.mapId, cha.Entity.ToString());
            Character character = new Character(cha);
            CharactersDict[cha.EntityId] = character;
            EntityManager.Instance.AddEntity(character);
            if (OnCharacterEnterAction != null)
            {
                OnCharacterEnterAction(character);
            }
        }

        public void RemoveCharacter(int entityId)
        {
            Debug.LogFormat("RemoveCharacter:{0}", entityId);
            if (CharactersDict.ContainsKey(entityId))
            {
                EntityManager.Instance.RemoveEntity(CharactersDict[entityId].Info.Entity);
                if (OnCharacterLeaveAction != null)
                {
                    OnCharacterLeaveAction(CharactersDict[entityId]);
                }
                CharactersDict.Remove(entityId);
            }
        }
    }
}

