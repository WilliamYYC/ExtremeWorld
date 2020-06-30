using Models;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIQuestItem : ListView.ListViewItem {
	
	
	public Text Title;
	public Image Background;
	public Sprite NormalBg;
	public Sprite SelectedBg;

	public override void onSelected(bool selected)
	{
		this.Background.overrideSprite = selected ? SelectedBg : NormalBg;
	}

	public Quest quest;
	// Use this for initialization
	void Start () {
		
	}
	bool isEquiped = false;


	public void SetQuestInfo(Quest item)
	{
		this.quest = item;
		if (this.Title !=null)
		{
			this.Title.text = this.quest.questDefine.Name;
		}
	}
	// Update is called once per frame
	void Update () {
		
	}
}
