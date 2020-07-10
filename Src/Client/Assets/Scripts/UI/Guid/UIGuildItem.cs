using SkillBridge.Message;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIGuildItem : ListView.ListViewItem {

	public Text guildID;
	public Text guildName;
	public Text guildMemberCount;
	public Text leaderName;

	public Image Background;
	public Sprite NormalBg;
	public Sprite SelectedBg;


	public NGuildInfo info;

	public override void onSelected(bool selected)
	{
		this.Background.overrideSprite = selected ? SelectedBg : NormalBg;
	}
	// Use this for initialization


	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void SetGuildInfo(NGuildInfo info)
	{
		this.info = info;
		if (this.guildID.text != null) this.guildID.text = this.info.Id.ToString();
		if (this.guildName.text != null) this.guildName.text = this.info.GuildName;
		if (this.guildMemberCount.text != null) this.guildMemberCount.text = this.info.memberCount.ToString();
		if (this.leaderName.text != null) this.leaderName.text = this.info.leaderName;
	}
}
