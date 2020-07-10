using Common;
using SkillBridge.Message;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIGuildInfo : MonoBehaviour {
	public Text guildName;
	public Text guildId;
	public Text guildLeadName;
	public Text guildNotice;
	public Text guildMemberNumber;

	private NGuildInfo info;
	public NGuildInfo Info {
		get {
			return this.info;
		}
		set {
			this.info = value;
			this.UpdateUI();
		}
	}

    public void UpdateUI()
    {
        if (info == null)
        {
			this.guildName.text = "无";
			this.guildId.text = "ID: 0";
			this.guildLeadName.text = "会长: 无";
			this.guildNotice.text = "";
			this.guildMemberNumber.text = string.Format("公会成员数量:0/{0}",GameDefine.GuildMaxMemberCount); 
		}
        else
        {
			this.guildName.text = this.info.GuildName;
			this.guildId.text = "ID: " + this.info.Id;
			this.guildLeadName.text = "会长: " + this.info.leaderName;
			this.guildNotice.text = this.info.Notice;
			this.guildMemberNumber.text = string.Format("公会成员数量:{0}/{0}", this.info.memberCount, GameDefine.GuildMaxMemberCount);
		}
    }

    // Use this for initialization
    void Start () {
		
	}
	  
	// Update is called once per frame
	void Update () {
		
	}
}
