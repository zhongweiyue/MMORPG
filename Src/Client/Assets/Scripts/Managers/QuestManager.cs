﻿using Models;
using Services;
using SkillBridge.Message;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

namespace Managers
{
    public enum NpcQuestStatus
    {
        None = 0,//无任务
        Complete,//拥有已完成可提交任务
        Available,//拥有可接受任务
        Incomplete,//拥有未完成任务
    }

    public class QuestManager : Singleton<QuestManager>
    {
        //所有有效任务
        public List<NQuestInfo> questInfos;
        public Dictionary<int, Quest> allQuests = new Dictionary<int, Quest>();
        public Dictionary<int, Dictionary<NpcQuestStatus, List<Quest>>> npcQuests = new Dictionary<int, Dictionary<NpcQuestStatus, List<Quest>>>();
        public UnityAction<Quest> onQuestStatusChanged;

        public void Init(List<NQuestInfo> quests)
        {
            this.questInfos = quests;
            allQuests.Clear();
            npcQuests.Clear();
            InitQuests();
        }

        void InitQuests()
        {
            //初始化已有任务
            foreach (var info in questInfos)
            {
                Quest quest = new Quest(info);
                allQuests[quest.Info.QuestId] = quest;
            }
            CheckAvailableQuests();
            foreach (var kv in allQuests)
            {
                AddNpcQuest(kv.Value.Define.AcceptNPC, kv.Value);
                AddNpcQuest(kv.Value.Define.SubmitNPC, kv.Value);
            }
        }

        //初始化可用任务
        void CheckAvailableQuests()
        {
            foreach (var kv in DataManager.Instance.Quests)
            {
                if (kv.Value.LimitClass != CharacterClass.None && kv.Value.LimitClass != User.Instance.CurrentCharacter.Class)
                    continue;//不符合职业
                if (kv.Value.LimitLevel > User.Instance.CurrentCharacter.Level)
                    continue;//不符合等级
                if (allQuests.ContainsKey(kv.Key))
                    continue;//任务已存在
                if (kv.Value.PreQuest > 0)
                {
                    Quest preQuest;
                    if (allQuests.TryGetValue(kv.Value.PreQuest, out preQuest))//获取前置任务
                    {
                        if (preQuest.Info == null)
                            continue;//前置任务未接取
                        if (preQuest.Info.Status != QuestStatus.Finished)
                            continue;//前置任务未完成
                    }
                    else
                    {
                        continue;//前置任务未接
                    }
                }
                Quest quest = new Quest(kv.Value);
                allQuests[quest.Define.ID] = quest;
            }
        }

        void AddNpcQuest(int npcId,Quest quest)
        {
            if (!npcQuests.ContainsKey(npcId))
                npcQuests[npcId] = new Dictionary<NpcQuestStatus, List<Quest>>();

            List<Quest> availables;
            List<Quest> complates;
            List<Quest> incompletes;

            if (!npcQuests[npcId].TryGetValue(NpcQuestStatus.Available, out availables))
            {
                availables = new List<Quest>();
                npcQuests[npcId][NpcQuestStatus.Available] = availables;
            }
            if (!npcQuests[npcId].TryGetValue(NpcQuestStatus.Complete, out complates))
            {
                complates = new List<Quest>();
                npcQuests[npcId][NpcQuestStatus.Complete] = complates;
            }
            if (!npcQuests[npcId].TryGetValue(NpcQuestStatus.Incomplete, out incompletes))
            {
                incompletes = new List<Quest>();
                npcQuests[npcId][NpcQuestStatus.Incomplete] = incompletes;
            }
            if (quest.Info == null)//还没接
            {
                if (npcId == quest.Define.AcceptNPC && !npcQuests[npcId][NpcQuestStatus.Available].Contains(quest))
                {
                    npcQuests[npcId][NpcQuestStatus.Available].Add(quest);
                }
            }
            else
            {
                if (npcId == quest.Define.SubmitNPC && quest.Info.Status == QuestStatus.Complated)
                {
                    if (!npcQuests[npcId][NpcQuestStatus.Complete].Contains(quest))
                    {
                        npcQuests[npcId][NpcQuestStatus.Complete].Add(quest);
                    }
                }

                if (npcId == quest.Define.SubmitNPC && quest.Info.Status == QuestStatus.InProgress)
                {
                    if (!npcQuests[npcId][NpcQuestStatus.Incomplete].Contains(quest))
                    {
                        npcQuests[npcId][NpcQuestStatus.Incomplete].Add(quest);
                    }
                }
            }
        }

