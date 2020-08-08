using Common.Data;
using Models;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UIBagItem : MonoBehaviour
{

	public Image icon;
	public Text mainText;
	public ItemDefine item;

	public int BagItemID { set; get; }


	
	//this.Background.overrideSprite = selected ? SelectedBg : NormalBg;


	private UIBag Bag;

	// Use this for initialization
	void Start()
	{

	}

	// Update is called once per frame
	void Update()
	{

	}

	public void SetBagItem(BagItem Item, UIBag owner)
	{
		this.Bag = owner;
		this.BagItemID = Item.itemId;
		this.item = ItemManager.Instance.items[Item.itemId].itemDefine;
		this.mainText.text = Item.Count.ToString();
		this.icon.overrideSprite = Resloader.Load<Sprite>(this.item.Icon);
	}

	public void OnClick()
    {
		UIPopBag popBag = UIManagers.Instance.Show<UIPopBag>();
		popBag.Icon.overrideSprite = Resloader.Load<Sprite>(this.item.Icon);
		popBag.NameText.text = "名称："+this.item.Name;
		popBag.sellPrice.text = "售出价格：" + this.item.SellPrice;
		popBag.Count.text = "数量：" + this.mainText.text;
		popBag.Decription.text = this.item.Description;
	}
}
