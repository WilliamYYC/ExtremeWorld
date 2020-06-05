using JetBrains.Annotations;
using SkillBridge.Message;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Common.Data;
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
}
