using Managers;
using Models;
using SkillBridge.Message;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIRide : UIWindows
{
	public Text descript;

	public GameObject ItemPrefab;
	public ListView listMain;

	public UIRideItem selectedItem;
	// Use this for initialization
	void Start()
	{
		RefreshUI();
		this.listMain.onItemSelected += this.OnItemSelected;
	}

    private void OnItemSelected(ListView.ListViewItem item)
    {
		this.selectedItem = item as UIRideItem;
		this.descript.text = this.selectedItem.Item.itemDefine.Description;
	}

    private void OnDestory()
	{
		
	}
	void RefreshUI()
	{
		ClearList();
		InitItems();
	}

    private void ClearList()
    {
		this.listMain.RemoveAll();
    }

    //初始化坐骑信息
    void InitItems()
	{
		foreach (var kv in ItemManager.Instance.items)
		{
			if (kv.Value.itemDefine.Type == ItemType.Ride &&
				(kv.Value.itemDefine.LimitClass == CharacterClass.None || kv.Value.itemDefine.LimitClass == User.Instance.CurrentCharacter.Class))
			{
				//已经装备的先不显示
				GameObject go = Instantiate(ItemPrefab, this.listMain.transform);
				UIRideItem ui = go.GetComponent<UIRideItem>();
				ui.setRideItem(kv.Value, this);
				this.listMain.AddItem(ui);
			}
		}
	}



	// Update is called once per frame
	void Update()
	{

	}

	public void doRide()
    {
        if (this.selectedItem == null)
        {
			MessageBox.Show("请选择坐骑","提示");
			return;
        }
		User.Instance.Ride(this.selectedItem.Item.itemId);
    }
}
