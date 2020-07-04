using Common;
using GameServer.Entities;
using GameServer.Services;
using SkillBridge.Message;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameServer.Managers
{
    class FriendManager
    {
        Character Owner;
        List<NFriendInfo> nFriendInfos = new List<NFriendInfo>();

        bool friendChanged = false;

        public FriendManager(Character owner)
        {
            this.Owner = owner;
            this.InitFriends();
        }

        public void GetFriendInfos(List<NFriendInfo> friendInfos)
        {
            foreach (var item in this.nFriendInfos)
            {
                friendInfos.Add(item);
            }
        }
        public void InitFriends()
        {
            this.nFriendInfos.Clear();
            foreach (var friend in this.Owner.Data.Friends)
            {
                this.nFriendInfos.Add(GetFriendInfo(friend));
            }
        }

        public void AddFriend(Character friend)
        {
            TCharacterFriend fri = new TCharacterFriend
            {
                FriendID = friend.Id,
                FriendName = friend.Data.Name,
                Class = friend.Data.Class,
                Level = friend.Data.Level
            };

            this.Owner.Data.Friends.Add(fri);
            this.friendChanged = true;
        }

        public bool RemoveFriendByFriendId(int friendId)
        {
            var removeItem = this.Owner.Data.Friends.FirstOrDefault(v => v.FriendID == friendId);
            if (removeItem !=null)
            {
                DBService.Instance.Entities.CharacterFriends.Remove(removeItem);
  
            }
            this.friendChanged = true;
            return true;
        }

        public bool RemoveFriendById(int Id)
        {
            var removeItem = this.Owner.Data.Friends.FirstOrDefault(v => v.Id == Id);
            if (removeItem != null)
            {
                DBService.Instance.Entities.CharacterFriends.Remove(removeItem);

            }
            this.friendChanged = true;
            return true;
        }
        public NFriendInfo GetFriendInfo(TCharacterFriend friend)
        {
            NFriendInfo friendInfo = new NFriendInfo();
            var character = CharacterManager.Instance.GetCharacter(friend.FriendID);
            friendInfo.friendInfo = new NCharacterInfo();
            friendInfo.Id = friend.Id;

            if (character == null)
            {
                friendInfo.friendInfo.Id = friend.FriendID;
                friendInfo.friendInfo.Name = friend.FriendName;
                friendInfo.friendInfo.Class = (CharacterClass)friend.Class;
                friendInfo.friendInfo.Level = friend.Level;
                friendInfo.Status = 0;
            }
            else
            {
                friendInfo.friendInfo = character.GetBasicInfo();
                friendInfo.friendInfo.Name = character.Info.Name;
                friendInfo.friendInfo.Class = character.Info.Class;
                friendInfo.friendInfo.Level = character.Info.Level;
                if (friend.Level != character.Info.Level)
                {
                    friend.Level = character.Info.Level;
                }
                character.FriendManager.UpdateFriendInfo(this.Owner.Info, 1);
                friendInfo.Status = 1;
            }
            Log.InfoFormat("{0}:{1} GetFriendInfo :{2}:{3} status:{4}", this.Owner.Id, this.Owner.Info.Name, friendInfo.friendInfo.Id, friendInfo.friendInfo.Name, friendInfo.Status);
            return friendInfo;
        }

        

        public NFriendInfo GetFriendInfo(int friendId)
        {
            foreach (var fri in this.nFriendInfos)
            {
                if (fri.friendInfo.Id == friendId)
                {
                    return fri;
                }
            }
            return null;
        }

        public void UpdateFriendInfo(NCharacterInfo friendInfo, int status)
        {
            foreach (var friend in this.nFriendInfos)
            {
                if (friend.friendInfo.Id == friendInfo.Id)
                {
                    friend.Status = status;
                    break;
                }
            }
            this.friendChanged = true;
        }
        //显示通知其他好友自己下线了
        public void OfflineNotify()
        {
            foreach (var friendinfo in nFriendInfos)
            {
                var friend = CharacterManager.Instance.GetCharacter(friendinfo.friendInfo.Id);
                if (friend !=null)
                {
                    friend.FriendManager.UpdateFriendInfo(this.Owner.Info, 0);
                }
            }
        }
        internal void PostProcess(NetMessageResponse message)
        {
            if (this.friendChanged)
            {
                Log.InfoFormat("PostProcess > FriendManager : CharacterID:{0}:{1}  ",this.Owner.Id,this.Owner.Info.Name);
                this.InitFriends();
                if (message.friendList == null)
                {
                    message.friendList = new FriendListResponse();
                    message.friendList.Friends.AddRange(this.nFriendInfos);
                }
                this.friendChanged = false;
            }
        }
    }
}
