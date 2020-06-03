using Managers;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class MapController : MonoBehaviour {

	public Collider collider;

	// Use this for initialization
	void Start () {
		MiniMapManager.Instance.UpdateMiniMap(collider);
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
