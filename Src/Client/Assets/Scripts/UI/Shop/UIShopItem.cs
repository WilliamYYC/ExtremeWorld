using Common.Data;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using Managers;

public class UIShopItem : MonoBehaviour,ISelectHandler {

	public Image icon;
	public Text title;
	public Text price;
	public Text count;
	public Text limitClass;

	public Image Background;
	public Sprite NormalBg;
	public Sprite SelectedBg;

	private bool selected;
	public bool Selected
	{
		get { return selected; }
		set
		{
			selected = value;
			this.Background.overrideSprite = selected ? SelectedBg : NormalBg;
		}
	}

	public int ShopItemID { set; get; }
	private UIShop shop;

	private ItemDefine item;
	private ShopItemDefine ShopItem { get; set; }
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void SetShopItem(int id, ShopItemDefine shopItem, UIShop owner)
	{
		this.shop = owner;
		this.ShopItemID = id;
		this.ShopItem = shopItem;
		this.item = DataManager.Instance.Items[this.ShopItem.ItemID];

		this.title.text = this.item.Name;
		this.price.text = ShopItem.Price.ToString();
		this.count.text ="x"+ ShopItem.Count.ToString();
		this.limitClass.text = this.item.LimitClass.ToString();
		this.icon.overrideSprite = Resloader.Load<Sprite>(item.Icon);
	}

	 public void OnSelect(BaseEventData eventData)
	{
		this.Selected = true;
		this.shop.SelectedShopItem(this);
	}
}
