using Managers;
using Models;
using Services;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIMain : MonoSingleton<UIMain> {


	public Text AvatarName;
	public Text Avatarlevel;
	public UITeam TeamWindow;
	// Use this for initialization
	protected override void OnStart() {
		this.UpdateAvatar();
	}

	// Update is called once per frame
	void Update() {

	}

	void UpdateAvatar()
	{
		AvatarName.text = string.Format("{0}[{1}]", User.Instance.CurrentCharacter.Name, User.Instance.CurrentCharacter.Id);
		Avatarlevel.text = User.Instance.CurrentCharacter.Level.ToString();
	}





	public void OnClickTest()
	{
		UITest test = UIManagers.Instance.Show<UITest>();

		test.setTitle("这是一个测试UI");

		test.OnClose += Test_OnClose;
	}

	private void Test_OnClose(UIWindows sender, UIWindows.WinowResult result)
	{
		MessageBox.Show("点击了对话框的: " + result, "对话框响应结果", MessageBoxType.Information);
	}

	public void OnClickBag()
	{
		UIManagers.Instance.Show<UIBag>();
	}


	public void OnClickEquip()
	{
		UIManagers.Instance.Show<UICharEquip>();
	}

	public void OnClickQuest()
	{
		UIManagers.Instance.Show<UIQuestSystem>();
	}

	public void OnClickFriend()
	{
		UIManagers.Instance.Show<UIFriend>();
	}


	public void ShowTeamUI(bool show)
	{
		TeamWindow.ShowTeam(show);
	}

	public void ShowGuildUI()
	{
		GuildManager.Instance.ShowGuild();
	}

	public void OnClickSetting()
    {
		UIManagers.Instance.Show<UISetting>();
	}

	public void OnClickRide()
	{
		
	}

}
