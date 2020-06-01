using Models;
using Services;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIMainCity : MonoBehaviour {


	public Text AvatarName;
	public Text Avatarlevel;
	// Use this for initialization
	void Start () {
		this.UpdateAvatar();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void UpdateAvatar()
	{
		AvatarName.text = string.Format("{0}[{1}]", User.Instance.CurrentCharacter.Name, User.Instance.CurrentCharacter.Id);
		Avatarlevel.text = User.Instance.CurrentCharacter.Level.ToString();
	}

	public void BackToCharSelect()
	{
		SceneManager.Instance.LoadScene("CharSelect");
		UserService.Instance.sendGameLeave();
	}
}
