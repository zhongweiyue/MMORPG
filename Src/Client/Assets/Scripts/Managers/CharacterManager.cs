using Entities;
using SkillBridge.Message;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class CharacterManager : Singleton<CharacterManager>, IDisposable
{

    public Dictionary<int, Character> CharactersDict = new Dictionary<int, Character>();
    public UnityAction<Character> OnCharacterEnterAction;

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
        CharactersDict.Clear();
    }
    public void AddCharacter(NCharacterInfo cha)
    {
        Debug.LogFormat("AddCharacter: characterId:{0},characterName:{1},Mapid:{2},Entity:{3}", cha.Id, cha.Name, cha.mapId, cha.Entity.ToString());
        Character character = new Character(cha);
        CharactersDict[cha.Id] = character;
        if (OnCharacterEnterAction != null)
        {
            OnCharacterEnterAction(character);
        }
    }

    public void RemoveCharacter(int characterId)
    {
        Debug.LogFormat("RemoveCharacter:{0}", characterId);
        CharactersDict.Remove(characterId);
    }

}
