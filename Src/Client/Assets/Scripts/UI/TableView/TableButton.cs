using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TableButton : MonoBehaviour {
	private Sprite NormalImage;
	public Sprite ActiveImage;

	public TableView tabView;
	public int tabIndex = 0;

	private Image tabImage;
	// Use this for initialization
	void Start () {
		tabImage = this.GetComponent<Image>();
		NormalImage = tabImage.sprite;

		this.GetComponent<Button>().onClick.AddListener(OnClick);
	}
	
	// Update is called once per frame
	void Update () {
		
	}
	public void Select(bool select)
	{
		tabImage.overrideSprite = select ? ActiveImage : NormalImage;
	}
	void OnClick()
	{
		this.tabView.selectTab(this.tabIndex);
	}
}
