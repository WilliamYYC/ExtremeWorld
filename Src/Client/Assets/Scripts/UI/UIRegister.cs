using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Services;
using SkillBridge.Message;

public class UIRegister : MonoBehaviour {
    
    public InputField usernameRegister;
    public InputField passwordRegister;
    public InputField passwordConfirm;
    public Button buttonRegister;


    public GameObject uiLogin;
	// Use this for initialization
	void Start () {
        UserService.Instance.OnRegister = this.OnRegister;

    }
	
    public void OnRegister(Result result, string msg)
    {
        if (result == Result.Success)
        {
            MessageBox.Show("注册成功,请登录","提示",MessageBoxType.Information).OnYes = this.OnCloseRegister;
        }
        else
        {
            MessageBox.Show(msg, "错误", MessageBoxType.Error);
        }
        
    }

    public void OnCloseRegister()
    {
        this.gameObject.SetActive(false);
        uiLogin.SetActive(true);
    }
    // Update is called once per frame
    void Update () {
		
	}
    public void OnClickRegister()
    {
        if (string.IsNullOrEmpty(this.usernameRegister.text))
        {
            MessageBox.Show("请输入账号");
            return;
        }
        if (string.IsNullOrEmpty(this.passwordRegister.text))
        {
            MessageBox.Show("请输入密码");
            return;
        }
        if (string.IsNullOrEmpty(this.passwordConfirm.text))
        {
            MessageBox.Show("请输入确认密码");
            return;
        }
        if (this.passwordRegister.text != this.passwordConfirm.text)
        {
            MessageBox.Show("两次输入的密码不一致");
            return;
        }
        SoundManager.Instance.PlaySound(SoundDefine.SFX_UI_Click);
        UserService.Instance.SendRegister(this.usernameRegister.text,this.passwordRegister.text);
    }
}
