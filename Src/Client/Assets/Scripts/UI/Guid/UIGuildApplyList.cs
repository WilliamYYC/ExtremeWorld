using Managers;
using Services;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIGuildApplyList : UIWindows {

	public GameObject itemPrefab;
	public ListView listMain;
	public Transform ItemRoot;
	// Use this for initialization
	void Start () {
		GuildServcie.Instance.OnGuildUpdate += UpdateUI;
		GuildServcie.Instance.sendGuildListRequest();
		this.UpdateUI();
	}

	private void OnDestroy()
	{
		GuildServcie.Instance.OnGuildUpdate -= UpdateUI;
	}
	void UpdateUI()
    {
		ClearList();
		InitItems();
    }

     void InitItems()
    {
        foreach (var item in GuildManager.Instance.guildInfo.Applies)
        {
			GameObject go = Instantiate(itemPrefab, this.listMain.transform);
			UIApplyListItem ui = go.GetComponent<UIApplyListItem>();
			ui.SetApplyItemInfo(item);
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
}
