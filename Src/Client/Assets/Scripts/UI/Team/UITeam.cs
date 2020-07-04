using Models;
using Services;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UITeam : MonoBehaviour {

	public ListView List;
	public UITeamItem[] members;
	public Text Title;
	// Use this for initialization
	void Start () {
		if (User.Instance.TeamInfo == null)
		{
			this.gameObject.SetActive(false);
			return;
		}
		foreach (var item in members)
		{
			this.List.AddItem(item);
		}
	}
	
	void OnEnabled()
	{
		UpdateTeamUI();
	}

	public void ShowTeam(bool show)
	{
		this.gameObject.SetActive(show);
		if (show)
		{
			UpdateTeamUI();
		}
	}
	void UpdateTeamUI()
	{
		if (User.Instance.TeamInfo == null) return;

		this.Title.text = string.Format("我的队伍 {0}/5", User.Instance.TeamInfo.Members.Count);

		for (int i = 0; i < 5; i++)
		{
			if (i < User.Instance.TeamInfo.Members.Count)
			{
				this.members[i].SetMemberInfo(i, User.Instance.TeamInfo.Members[i], User.Instance.TeamInfo.Members[i].Id ==User.Instance.TeamInfo.Leader );
				this.members[i].gameObject.SetActive(true);
			}
			else
				this.members[i].gameObject.SetActive(false);
		}
	}

	// Update is called once per frame
	void Update () {
		
	}

	public void OnClickLeaveTeam()
	{
		MessageBox.Show("确定离开这个队伍吗？", "离开队伍", MessageBoxType.Confirm, "确定", "取消").OnYes = () => {
			TeamService.Instance.SendTeamLeaveRequest(User.Instance.TeamInfo.Id);
		};
	}
}
