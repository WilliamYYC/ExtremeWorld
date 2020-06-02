using Managers;
using Models;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIMiniMap : MonoBehaviour {


	public Image Arrow;
	public Image MinMap;
	public Text MiniName;
	public Collider minimapBoundingBox;

	private Transform thistransform;
	// Use this for initialization
	void Start () {
		MiniMapManager.Instance.mini = this;
		this.UpdateMap();
	}

	public void UpdateMap()
	{
		this.MiniName.text = Models.User.Instance.CurrentMapData.Name;
		this.MinMap.overrideSprite = MiniMapManager.Instance.LoadCurrentMinimap();
	

		this.MinMap.SetNativeSize();
		this.MinMap.transform.localPosition = Vector3.zero;
		this.minimapBoundingBox = MiniMapManager.Instance.MiniBoundingBox;
		this.thistransform = null;
	}
	
	// Update is called once per frame
	void Update () {

		if (thistransform == null)
		{
			thistransform = MiniMapManager.Instance.PlayerTranform;
			
		}
		if (minimapBoundingBox == null || thistransform == null)
		{
			return;
		}
		float realWidth = minimapBoundingBox.bounds.size.x;
		float realHeight = minimapBoundingBox.bounds.size.z;

		float relativeX = thistransform.position.x - minimapBoundingBox.bounds.min.x;
		float relativeY = thistransform.position.z - minimapBoundingBox.bounds.min.z;

		float pivotX = relativeX / realWidth;
		float pivotY = relativeY / realHeight;

		this.MinMap.rectTransform.pivot = new Vector2(pivotX, pivotY);
		this.MinMap.rectTransform.localPosition = Vector2.zero;


		this.Arrow.transform.eulerAngles = new Vector3(0, 0, -thistransform.eulerAngles.y);
	}
}
