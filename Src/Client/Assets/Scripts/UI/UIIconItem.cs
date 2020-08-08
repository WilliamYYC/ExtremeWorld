using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UIIconItem : MonoBehaviour
{

     public Image MainImage;
	public Image SecondImage;


	public Text mainText;
	

	



	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
	public void SetMainIcon(string iconName, string text)
	{
		this.MainImage.overrideSprite = Resloader.Load<Sprite>(iconName);
		this.mainText.text = text;
	}

	
}
