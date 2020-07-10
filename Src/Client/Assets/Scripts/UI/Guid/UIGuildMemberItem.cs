using Common.Utils;
using SkillBridge.Message;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIGuildMemberItem : ListView.ListViewItem
{

	public Text Name;
	public Text Level;
	public Text Class;
	public Text Title;
	public Text JoinTime;
	public Text Status;


	public Image Background;
	public Sprite NormalBg;
	public Sprite SelectedBg;

	public NGuildMemeberInfo info;


	public override void onSelected(bool selected)
	{
		this.Background.overrideSprite = selected ? SelectedBg : NormalBg;
	}
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void SetGuildMemberInfo(NGuildMemeberInfo info)
	{
		this.info = info;
		if (this.Name.text != null) this.Name.text = this.info.Info.Name;
		if (this.Level.text != null) this.Level.text = this.info.Info.Level.ToString();
		if (this.Class.text != null) this.Class.text = this.info.Info.Class.ToString();
		if (this.Title.text != null)
        {
            switch(this.info.Title)
            {
				case GuildTitle.None:
					this.Title.text = "成员";
					break;
				case GuildTitle.President:
					this.Title.text = "会长";
					break;
				case GuildTitle.Vicepresident:
					this.Title.text = "副会长";
					break;
            }
		}
		if (this.JoinTime.text != null) this.JoinTime.text = TimeUtil.GetTime(this.info.Jointime).ToShortDateString();
		if (this.Status.text != null) this.Status.text = this.info.Status == 1?"在线": TimeUtil.GetTime(this.info.Lasttime).ToShortDateString();
	}
}
