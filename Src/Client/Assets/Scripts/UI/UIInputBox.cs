using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class UIInputBox : MonoBehaviour {


	public Text Title;
	public Text message;
	public Text Tips;

	public Button ButtonYes;
	public Button ButtonNo;

	public InputField Input;

	public Text ButtonYesTitle;
	public Text ButtonNoTitle;
    public string EmptyTips;

    public delegate bool submitHandler(string inputText, out string tips);
    public event submitHandler OnSubmit;

    public UnityAction OnCancel;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void Init(string title, string message, string btnOK = "", string btnCancel = "", string emptyTips ="")
    {
        if (!string.IsNullOrEmpty(title)) this.Title.text = title;
        this.message.text = message;
        this.Tips.text = null;
        this.OnSubmit = null;
        this.EmptyTips = emptyTips;

        if (!string.IsNullOrEmpty(btnOK)) this.ButtonYesTitle.text = title;
        if (!string.IsNullOrEmpty(btnCancel)) this.ButtonNoTitle.text = title;

        this.ButtonYes.onClick.AddListener(OnClickYes);
        this.ButtonNo.onClick.AddListener(OnClickNo);

       
    }

    void OnClickYes()
    {
        this.Tips.text = "";
        if (string.IsNullOrEmpty(Input.text))
        {
            this.Tips.text = EmptyTips;
            return;
        }
        if (OnSubmit !=null)
        {
            string tips;
            if (OnSubmit(this.Input.text,out tips))
            {
                this.Tips.text = tips;
                return;
            }
        }
        Destroy(this.gameObject);

    }

    void OnClickNo()
    {

        Destroy(this.gameObject);
        if (this.OnCancel !=null)
        {
            this.OnCancel();
        }
    }
}
