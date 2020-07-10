using Common.Data;
using Managers;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIQuestSystem : UIWindows {
	public Text Title;

	public GameObject ItemPrefab;

	public TableView Tabs;
	public ListView listMain;
	public ListView listBranch;

	public UIQuestInfo questInfo;

	private bool showAviableList = false;
	// Use this for initialization
	void Start () {
		this.listMain.onItemSelected += this.OnQuestSelected;
		this.listBranch.onItemSelected += this.OnQuestSelected;
		this.Tabs.OnTabSelect += OnSelectTab;
		RefreshUI();
	}

	 void RefreshUI()
	{
		ClearAllQuestList();
		InitAllQuestItems();
	}

	//初始化所有任务物品
	private void InitAllQuestItems()
	{
		foreach (var kv in QuestManager.Instance.allQuests)
		{
			if (showAviableList)
			{
				if (kv.Value.Info !=null)
				{
					continue;
				}
			} 
			else
			{
				if (kv.Value.Info == null)
				{
					continue;
				}
			}
			GameObject go = Instantiate(ItemPrefab, kv.Value.questDefine.Type == QuestType.Main ? this.listMain.transform : this.listBranch.transform);
			UIQuestItem ui = go.GetComponent<UIQuestItem>();
			ui.SetQuestInfo(kv.Value);

			if (kv.Value.questDefine.Type == QuestType.Main)
			{
				this.listMain.AddItem(ui as ListView.ListViewItem);
			}
			else
			{
				this.listBranch.AddItem(ui as ListView.ListViewItem);
			}
		}
	}


	//清除所有任务列表
	 void ClearAllQuestList()
	{
		this.listMain.RemoveAll();
		this.listBranch.RemoveAll();
	}

	private void OnSelectTab(int idx)
	{
		showAviableList = idx == 1;
		RefreshUI();
	}

	public  void OnQuestSelected(ListView.ListViewItem Item)
	{
		UIQuestItem questItem = Item as UIQuestItem;
		this.questInfo.SetQuestInfo(questItem.quest);
	}

	 
	private void OnDestroy()
	{ }
	// Update is called once per frame
	void Update () {
		
	}
}
