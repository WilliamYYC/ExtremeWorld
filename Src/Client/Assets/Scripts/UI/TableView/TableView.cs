using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class TableView : MonoBehaviour {


	public TableButton[] tableButtons;
	public GameObject[] tablePages;

	public int index = -1;

	public UnityAction<int> OnTabSelect;
	// Use this for initialization
	IEnumerator Start () {
		//tableButtons绑定对应的tabView
		for (int i = 0; i < tableButtons.Length; i++)
		{
			tableButtons[i].tabView = this;
			tableButtons[i].tabIndex = i;
		}
		yield return new WaitForEndOfFrame();
		selectTab(0);
	}
	

	public void selectTab(int index)
	{
		//如果当前的页面index和传入的不一样 切换页面
		if (this.index != index)
		{
			for (int i = 0; i < tableButtons.Length; i++)
			{
				tableButtons[i].Select(i == index);
				if (i <= tablePages.Length-1)
				{
					tablePages[i].SetActive(i == index);
				}
			}
			if (OnTabSelect != null)
				OnTabSelect(index);
		}
	}
	// Update is called once per frame
	void Update () {
		
	}
}
