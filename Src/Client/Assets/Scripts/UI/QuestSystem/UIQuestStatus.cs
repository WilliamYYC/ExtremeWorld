using Managers;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIQuestStatus : MonoBehaviour {


	public Image[] statusImages;
	private NpcQuestStatus questStatus;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void SetQuestStatus(NpcQuestStatus status)
    {
		this.questStatus = status;
		for (int i = 0; i < 4; i++)
		{
			if (this.statusImages[i] !=null)
			{
				this.statusImages[i].gameObject.SetActive(i ==(int)status);
			}
		}
    }
}
