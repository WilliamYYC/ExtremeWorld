using Managers;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UIEquipItem : MonoBehaviour,IPointerClickHandler {

	public Image icon;
	public Text title;
	public Text level;
	public Text limitClass;
	public Text limitCategory;

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

	public int Index { set; get; }

	private UICharEquip Owner;
	private Item item;
	// Use this for initialization
	void Start () {
		
	}

	bool isEquiped = false;
	

	// Update is called once per frame
	void Update () {
		
	}

    public  void setEquipItem(int idx, Item item, UICharEquip owner, bool equiped)
    {
		this.Owner = owner;
		this.Index = idx;
		this.item = item;
		this.isEquiped = equiped;

		if (this.title != null) this.title.text = this.item.itemDefine.Name;
		if (this.level != null) this.level.text = this.item.itemDefine.Level.ToString();
		if (this.limitClass != null) this.limitClass.text = this.item.itemDefine.LimitClass.ToString();
		if (this.limitCategory != null) this.limitCategory.text = this.item.itemDefine.Category;
		if (this.icon != null) this.icon.overrideSprite = Resloader.Load<Sprite>(this.item.itemDefine.Icon);
	}

	public void OnPointerClick(PointerEventData eventData)
	{
		if (this.isEquiped)
		{
			UnEquip();
		}
		else
		{
			if (this.selected)
			{
				DoEquip();
				this.selected = false;
			}
			else
				this.selected = true;
		}
	}

	private void DoEquip()
	{
		var msg = MessageBox.Show(string.Format("要装备{0}吗？", this.item.itemDefine.Name), "确认", MessageBoxType.Confirm);
		msg.OnYes = () =>
		{
			var oldEquip = EquipManager.Instance.GetEquip(item.EquipInfo.Slot);
			if (oldEquip != null)
			{
				var Newmsg = MessageBox.Show(string.Format("要替换装备{0}吗？", oldEquip.itemDefine.Name), "确认", MessageBoxType.Confirm);
				Newmsg.OnYes=()=>
				{
					this.Owner.doEquip(this.item);
				};
			}
			else
				this.Owner.doEquip(this.item);
		};
	}

	private void UnEquip()
	{
		var msg = MessageBox.Show(string.Format("要取下装备{0}吗？", this.item.itemDefine.Name), "确认", MessageBoxType.Confirm);

		msg.OnYes = () => 
		{
			this.Owner.UnEquip(this.item);
		};
	}
}
