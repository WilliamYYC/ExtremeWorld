using Services;
using SkillBridge.Message;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIApplyListItem : ListView.ListViewItem {

	public Text Name;
	public Text Class;
	public Text Level;

	public NGuildApplyInfo info;
		// Use this for initialization
	void Start () {
		
	}


	public void SetApplyItemInfo(NGuildApplyInfo item)
	{
		this.info = item;
		if (this.Name.text != null) this.Name.text = this.info.Name;
		if (this.Class.text != null) this.Class.text = this.info.Class.ToString();
		if (this.Level.text != null) this.Level.text = this.info.Level.ToString();

	}
	// Update is called once per frame
	void Update () {
		
	}

	public void Accept() 
	{
		MessageBox.Show(string.Format("是否同意{0}进入公会",this.info.Name),"申请列表",MessageBoxType.Confirm, "同意入会","拒绝").OnYes = () => {
			GuildServcie.Instance.sendGuidJoinApply(true, this.info);		
		};
	}
	public void Reject()
	{
		MessageBox.Show(string.Format("是否拒绝{0}进入公会", this.info.Name), "申请列表", MessageBoxType.Confirm, "同意入会", "拒绝").OnYes = () => {
			GuildServcie.Instance.sendGuidJoinApply(false, this.info);
		};
	}
}
