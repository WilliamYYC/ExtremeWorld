using Managers;
using Models;
using Services;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIFriend : UIWindows {

	public GameObject ItemPrefab;
	public ListView listMain;
	public Transform ItemRoot;
	public UIFriendItem friendItem;
	// Use this for initialization
	void Start () {
		FriendService.Instance.OnFriendUpdate = RefreshUI;
		listMain.onItemSelected += OnFriendSelected;
		RefreshUI();
	}

	public void OnDestroy()
	{
		FriendService.Instance.OnFriendUpdate -= RefreshUI;
	}

	private void RefreshUI()
	{
		ClearFriendList();
		InitFriendItems();
	}

	

	void OnFriendSelected(ListView.ListViewItem item)
	{
		this.friendItem = item as UIFriendItem;
	}

	public void OnClickFriendAdd()
	{
		InputBox.show("输入你要添加的好友名称或ID","添加好友").OnSubmit += OnFriendAddSubmit;
	}

	public void OnClickFriendInvite()
	{
		if (friendItem == null )
		{
			MessageBox.Show("请选择邀请的好友");
			return;
		}
		if (friendItem.friend.Status == 0)
		{
			MessageBox.Show("请选择在线的好友");
			return;
		}

		MessageBox.Show(string.Format("确定邀请{0}玩家加入队伍",friendItem.friend.friendInfo.Name), "邀请组队", MessageBoxType.Confirm, "确定", "取消").OnYes = () => {
			TeamService.Instance.SendTeamInviteRequest(this.friendItem.friend.friendInfo.Id, this.friendItem.friend.friendInfo.Name);
		};
	}

	private bool OnFriendAddSubmit(string inputText, out string tips)
	{
		tips = "";
		int friendID = 0;
		string friendName = "";
		if (!int.TryParse(inputText, out friendID))
		{
			friendName = inputText;
		}
		if (friendID == User.Instance.CurrentCharacter.Id || friendName == User.Instance.CurrentCharacter.Name)
		{
			tips = "你不能添加自己为好友";
			return false;
		}
		FriendService.Instance.SendFriendAddRequest(friendID, friendName);
		return true;
	}

	public void OnFriendChat()
	{
		MessageBox.Show("暂未开放");
	}

	public void OnFriendRemove()
	{

		if (friendItem ==null)
		{
			MessageBox.Show("请选择要删除的好友");
			return;
		}
		MessageBox.Show(string.Format("你确定要删除好友{0}吗？", this.friendItem.friend.friendInfo.Name), "删除好友", MessageBoxType.Confirm, "删除", "取消").OnYes = () => {
			FriendService.Instance.SendFriendRemoveRequest(this.friendItem.friend.Id, this.friendItem.friend.friendInfo.Id);
		};
	}
	// Update is called once per frame
	void Update () {
		
	}


	private void InitFriendItems()
	{
		foreach (var item in FriendManager.Instance.allFriends)
		{
			GameObject go = Instantiate(ItemPrefab, this.listMain.transform);
			UIFriendItem ui = go.GetComponent<UIFriendItem>();
			ui.SetFriendInfo(item);
			this.listMain.AddItem(ui);
		}
	}

	private void ClearFriendList()
	{
		this.listMain.RemoveAll();
	}
}
