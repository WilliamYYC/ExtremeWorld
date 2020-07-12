using Managers;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UIRideItem : ListView.ListViewItem
{

	public Image icon;
	public Text title;
	public Text level;

	public Image Background;
	public Sprite NormalBg;
	public Sprite SelectedBg;

	public override void onSelected(bool selected)
	{
		this.Background.overrideSprite = selected ? SelectedBg : NormalBg;
	}

	// Use this for initialization
	void Start()
	{

	}

	public Item Item;
	// Update is called once per frame
	void Update()
	{

	}

	public void setRideItem(Item item, UIRide ride)
	{

		this.Item = item;
		if (this.title != null) this.title.text = this.Item.itemDefine.Name;
		if (this.level != null) this.level.text = this.Item.itemDefine.Level.ToString();
		if (this.icon != null) this.icon.overrideSprite = Resloader.Load<Sprite>(this.Item.itemDefine.Icon);
	}

}
