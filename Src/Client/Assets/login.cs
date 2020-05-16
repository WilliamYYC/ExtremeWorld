using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class login : MonoBehaviour {

	// Use this for initialization
	void Start () {
		Network.NetClient.Instance.Init("127.0.0.1", 8000);
		Network.NetClient.Instance.Connect();

		SkillBridge.Message.NetMessage msg = new SkillBridge.Message.NetMessage();
		msg.Request = new SkillBridge.Message.NetMessageRequest();
		msg.Request.firstRequest = new SkillBridge.Message.FirstDefineRequest();
		msg.Request.firstRequest.Msg = "helloWorld";

		Network.NetClient.Instance.SendMessage(msg);
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
