using Common.Data;
using Managers;
using Services;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeleportObject : MonoBehaviour {


	public int ID;
	Mesh mesh = null;
	// Use this for initialization
	void Start () {
		this.mesh = this.GetComponent<MeshFilter>().sharedMesh;
	}
	
	// Update is called once per frame
	void Update () {
		
	}

#if UNITY_EDITOR
	void OnDrawGizmos()
	{
		Gizmos.color = Color.green;
		if (this.mesh != null)
		{
			Gizmos.DrawWireMesh(this.mesh , this.transform.position + Vector3.up * this.transform.localScale.y * 0.5f, this.transform.rotation,this.transform.localScale);
		}
		UnityEditor.Handles.color = Color.red;
		UnityEditor.Handles.ArrowHandleCap(0, this.transform.position,this.transform.rotation, 1f, EventType.Repaint );
	}
#endif

	void OnTriggerEnter(Collider collider)
	{
		PlayerInputController playerInput = collider.GetComponent<PlayerInputController>();

		if (playerInput !=null && playerInput.isActiveAndEnabled)
		{
			TeleporterDefine tdDefine = DataManager.Instance.Teleporters[this.ID];

			if (tdDefine == null)
			{
				Debug.LogErrorFormat("TeleportObject: Chacater :{0}  Enter Teleport{1} but TeleporterDefine is not existed", playerInput.character.Info.Name , this.ID);
				return;
			}
			Debug.LogFormat("TeleportObject: Chacater : [{0}]  Enter Teleport [{1}:{2}] ", playerInput.character.Info.Name, this.ID, tdDefine.Name);
			if (tdDefine.LinkTo > 0)
			{
				if (DataManager.Instance.Teleporters.ContainsKey(tdDefine.LinkTo))
				{
					MapService.Instance.sendMapTeleport(this.ID);
				}
				else
				{
					Debug.LogErrorFormat("teleporter id {0}  link id {1}  error", this.ID, tdDefine.LinkTo);
				}
			}
		}

		
	}
}