        //获取NPC任务状态
        public NpcQuestStatus GetQuestStatusByNpc(int npcId)
        {
            Dictionary<NpcQuestStatus, List<Quest>> status = new Dictionary<NpcQuestStatus, List<Quest>>();
            if (npcQuests.TryGetValue(npcId, out status))//获取NPC任务,顺序是已完成，可接，进行中
            {
                if (status[NpcQuestStatus.Complete].Count > 0)
                    return NpcQuestStatus.Complete;
                if (status[NpcQuestStatus.Available].Count > 0)
                    return NpcQuestStatus.Available;
                if (status[NpcQuestStatus.Incomplete].Count > 0)
                    return NpcQuestStatus.Incomplete;
            }
            return NpcQuestStatus.None;
        }

        public bool OpenNpcQuest(int npcId)
        {
            Dictionary<NpcQuestStatus, List<Quest>> status = new Dictionary<NpcQuestStatus, List<Quest>>();
            if (npcQuests.TryGetValue(npcId, out status))//获取NPC任务,顺序是已完成，可接，进行中
            {
                if (status[NpcQuestStatus.Complete].Count > 0)
                    return ShowQuestDialog(status[NpcQuestStatus.Complete].First());
                if (status[NpcQuestStatus.Available].Count > 0)
                    return ShowQuestDialog(status[NpcQuestStatus.Available].First());
                if (status[NpcQuestStatus.Incomplete].Count > 0)
                    return ShowQuestDialog(status[NpcQuestStatus.Incomplete].First());
            }
            return false;
        }

        bool ShowQuestDialog(Quest quest)
        {
            if (quest.Info == null || quest.Info.Status == QuestStatus.Complated)
            {
                UIQuestDialog dlg = UIManager.Instance.Show<UIQuestDialog>();
                dlg.SetQuest(quest);
                dlg.OnClose += OnQuestDialogClose;
                return true;
            }
            if (quest.Info != null || quest.Info.Status == QuestStatus.Complated)
            {
                if (!string.IsNullOrEmpty(quest.Define.DialogIncomplete))
                    MessageBox.Show(quest.Define.DialogIncomplete);
            }
            return true;
        }

        void OnQuestDialogClose(UIWindow sender, UIWindow.WindowResult result)
        {
            UIQuestDialog dlg = (UIQuestDialog)sender;
            if (result == UIWindow.WindowResult.Yes)
            {
                if (dlg.quest.Info == null)
                    QuestService.Instance.SendQuestAccept(dlg.quest);
                else if (dlg.quest.Info.Status == QuestStatus.Complated)
                    QuestService.Instance.SendQuestSubmit(dlg.quest);
            }
            else if (result == UIWindow.WindowResult.No)
            {
                MessageBox.Show(dlg.quest.Define.DialogDeny);
            }
        }

        Quest RefreshQuestStatus(NQuestInfo quest)
        {
            npcQuests.Clear();
            Quest result;
            if (allQuests.ContainsKey(quest.QuestId))
            {
                //更新新的任务状态
                allQuests[quest.QuestId].Info = quest;
                result = allQuests[quest.QuestId];
            }
            else
            {
                result = new Quest(quest);
                allQuests[quest.QuestId] = result;
            }
            CheckAvailableQuests();
            foreach (var kv in allQuests)
            {
                AddNpcQuest(kv.Value.Define.AcceptNPC, kv.Value);
                AddNpcQuest(kv.Value.Define.SubmitNPC, kv.Value);
            }
            if (onQuestStatusChanged != null)
            {
                onQuestStatusChanged(result);
            }
            return result;
        }

        public void OnQuestAccepted(NQuestInfo info)
        {
            var quest = RefreshQuestStatus(info);
            MessageBox.Show(quest.Define.DialogAccept);
        }

        public void OnQuestSubmited(NQuestInfo info)
        {
            var quest = RefreshQuestStatus(info);
            MessageBox.Show(quest.Define.DialogFinish);
        }
    }
}

