﻿using Common;
using Common.Utils;
using GameServer.Entities;
using GameServer.Managers;
using GameServer.Services;
using SkillBridge.Message;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameServer.Models
{
    class Guild
    {
        public int Id { get { return this.Data.Id; } }
        private Character Leader;
        public string Name { get { return this.Data.Name; } }
        public List<Character> Members = new List<Character>();
        public double timestamp;
        public TGuild Data;
        public Guild(TGuild guild) { this.Data = guild; }

        internal bool JoinApply(NGuildApplyInfo apply)
        {
            var oldApply = this.Data.Applies.FirstOrDefault(v => v.CharacterId == apply.characterId);
            if (oldApply != null)
            {
                return false;
            }
            var dbApply = DBService.Instance.Entities.TGuildApplies.Create();
            dbApply.GuildId = apply.GuildId;
            dbApply.CharacterId = apply.characterId;
            dbApply.Name = apply.Name;
            dbApply.Class = apply.Class;
            dbApply.Level = apply.Level;
            dbApply.ApplyTime = DateTime.Now;
            DBService.Instance.Entities.TGuildApplies.Add(dbApply);
            this.Data.Applies.Add(dbApply);
            DBService.Instance.Save();
            this.timestamp = TimeUtil.timestamp;
            return true;
        }

        internal bool JoinAppove(NGuildApplyInfo apply)//审批
        {
            var oldApply = Data.Applies.FirstOrDefault(v => v.CharacterId == apply.characterId && v.Result == 0);//获取审批记录，result为0表示没申请过
            if (oldApply == null)
            {
                return false;
            }
            oldApply.Result = (int)apply.Result;
            if (apply.Result == ApplyResult.Accept)
            {
                this.AddMember(apply.characterId, apply.Name, apply.Class, apply.Level, GuildTitle.None);
            }
            DBService.Instance.Save();
            this.timestamp = TimeUtil.timestamp;
            return true;
        }

        public void AddMember(int id, string name, int @class, int level, GuildTitle title)
        {
            DateTime now = DateTime.Now;
            TGuildMember dbMember = new TGuildMember
            {
                CharacterId = id,
                Name = name,
                Class = @class,
                Level = level,
                Title = (int)title,
                JoinTime = now,
                LastTime = now
            };
            this.Data.Members.Add(dbMember);
            timestamp = TimeUtil.timestamp;
        }

        public void Leave(Character member)
        {
            Log.InfoFormat("Leave Guild:{0} {1}", member.Id, member.Info.Name);
            this.Members.Remove(member);
            if (member == Leader)
            {
                if (this.Members.Count > 0)
                    Leader = Members[0];
                else
                    Leader = null;
            }
            member.Guild = null;
            timestamp = Time.timestamp;
        }

        public void PostProcess(Character from, NetMessageResponse message)
        {
            if (message.Guild == null)
            {
                message.Guild = new GuildResponse();
                message.Guild.Result = Result.Success;
                message.Guild.guildInfo = GuildInfo(from);
            }
        }

        public NGuildInfo GuildInfo(Character from)
        {
            NGuildInfo info = new NGuildInfo
            {
                Id = this.Id,
                GuildName = this.Name,
                Notice = this.Data.Notice,
                leaderId = this.Data.LeaderID,
                leaderName = this.Data.LeaderName,
                createTime = (long)TimeUtil.GetTimestamp(this.Data.CreateTime),
                memberCount = this.Data.Members.Count
            };
            if (from != null)
            {
                info.Members.AddRange(GetMemberInfos());
                if (from.Id == this.Data.LeaderID)
                {
                    info.Applies.AddRange(GetApplyInfos());
                }
            }
            return info;
        }

        List<NGuildMemberInfo> GetMemberInfos()
        {
            List<NGuildMemberInfo> members = new List <NGuildMemberInfo>();
            foreach (var member in Data.Members)
            {
                var memberInfo = new NGuildMemberInfo
                {
                    Id = member.Id,
                    characterId = member.CharacterId,
                    Title = (GuildTitle)member.Title,
                    joinTime = (long)TimeUtil.GetTimestamp(member.JoinTime),
                    lastTime = (long)TimeUtil.GetTimestamp(member.LastTime),
                };
                //应该增加更多检查
                var character = CharacterManager.Instance.GetCharacter(member.CharacterId);
                if (character != null)
                {
                    memberInfo.Info = character.GetBasicInfo();
                    memberInfo.Status = 1;
                    member.Level = character.Data.Level;
                    member.Name = character.Data.Name;
                    member.LastTime = DateTime.Now;
                    if (member.Id == Data.LeaderID)
                        Leader = character;
                }
                else
                {
                    memberInfo.Info = this.GetMemberInfo(member);
                    memberInfo.Status = 0;
                    if (member.Id == Data.LeaderID)
                        this.Leader = null;

                }
                members.Add(memberInfo);
            }
            return members;
        }

        NCharacterInfo GetMemberInfo(TGuildMember member)
        {
            return new NCharacterInfo
            {
                Id = member.CharacterId,
                Name = member.Name,
                Class = (CharacterClass)member.Class,
                Level = member.Level,
            };
        }

        List<NGuildApplyInfo> GetApplyInfos()
        {
            List<NGuildApplyInfo> applies = new List<NGuildApplyInfo>();
            foreach (var apply in Data.Applies)
            {
                applies.Add(new NGuildApplyInfo()
                {
                    characterId = apply.CharacterId,
                    GuildId = apply.GuildId,
                    Class = apply.Class,
                    Level = apply.Level,
                    Name = apply.Name,
                    Result = (ApplyResult)apply.Result
                });
            }
            return applies;
        }
    }
}


