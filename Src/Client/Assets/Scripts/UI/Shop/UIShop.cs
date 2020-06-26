using Common.Data;
using Managers;
using Models;
using SkillBridge.Message;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIShop : UIWindows {

	public Text Title;
	public Text Money;

	public GameObject shopItem;
	ShopDefine shop;

	public Transform[] ItemRoot;
	// Use this for initialization
	void Start () {
		StartCoroutine(InitItems());
	}

	//从DataManager中加载到scrollview的content 中
	IEnumerator InitItems()
	{
		foreach (var kv in DataManager.Instance.ShopItems[shop.ID])
		{
			if (kv.Value.Status > 0)
			{
				GameObject go = Instantiate(shopItem, ItemRoot[0]);
				UIShopItem ui = go.GetComponent<UIShopItem>();
				ui.SetShopItem(kv.Key,kv.Value,this);
			}
		}
		yield return null;
	}

	//初始化商店信息
	public void setShop(ShopDefine shop)
	{
		this.shop = shop;
		this.Title.text = shop.Name;
		this.Money.text = User.Instance.CurrentCharacter.Gold.ToString();
	}


	private UIShopItem selectedItem;
	public void SelectedShopItem(UIShopItem shopItem)
	{
		if (selectedItem != null)
		{
			selectedItem.Selected = false;
		}
		selectedItem = shopItem;
	}

	public void OnClickBuy()
	{
		if (this.selectedItem == null)
		{
			MessageBox.Show("请选择要购买的道具","购买提示");
			return;
		}
		if (!ShopManager.Instance.BuyItem(this.shop.ID, this.selectedItem.ShopItemID))
		{

		}
	}
	// Update is called once per frame
	void Update () {
		
	}
}
