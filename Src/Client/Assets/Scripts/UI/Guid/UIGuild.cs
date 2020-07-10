using Managers;
using Models;
using Services;
using SkillBridge.Message;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIGuild : UIWindows {

	public GameObject itemPrefab;
	public ListView listMain;
	public Transform ItemRoot;
	public UIGuildInfo uiInfo;
	public UIGuildMemberItem uiSelectedItem;

	public GameObject panelAdmin;
	public GameObject panelLeader;
	// Use this for initialization
	void Start () {
		GuildServcie.Instance.OnGuildUpdate += UpdateUI;
		this.listMain.onItemSelected += this.OnGuildMemberSelected;
		this.UpdateUI();

		panelAdmin.SetActive(GuildManager.Instance.memeberInfo.Title > GuildTitle.None);
		panelLeader.SetActive(GuildManager.Instance.memeberInfo.Title == GuildTitle.President) ;
	}

    void OnGuildMemberSelected(ListView.ListViewItem item)
    {
		this.uiSelectedItem = item as UIGuildMemberItem;
	}

    void OnDestroy()
    {
		GuildServcie.Instance.OnGuildUpdate -= UpdateUI;
	}
    private void UpdateUI()
    {
		this.uiInfo.Info = GuildManager.Instance.guildInfo;

		ClearList();
		InitItems();
    }


	private void InitItems()
	{
		foreach (var item in GuildManager.Instance.guildInfo.Members)
		{
			GameObject go = Instantiate(itemPrefab, this.listMain.transform);
			UIGuildMemberItem ui = go.GetComponent<UIGuildMemberItem>();
			ui.SetGuildMemberInfo(item);
			this.listMain.AddItem(ui);
		}
	}

	private void ClearList()
	{
		this.listMain.RemoveAll();
	}
	// Update is called once per frame
	void Update () {
		
	}

	public void OnClickAppliesList()
    {
		UIManagers.Instance.Show<UIGuildApplyList>();
    }

	public void OnClickChat()
	{ }

	public void OnClickKickOut()
	{
        if (this.uiSelectedItem == null)
        {
			MessageBox.Show("请选择要踢出公会的人");
			return;
        }
        MessageBox.Show(string.Format("是否要踢出{0}玩家出工会", this.uiSelectedItem.info.Info.Name),"踢出公会", MessageBoxType.Confirm,"确定","取消").OnYes=()=>{
			GuildServcie.Instance.sendGuildAdminCommand(GuildAdminCommand.Kickout, this.uiSelectedItem.info.Info.Id);
		};
	}

	public void OnClickPromote()
	{
		if (this.uiSelectedItem == null)
		{
			MessageBox.Show("请选择要晋升的人");
			return;
		}
		if (this.uiSelectedItem.info.Title != GuildTitle.None)
		{
			MessageBox.Show("你选择的人没有课晋升的职位");
			return;
		}

		MessageBox.Show(string.Format("是否要晋升{0}玩家", this.uiSelectedItem.info.Info.Name), "升职", MessageBoxType.Confirm, "确定", "取消").OnYes = () => {
			GuildServcie.Instance.sendGuildAdminCommand(GuildAdminCommand.Promote, this.uiSelectedItem.info.Info.Id);
		};
	}

	public void OnClickLeave()
    {
		MessageBox.Show(string.Format("{0},你是否要离开公会",User.Instance.CurrentCharacter.Name), "离开", MessageBoxType.Confirm, "确定", "取消").OnYes = () => {
			GuildServcie.Instance.sendGuildLeaveRequest(User.Instance.CurrentCharacter.Id);
		};
	}

	public void OnClickDespose()
	{
		if (this.uiSelectedItem == null)
		{
			MessageBox.Show("请选择要罢免的人");
			return;
		}
		if (this.uiSelectedItem.info.Title == GuildTitle.None)
		{
			MessageBox.Show("这个人已经不能再降职了");
			return;
		}

		if (this.uiSelectedItem.info.Title == GuildTitle.President)
		{
			MessageBox.Show("会长不能降级");
			return;
		}
		MessageBox.Show(string.Format("是否要降职{0}玩家", this.uiSelectedItem.info.Info.Name), "降级", MessageBoxType.Confirm, "确定", "取消").OnYes = () => {
			GuildServcie.Instance.sendGuildAdminCommand(GuildAdminCommand.Depost, this.uiSelectedItem.info.Info.Id);
		};

	}

	public void OnClickTransfer()
	{
		if (this.uiSelectedItem == null)
		{
			MessageBox.Show("请选择要转让的人");
			return;
		}
		MessageBox.Show(string.Format("是否要转让会长给{0}", this.uiSelectedItem.info.Info.Name), "转让会长", MessageBoxType.Confirm, "确定", "取消").OnYes = () => {
			GuildServcie.Instance.sendGuildAdminCommand(GuildAdminCommand.Transfer, this.uiSelectedItem.info.Info.Id);
		};
	}

	public void OnClickSetNotice()
    {

    }
}
