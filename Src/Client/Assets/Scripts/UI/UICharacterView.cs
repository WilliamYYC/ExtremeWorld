using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UICharacterView : MonoBehaviour {


	public GameObject[] characterObjects;

	private int currentCharacter;

	public int CurrentCharacter
	{
		get
		{
			return currentCharacter;
		}
		set
		{
			currentCharacter = value;
			this.updateCharacter();
		}
	}
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void updateCharacter()
	{
		for (int i = 0; i < 3; i++)
		{
			characterObjects[i].SetActive(i == currentCharacter);
		}
	}
}
