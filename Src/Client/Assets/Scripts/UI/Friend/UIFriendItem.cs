using SkillBridge.Message;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIFriendItem : ListView.ListViewItem {

	public ListView Owner;
	public Text Name;
	public Text Level;
	public Text Class;
	public Text Status;

	public Image Background;
	public Sprite NormalBg;
	public Sprite SelectedBg;

	public NFriendInfo friend;

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

	public void SetFriendInfo(NFriendInfo info)
	{
		this.friend = info;
		if (this.Name.text != null) this.Name.text = this.friend.friendInfo.Name;
		if (this.Class.text != null) this.Class.text = this.friend.friendInfo.Class.ToString();
		if (this.Level.text != null) this.Level.text = this.friend.friendInfo.Level.ToString();
		if (this.Status.text != null) this.Status.text = this.friend.Status == 1?"在线":"离线";
	}
}
