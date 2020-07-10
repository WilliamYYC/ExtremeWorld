using Models;
using SkillBridge.Message;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Text;

namespace Managers
{
    class GuildManager : Singleton<GuildManager>
    {
        public NGuildInfo guildInfo;

        public NGuildMemeberInfo memeberInfo;
        public bool HasGuild
        {
            get 
            {
                return this.guildInfo != null;
            }
        }

        public void Init(NGuildInfo guild)
        {
            this.guildInfo = guild;
            if (guild == null)
            {
                this.memeberInfo = null;
                return;
            }
            foreach (var item in guild.Members)
            {
                if (item.characterId == User.Instance.CurrentCharacter.Id )
                {
                    this.memeberInfo = item;
                    break;
                }
            }
        }

        public void ShowGuild()
        {
           if (this.HasGuild)
            {
                UIManagers.Instance.Show<UIGuild>();
            }
            else
            {
               var win =  UIManagers.Instance.Show<UIGuidPopNoGuid>();
                win.OnClose += PopNoGuildOnClose;
            }
        }

        void PopNoGuildOnClose(UIWindows sender, UIWindows.WinowResult result)
        {
            if (result == UIWindows.WinowResult.Yes)
            {
                UIManagers.Instance.Show<UIPopCreate>();
            }
            else if (result == UIWindows.WinowResult.No)
            {
                UIManagers.Instance.Show<UIGuildList>();
            }
        }
    }
}
