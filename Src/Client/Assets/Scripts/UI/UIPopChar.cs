using Managers;
using Services;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UIPopChar : UIWindows,IDeselectHandler {

	public int targetId;
	public string targetName;

	public void OnDeselect(BaseEventData eventData)
    {
        PointerEventData pointerEvent = eventData as PointerEventData;
        if (pointerEvent.hovered.Contains(this.gameObject))
        {
            return;
        }

        this.Close(WinowResult.None);
    }


	public void OnEable()
    {
        this.GetComponent<Selectable>().Select();
        this.Root.transform.position = Input.mousePosition + new Vector3(80, 0, 0);
    }
		
	public void OnChat()
    {
        ChatManager.Instance.startPrivateChat(targetId, targetName);
        this.Close(WinowResult.No);
    }

    public void OnAddFriend()
    {
        FriendService.Instance.SendFriendAddRequest(targetId, targetName);
        this.Close(WinowResult.No);
    }

    public void OnInvite()
    {
        TeamService.Instance.SendTeamInviteRequest(targetId, targetName);
        this.Close(WinowResult.No);
    }
}
