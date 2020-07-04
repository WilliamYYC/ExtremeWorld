using SkillBridge.Message;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UITeamItem : ListView.ListViewItem {

	public Image background;
	public Image classIcon;
	public Image leaderIcon;
	public Text NickName;
	public int Idx;
	// Use this for initialization
	public NCharacterInfo info;


	public override void onSelected(bool selected)
	{
		this.background.enabled = selected ? true : false;
	}
	void Start () {
		this.background.enabled = false;
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void SetMemberInfo(int idx, NCharacterInfo item, bool isLead)
	{
		this.Idx = idx;
		this.info = item;
		if (this.NickName != null) this.NickName.text = this.info.Level.ToString().PadRight(4) + this.info.Name;
		if (this.classIcon != null) this.classIcon.overrideSprite = SpriteManager.Instance.classIcons[(int)this.info.Class - 1];
		if (this.leaderIcon != null) this.leaderIcon.gameObject.SetActive(isLead);
	}
}
