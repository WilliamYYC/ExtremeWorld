using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;
using System.Configuration;

using System.Threading;

using Network;
using GameServer.Services;
using GameServer.Managers;

namespace GameServer
{
    class GameServer
    {
        Thread thread;
        bool running = false;
        NetService network;
        public bool Init()
        {
            int port = Properties.Settings.Default.ServerPort;
            network = new NetService();
            network.Init(port);
           
            DBService.Instance.Init();
            //var name = DBService.Instance.Entities.Characters.Where(s=>s.TID == 1);
            UserService.Instance.Init();
            DataManager.Instance.Load();
            MapService.Instance.Init();
            ItemService.Instance.Init();
            QuestService.Instance.Init();
            TeamServices.Instance.Init();
            GuildService.Instance.Init();
            FriendService.Instance.Init();
            //Console.WriteLine("{0}", name.FirstOrDefault<TCharacter>().Name);
            thread = new Thread(new ThreadStart(this.Update));
            return true;
        }

        public void Start()
        {
            network.Start();
            running = true;
            thread.Start();
        }


        public void Stop()
        {
            running = false;
            thread.Join();
            network.Stop();
        }

        public void Update()
        {
            var mapManager = MapManager.Instance;
            while (running)
            {
                Time.Tick();
                Thread.Sleep(100);
                //Console.WriteLine("{0} {1} {2} {3} {4}", Time.deltaTime, Time.frameCount, Time.ticks, Time.time, Time.realtimeSinceStartup);
                mapManager.update();
            }
        }
    }
}
