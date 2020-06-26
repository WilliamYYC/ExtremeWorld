using JetBrains.Annotations;
using SkillBridge.Message;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Common.Data;
using Managers;
using Services;

public class ItemManager : Singleton<ItemManager> {


	public Dictionary<int, Item> items = new Dictionary<int, Item>();
	public void Init(List<NItemInfo> Nitems)
	{
		this.items.Clear();

		foreach (var item in Nitems)
		{
			Item newItem = new Item(item);
			this.items.Add(item.Id, newItem);

			Debug.LogFormat("ItemManager : Init() : {0}", newItem);
		}
		StatusService.Instance.RegisterStatusNotify(StatusType.Item, OnItemNotify);
	}

	public ItemDefine GetItem( int itemid)
	{
		return null;
	}

	public bool UseItem(int itemid)
	{
		return false;
	}

	public bool UseItem(ItemDefine item)
	{
		return false;
	}


	bool OnItemNotify(NStatus status)
	{
		if (status.Action ==  StatusAction.Add)
		{
			this.AddItem(status.Id,status.Value);
		}
		if (status.Action == StatusAction.Delete)
		{
			this.RemoveItem(status.Id, status.Value);
		}
		return true;
	}

	void AddItem(int itemid, int count)
	{
		Item item = null;
		if (this.items.TryGetValue(itemid, out item))
		{
			item.count += count;
		}
		else 
		{
			item = new Item(itemid, count);
			this.items.Add(itemid, item);
		}
		BagManager.Instance.AddItem(itemid, count);
	}

	void RemoveItem(int itemid, int count)
	{
		if (!this.items.ContainsKey(itemid))
		{
			return;
		}
		Item item = this.items[itemid];
		if (item.count < count)
			return;

		item.count -= count;

		BagManager.Instance.RemoveItem(itemid, count);
	}
}

