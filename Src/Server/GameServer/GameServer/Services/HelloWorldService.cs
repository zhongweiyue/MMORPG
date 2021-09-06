using Common;
using Network;
using SkillBridge.Message;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;


public class HelloWorldService:Singleton<HelloWorldService>
{
    public void Init()
    {

    }

    public void Start()
    {
        MessageDistributer<NetConnection<NetSession>>.Instance.Subscribe<FirstTestRequest>(this.OnFirstTestRequest);
    }

    void OnFirstTestRequest(NetConnection<NetSession>sender,FirstTestRequest request)
    {
        Log.InfoFormat("OnFirstTestRequest helloword:{0}", request.Helloword);
    }
}

