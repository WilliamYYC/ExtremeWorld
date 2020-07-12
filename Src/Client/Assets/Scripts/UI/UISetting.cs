using Models;
using Services;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UISetting : UIWindows {

	public void ExitToCharSelect() 
	{
		SceneManager.Instance.LoadScene("CharSelect");
		SoundManager.Instance.PlayMusic(SoundDefine.Music_Select);
		UserService.Instance.sendGameLeave();
	}

	public void SystemConfig()
    {
		UIManagers.Instance.Show<UISystemConfig>();
		this.Close();
    }
	public void ExitGame()
	{
		UserService.Instance.sendGameLeave(true);
	}
}
