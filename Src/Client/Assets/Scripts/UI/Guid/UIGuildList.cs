using Services;
using SkillBridge.Message;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIGuildList :  UIWindows{

	public GameObject itemPrefab;
	public ListView listMain;
	public Transform ItemRoot;
	public UIGuildInfo uiInfo;
	public UIGuildItem uiSelectedItem;
	// Use this for initialization
	void Start () {

		this.listMain.onItemSelected += this.OnGuildMemberSelected;
		this.uiInfo.Info = null;
		GuildServcie.Instance.OnGuildListResult += UpdateGuildList;
		GuildServcie.Instance.sendGuildListRequest();
	}

	private void OnDestroy()
	{
		GuildServcie.Instance.OnGuildListResult -= UpdateGuildList;
	}
    void UpdateGuildList(List<NGuildInfo> guids)
    {
		ClearList();
		InitItems(guids);
    }

    private void InitItems(List<NGuildInfo> guids)
    {
        foreach (var item in guids)
        {
			GameObject go = Instantiate(itemPrefab, this.listMain.transform);
			UIGuildItem ui = go.GetComponent<UIGuildItem>();
			ui.SetGuildInfo(item);
			this.listMain.AddItem(ui);
        }
    }

    private void ClearList()
    {
		this.listMain.RemoveAll();
    }

    private void OnGuildMemberSelected(ListView.ListViewItem item)
    {
		this.uiSelectedItem = item as UIGuildItem;
		this.uiInfo.Info = this.uiSelectedItem.info;

	}

    // Update is called once per frame
    void Update () {
		
	}


	public void OnClickJoin()
    {
        if (uiSelectedItem == null)
        {
			MessageBox.Show("请选择要加入的公会");
			return;
        }
		MessageBox.Show(string.Format("确定要加入公会{0}吗？", this.uiSelectedItem.info.GuildName), "申请加入公会", MessageBoxType.Confirm, "申请加入", "取消").OnYes=()=> {
			GuildServcie.Instance.sendJoinGuildRequest(this.uiSelectedItem.info.Id);
		};
    }
}
