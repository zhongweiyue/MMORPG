using Common.Data;
using SkillBridge.Message;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Models
{
    class User : Singleton<User>
    {
        public MapDefine currentMapData { get; set; }
        public GameObject currentCharacterObject { get; set; }
        public NTeamInfo TeamInfo { get; set; }

        SkillBridge.Message.NUserInfo userInfo;

        public SkillBridge.Message.NUserInfo Info
        {
            get { return userInfo; }
        }


        public void SetupUserInfo(SkillBridge.Message.NUserInfo info)
        {
            this.userInfo = info;
        }

        public SkillBridge.Message.NCharacterInfo CurrentCharacter { get; set; }


        public void AddGold(int gold) 
        {
            this.CurrentCharacter.Gold += gold;
        }
    }
}
