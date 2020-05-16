using Common;
using Network;
using SkillBridge.Message;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameServer.Services
{
    class HelloWorldServices:Singleton<HelloWorldServices>
    {
        public void Init()
        {
           
        }
        public void Start()
        {
            MessageDistributer<NetConnection<NetSession>>.Instance.Subscribe<FirstDefineRequest>(this.onFirstDefineRequest);
        }


        void onFirstDefineRequest(NetConnection<NetSession> sender , FirstDefineRequest firstDefineRequest)
        {
            Log.InfoFormat("FirstDefineRequest : {0}", firstDefineRequest.Msg);
        }


        public void Stop()
        {

        }
    }
}
