using Network;
using SkillBridge.Message;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Services
{
    public class GuildService : Singleton<GuildService>, IDisposable
    {
        public UnityAction OnGuildUpdate;
        public UnityAction<bool> OnGuildCreateResult;
        public UnityAction<List<NGuildInfo>> OnGuildListResult;

        public void Init() { }

        public GuildService()
        {
            MessageDistributer.Instance.Subscribe<GuildCreateResponse>(this.OnGuildCreate);
            MessageDistributer.Instance.Subscribe<GuildListResponse>(this.OnGuildList);
            MessageDistributer.Instance.Subscribe<GuildJoinRequest>(this.OnGuildJoinRequest);
            MessageDistributer.Instance.Subscribe<GuildJoinResponse>(this.OnGuildJoinResponse);
            MessageDistributer.Instance.Subscribe<GuildResponse>(this.OnGuild);
            MessageDistributer.Instance.Subscribe<GuildLeaveResponse>(this.OnGuildLeave);
        }

        public void Dispose()
        {
            MessageDistributer.Instance.Unsubscribe<GuildCreateResponse>(this.OnGuildCreate);
            MessageDistributer.Instance.Unsubscribe<GuildListResponse>(this.OnGuildList);
            MessageDistributer.Instance.Unsubscribe<GuildJoinRequest>(this.OnGuildJoinRequest);
            MessageDistributer.Instance.Unsubscribe<GuildJoinResponse>(this.OnGuildJoinResponse);
            MessageDistributer.Instance.Unsubscribe<GuildResponse>(this.OnGuild);
            MessageDistributer.Instance.Unsubscribe<GuildLeaveResponse>(this.OnGuildLeave);
        }

        //发送创建公会
        public void SendGuildCreate(string guildName, string notice)
        {
            Debug.Log("SendGuildCreate");
            NetMessage message = new NetMessage();
            message.Request = new NetMessageRequest();
            message.Request.guildCreate = new GuildCreateRequest();
            message.Request.guildCreate.GuildName = guildName;
            message.Request.guildCreate.GuildNotice = notice;
            NetClient.Instance.SendMessage(message);
        }

        private void OnGuildCreate(object sender, GuildCreateResponse response)
        {
            Debug.LogFormat("OnGuildCreateResponse:{0}", response.Result);
            if (OnGuildCreateResult != null)
            {
                OnGuildCreateResult(response.Result == Result.Success);
            }
            if (response.Result == Result.Success)
            {
                GuildManager.Instance.Init(response.guildInfo);
                MessageBox.Show(string.Format("{0} 公会创建成功", response.guildInfo.GuildName), "公会");
            }
            else
            {
                MessageBox.Show(string.Format("{0} 公会创建失败", response.guildInfo.GuildName), "公会");
            }
        }

        //发送加入公会请求
        public void SendGuildJoinRequest(int guildId)
        {
            Debug.LogFormat("OnGuildJoinRequest");
            NetMessage message = new NetMessage();
            message.Request = new NetMessageRequest();
            message.Request.guildJoinReq = new GuildJoinRequest();
            message.Request.guildJoinReq.Apply = new NGuildApplyInfo();
            message.Request.guildJoinReq.Apply.GuildId = guildId;
            NetClient.Instance.SendMessage(message);
        }

        //发送加入公会请求
        public void SendGuildJoinResponse(bool accept, GuildJoinRequest request)
        {
            Debug.LogFormat("OnGuildJoinResponse");
            NetMessage message = new NetMessage();
            message.Request = new NetMessageRequest();
            message.Request.guildJoinRes = new GuildJoinResponse();
            message.Request.guildJoinRes.Result = Result.Success;
            message.Request.guildJoinRes.Apply = request.Apply;
            message.Request.guildJoinRes.Apply.Result = accept ? ApplyResult.Accept : ApplyResult.Reject;
            NetClient.Instance.SendMessage(message);
        }

        //收到加入公会请求
        private void OnGuildJoinRequest(object sender, GuildJoinRequest request)
        {
            var confirm = MessageBox.Show(string.Format("{0} 申请加入公会", request.Apply.Name), "公会申请", MessageBoxType.Confirm);
            confirm.OnYes = () =>
            {
                SendGuildJoinResponse(true, request);
            };
            confirm.OnNo = () =>
            {
                SendGuildJoinResponse(false, request);
            };
        }

        //收到加入公会的响应
        private void OnGuildJoinResponse(object sender, GuildJoinResponse response)
        {
            Debug.LogFormat("OnGuildJoinResponse :{0}",response.Result);
            if (response.Result == Result.Success)
            {
                MessageBox.Show("加入公会成功", "公会");
            }
            else
            {
                MessageBox.Show("加入公会失败", "公会");
            }
        }

        private void OnGuild(object sender, GuildResponse message)
        {
            Debug.LogFormat("OnGuild:{0} {1} {2}", message.Result, message.guildInfo.Id,message.guildInfo.GuildName);
            GuildManager.Instance.Init(message.guildInfo);
            if (OnGuildUpdate != null)
                OnGuildUpdate();
        }

        public void SendGuildLeaveRequest()
        {
            Debug.LogFormat("SendGuildLeaveRequest");
            NetMessage message = new NetMessage();
            message.Request = new NetMessageRequest();
            message.Request.Guildleave = new GuildLeaveRequest();
            NetClient.Instance.SendMessage(message);
        }

        private void OnGuildLeave(object sender, GuildLeaveResponse message)
        {
            if (message.Result == Result.Success)
            {
                GuildManager.Instance.Init(null);
                MessageBox.Show("离开公会成功", "公会");
            }
            else
            {
                MessageBox.Show("离开公会失败", "公会",MessageBoxType.Error);
            }
        }

        public void SendGuildListRequest()
        {
            Debug.LogFormat("SendGuildListRequest");
            NetMessage message = new NetMessage();
            message.Request = new NetMessageRequest();
            message.Request.guildList = new GuildListRequest();
            NetClient.Instance.SendMessage(message);
        }

        private void OnGuildList(object sender, GuildListResponse response)
        {
            if (OnGuildListResult != null)
                OnGuildListResult(response.Guilds);
        }
    }
}

