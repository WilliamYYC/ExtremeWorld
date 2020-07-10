using Models;
using Services;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UISetting : UIWindows {

	public void ExitToCharSelect() 
	{
		SceneManager.Instance.LoadScene("CharSelect");
		UserService.Instance.sendGameLeave();
	}

	public void ExitGame()
	{
		UserService.Instance.sendGameLeave(true);
	}
}
