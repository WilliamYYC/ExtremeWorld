﻿using Entities;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIWorldElementManager : MonoSingleton<UIWorldElementManager>
{ 


	public GameObject NameBarPrefab;

	public Dictionary<Transform, GameObject> elements = new Dictionary<Transform, GameObject>();

	 //Use this for initialization
	protected override void OnStart()
	{
	    NameBarPrefab.SetActive(false);
	}

	// Update is called once per frame
	void Update () {
		
	}

	public void AddCharacterNameBar(Transform owner , Character cha)
	{
		GameObject goNameBar = Instantiate(NameBarPrefab, this.transform);
		goNameBar.name = "NameBar" + cha.entityId;
		goNameBar.GetComponent<UIWorldElement>().owner = owner;
		goNameBar.GetComponent<UINameBar>().character = cha;
		goNameBar.SetActive(true);
		this.elements[owner] = goNameBar;
	}

	public void RemoveCharacterNameBar(Transform owner)
	{
		if (this.elements.ContainsKey(owner))
		{
			Destroy(this.elements[owner]);
			this.elements.Remove(owner);
		}
	}
}