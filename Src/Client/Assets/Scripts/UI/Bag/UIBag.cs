using Managers;
using Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class UIBag : UIWindows {

	public Text Moneys;
	public Transform[] Pages;
	private UIBagItem selectedItem;

	public GameObject BagItem;

	List<Image> Slots;
	// Use this for initialization
	void Start () {
		if (Slots == null)
		{
			Slots = new List<Image>();
			for (int page = 0; page < this.Pages.Length; page++)
			{
				//获取所有背包里的道具组件包括不活跃的
				Slots.AddRange(this.Pages[page].GetComponentsInChildren<Image>(true));
			}
		}
		Moneys.text = User.Instance.CurrentCharacter.Gold.ToString();
		StartCoroutine(InitBags());
	}

	IEnumerator InitBags()
	{
		for (int i = 0; i < BagManager.Instance.items.Length; i++)
		{
			var item = BagManager.Instance.items[i];
			if (item.itemId >0)
			{
				GameObject go = Instantiate(BagItem, Slots[i].transform);
				var ui = go.GetComponent<UIBagItem>();
				ui.SetBagItem(item, this);
			}

		}
		//默认解锁20个格子 把未解锁的颜色编程灰色
		for (int i = BagManager.Instance.items.Length; i < Slots.Count; i++)
		{
			Slots[i].color = Color.gray;
		}
		yield return null;
	}


    void Clear()
	{
		for (int i = 0; i < Slots.Count; i++)
		{
			if (Slots[i].transform.childCount > 0)
			{
				Destroy(Slots[i].transform.GetChild(0).gameObject);
			}
		}
	}

	//每次整理背包都先销毁在重新生成
	public void OnReset()
	{
		BagManager.Instance.Reset();
		Clear();
		StartCoroutine(InitBags());
	}
}
