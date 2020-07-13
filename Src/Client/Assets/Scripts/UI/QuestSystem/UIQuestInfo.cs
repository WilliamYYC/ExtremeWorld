using Managers;
using Models;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIQuestInfo : MonoBehaviour {


	public Text Title;

	public Text[] Targets;
	public Text Description;

	public Text OverView;
	public UIIconItem RewardItem;

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
		this.NavButton.gameObject.SetActive(this.npc > 0);
		foreach (var fitter in this.GetComponentsInChildren<ContentSizeFitter>())
		{
			fitter.SetLayoutVertical();
		}
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
