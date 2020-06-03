using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UITest : UIWindows {

	public Text title;
	// Use this for initialization
	void Start () {
		
	}
	
	public void setTitle(string titile)
	{
		this.title.text = titile;
	}
	// Update is called once per frame
	void Update () {
		
	}
}
