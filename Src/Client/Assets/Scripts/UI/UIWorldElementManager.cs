using Entities;
using Managers;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIWorldElementManager : MonoSingleton<UIWorldElementManager>
{ 


	public GameObject NameBarPrefab;

	public GameObject NpcStatusPrefab;

	public Dictionary<Transform, GameObject> elementNames = new Dictionary<Transform, GameObject>();
	public Dictionary<Transform, GameObject> elementStatus = new Dictionary<Transform, GameObject>();

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
		this.elementNames[owner] = goNameBar;
	}

	public void RemoveCharacterNameBar(Transform owner)
	{
		if (this.elementNames.ContainsKey(owner))
		{
			Destroy(this.elementNames[owner]);
			this.elementNames.Remove(owner);
		}
	}

	public void AddNpcQuestStatus(Transform owner, NpcQuestStatus status)
	{
		if (this.elementStatus.ContainsKey(owner))
		{
			elementStatus[owner].GetComponent<UIQuestStatus>().SetQuestStatus(status);
		}
		else
		{
			GameObject go = Instantiate(NpcStatusPrefab, this.transform);
			go.name = "NpcQuestStatus" + owner.name;
			go.GetComponent<UIWorldElement>().owner = owner;
			go.GetComponent<UIQuestStatus>().SetQuestStatus(status);
			go.SetActive(true);
			this.elementStatus[owner] = go;
		}
	}


	public void RemoveNpcQusetStatus(Transform owner)
	{
		if (this.elementStatus.ContainsKey(owner))
		{
			Destroy(this.elementStatus[owner]);
			this.elementStatus.Remove(owner);
		}
	}
}
