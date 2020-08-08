using Managers;
using Models;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIQuestInfo : MonoBehaviour {


	public Text Title;
	public Text[] Targets;
	public Text Description;
	public Image RewardItem1;
	public Image RewardItem2;
	public Image RewardItem3;
	public GameObject UiRewardItem;

	public Text OverView;
	//public UIIconItem RewardItem;

	public Text RewardMoney;
	public Text RewardExp;

	public Button NavButton;
	private int npc = 0;

	// Use this for initialization
	void Start () {
	}

	public void SetQuestInfo(Quest quest)
	{
		this.Title.text = string.Format("[{0}]{1}", quest.questDefine.Type, quest.questDefine.Name);

        if (this.OverView.text !=null)
        {
			this.OverView.text = quest.questDefine.Overview;

		}
        if (this.Description != null)
        {
			if (quest.Info == null)
			{
				this.Description.text = quest.questDefine.Dialog;
			}
			else
			{
				if (quest.Info.Status == SkillBridge.Message.QuestStatus.Completed)
				{
					this.Description.text = quest.questDefine.DialogFinish;
				}
			}
		}

		if (quest.questDefine.RewardItem1 > 0)
		{
			setRewardItem(RewardItem1, quest.questDefine.RewardItem1, quest.questDefine.RewardItem1Count);
		}
		else
		{
			RewardItem1.gameObject.SetActive(false);
		}

		if (quest.questDefine.RewardItem2 >0 )
		{
			setRewardItem(RewardItem2, quest.questDefine.RewardItem2, quest.questDefine.RewardItem2Count);
		}
		else
		{
			RewardItem2.gameObject.SetActive(false);
		}

		if (quest.questDefine.RewardItem3 > 0)
		{
			setRewardItem(RewardItem3, quest.questDefine.RewardItem3, quest.questDefine.RewardItem3Count);
		}
		else
		{
			RewardItem3.gameObject.SetActive(false);
		}

		this.RewardMoney.text = quest.questDefine.RewardGold.ToString();
		this.RewardExp.text = quest.questDefine.RewardExp.ToString();

		//任务导航开启导航按钮
        if (quest.Info == null)
        {
			this.npc = quest.questDefine.AcceptNPC;
        }
        else if (quest.Info.Status == SkillBridge.Message.QuestStatus.Completed)
		{
			this.npc = quest.questDefine.SubmitNPC;
		}
		//this.NavButton.gameObject.SetActive(this.npc > 0);

		foreach (var fitter in this.GetComponentsInChildren<ContentSizeFitter>())
		{
			fitter.SetLayoutVertical();
		}
	}

    private void setRewardItem(Image rewardItem, int rewardItemId, int RewardItemCount)
    {
		rewardItem.gameObject.SetActive(true);
		GameObject go = Instantiate(UiRewardItem, rewardItem.transform);
		var ui = go.GetComponent<UIIconItem>();
		var def = DataManager.Instance.Items[rewardItemId];
		ui.SetMainIcon(def.Icon, RewardItemCount.ToString());
	}



    // Update is called once per frame
    void Update () {
		
	}


	public void OnClickAbandon()
	{ }

	public void OnClickNav()
	{
		Vector3 pos = NpcManager.Instance.GetNpcPosition(this.npc);
		User.Instance.CurrentCharacterObject.startNav(pos);
		UIManagers.Instance.Close<UIQuestSystem>();
	}
}
