using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Common.Data;
using Managers;
using UnityEditor;
using Models;

public class NpcController : MonoBehaviour {


	public int NpcID;
	Animator animator;
	NpcDefine Npcs;
	SkinnedMeshRenderer renderer;
	Color originColor;

	private bool isInteractive = false;
	// Use this for initialization
	void Start () {
		renderer = this.gameObject.GetComponentInChildren<SkinnedMeshRenderer>();
		animator = this.gameObject.GetComponent<Animator>();
		originColor = renderer.sharedMaterial.color;
		Npcs = NpcManager.Instance.GetNpcDefine(NpcID);
		this.StartCoroutine(Actions());
	}
	

	IEnumerator Actions()
	{
		while (true)
		{
			if (isInteractive)
			{
				yield return new WaitForSeconds(2f);
			}
			else
			{
				yield return new WaitForSeconds(Random.Range(5f,10f));
			}
			this.Relax();
		}
	}
	// Update is called once per frame
	void Update () {
		
	}

	void Relax()
	{
		animator.SetTrigger("Relax");
	}

	public void Interactive()
	{
		if (!isInteractive)
		{
			isInteractive = true;
			this.StartCoroutine(DoInteractive());
		}
	}

	IEnumerator DoInteractive()
	{
		yield return FaceToPlayer();
		//调用实际处理的函数Interactive
		if (NpcManager.Instance.Interactive(Npcs))
		{
			animator.SetTrigger("Talk");
		}
		yield return new WaitForSeconds(3f);
		isInteractive = false;
	}

	//转向算法
	IEnumerator FaceToPlayer()
	{
		Vector3 faceto = (User.Instance.CurrentCharacterObject.transform.position - this.transform.position).normalized;
		while (Mathf.Abs(Vector3.Angle(this.gameObject.transform.forward , faceto)) > 5)
		{
			this.gameObject.transform.forward = Vector3.Lerp(this.gameObject.transform.forward, faceto, Time.deltaTime*5f);
			yield return null;
		}
	}
	void OnMouseDown()
	{
		Interactive();
	}


	void OnMouseOver()
	{
		Highlight(true);
	}
	void OnMouseEnter()
	{
		Highlight(true);
	}
	void OnMouseExit()
	{
		Highlight(false);
	}

	void Highlight(bool highlight)
	{
		if (highlight)
		{
			if (renderer.sharedMaterial.color != Color.white)
			{
				renderer.sharedMaterial.color = Color.white;
			}
		}
		else
		{
			if (renderer.sharedMaterial.color != originColor)
			{
				renderer.sharedMaterial.color = originColor;
			}
		}
	}
}
