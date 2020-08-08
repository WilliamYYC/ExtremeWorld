using SkillBridge.Message;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Managers
{
    class FriendManager:Singleton<FriendManager>
    {
        public List<NFriendInfo> allFriends;
        public List<NFriendInfo> allLiveFriends = new List<NFriendInfo>();
        public List<NFriendInfo> allLeaveFriends = new List<NFriendInfo>();

        public void Init(List<NFriendInfo> friendInfos)
        {
            this.allFriends = friendInfos;
            foreach (var item in friendInfos)
            {
                if (item.Status == 1)
                {
                    this.allLiveFriends.Add(item);
                }
            }

            foreach (var item in friendInfos)
            {
                if (item.Status == 0)
                {
                    this.allLeaveFriends.Add(item);
                }
            }

        }
    }
}
