using Entities;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UINameBar : MonoBehaviour {


	public Text avatarName;

	public Character character;
	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
		this.updateInfo();

	}

	public void updateInfo()
	{
		if (character !=null)
		{
			string name = this.character.Name + "LV " + this.character.Info.Level;
			if (name != this.avatarName.text)
			{
				this.avatarName.text = name;
			}
		}
	}


}
