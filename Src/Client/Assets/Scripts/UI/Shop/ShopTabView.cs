using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using UnityEngine;
using UnityEngine.UI;

public class ShopTabView : MonoBehaviour {

	public Button ShopButtonLeft;
	public Button ShopButtonRight;
	public GameObject[] ShoptablePages;
	public Text ShopPage;

	public int page = 1;
	public int len = 0;
	// Use this for initialization
	void Start()
	{
		len = ShoptablePages.Length;
		for (int i = 0; i < ShoptablePages.Length; i++)
		{
			if (i == 0)
			{
				ShoptablePages[i].SetActive(true);
			}
			else
			{
				ShoptablePages[i].SetActive(false);
			}		
		}
		string str = string.Format("第" + page + "页");
		ShopPage.text = str;
	}

	public void OnClickLeft()
	{
		page--;
		if (page == 0)
		{
			page = 1;
			return;
		}
		if (page > 0)
		{
			ShoptablePages[page].SetActive(false);
			ShoptablePages[page-1].SetActive(true);
			string str = string.Format("第" + page + "页");
			ShopPage.text = str;
		}
	}

	public void OnClickRight()
	{
		page++;
		if (page  == len + 1)
		{
			page = len;
			return;
		}
		if (page > 0 && page < len + 1)
		{
			ShoptablePages[page-2].SetActive(false);
			ShoptablePages[page-1].SetActive(true);
			string str = string.Format("第" + page + "页");
			ShopPage.text = str;
		}
	}
	// Update is called once per frame
	void Update () {
		
	}
}
