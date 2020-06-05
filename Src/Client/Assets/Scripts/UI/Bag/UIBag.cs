using Managers;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class UIBag : UIWindows {

	public Text Moneys;
	public Transform[] Pages;


	public GameObject BagItem;

	List<Image> Slots;
	// Use this for initialization
	void Start () {
		if (Slots == null)
		{
			Slots = new List<Image>();
			for (int page = 0; page < this.Pages.Length; page++)
			{
				Slots.AddRange(this.Pages[page].GetComponentsInChildren<Image>(true));
			}
		}

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
				var ui = go.GetComponent<UIIconItem>();
				var def = ItemManager.Instance.items[item.itemId].itemDefine;
				ui.SetMainIcon(def.Icon, item.Count.ToString());
			}

		}
		for (int i = BagManager.Instance.items.Length; i < Slots.Count; i++)
		{
			Slots[i].color = Color.gray;
		}
		yield return null;
	}


	public void setTitle()
	{

	}

	public void OnReset()
	{
		BagManager.Instance.Reset();
	}
}
