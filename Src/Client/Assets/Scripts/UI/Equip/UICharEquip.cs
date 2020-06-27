﻿using Managers;
using SkillBridge.Message;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UICharEquip : UIWindows {
	public Text Title;
	//public Text Money;

	public GameObject ItemPrefab;
	public GameObject ItemEquipPrefab;

	public Transform ItemListRoot;

	public List<Transform> slots;
	// Use this for initialization
	void Start () {
		RefreshUI();
		EquipManager.Instance.OnEquipChanged += RefreshUI;
	}

	private void OnDestory()
	{
		EquipManager.Instance.OnEquipChanged -= RefreshUI;
	}
	 void RefreshUI()
	{
		ClearAllEquipList();
		InitAllEquipItems();
		ClearEquipedList();
		InitEquipedItems();
	}

	//初始化所有装备列表
	 void InitEquipedItems()
	{
		foreach (var kv in ItemManager.Instance.items)
		{
			if (kv.Value.itemDefine.Type == ItemType.Equip)
			{
				//已经装备的先不显示
				if (EquipManager.Instance.Contains(kv.Key))
				{
					continue;
				}
				GameObject go = Instantiate(ItemPrefab, ItemListRoot);
				UIEquipItem ui = go.GetComponent<UIEquipItem>();
				ui.setEquipItem(kv.Key, kv.Value, this, false);
			}
		}
	}

	 void ClearEquipedList()
	{
		foreach (var item in ItemListRoot.GetComponentsInChildren<UIEquipItem>())
		{
			Destroy(item.gameObject);
		}
	}

	//初始化已经装备的装备
	 void InitAllEquipItems()
	{
		for (int i = 0; i < (int)EquipSlot.SlotMax; i++)
		{
			var item = EquipManager.Instance.Equips[i];
			{
				if (item!= null)
				{
					GameObject go = Instantiate(ItemEquipPrefab, slots[i]);
					UIEquipItem ui = go.GetComponent<UIEquipItem>();
					ui.setEquipItem(i, item, this, true);
				}
			}
		}
	}

	 void ClearAllEquipList()
	{
		foreach (var item in slots)
		{
			if (item.childCount >0)
			{
				Destroy(item.GetChild(0).gameObject);
			}
			
		}
	}


	public void doEquip(Item equip)
	{
		EquipManager.Instance.EquipItem(equip);
	}

	public void UnEquip(Item equip)
	{
		EquipManager.Instance.UnEquipItem(equip);
	}
	// Update is called once per frame
	void Update () {
		
	}
}
