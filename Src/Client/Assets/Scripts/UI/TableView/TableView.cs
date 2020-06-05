using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TableView : MonoBehaviour {


	public TableButton[] tableButtons;
	public GameObject[] tablePages;

	public int index = -1;
	// Use this for initialization
	IEnumerator Start () {
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
		if (this.index != index)
		{
			for (int i = 0; i < tableButtons.Length; i++)
			{
				tableButtons[i].Select(i == index);
				tablePages[i].SetActive(i == index);
			}
		}
	}
	// Update is called once per frame
	void Update () {
		
	}
}
