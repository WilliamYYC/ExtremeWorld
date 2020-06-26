using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.UIElements;
using UnityEngine;

public class UIManagers : Singleton<UIManagers> {

	class UIElements
	{
		public string ResourcePath;
		public bool Cache;
		public GameObject Instance;
	}
	private Dictionary<Type, UIElements> UIResources = new Dictionary<Type, UIElements>();

	public UIManagers()
	{
		
		UIResources.Add(typeof(UITest), new UIElements() { ResourcePath = "UI/UITest", Cache = true });
		UIResources.Add(typeof(UIBag), new UIElements() { ResourcePath = "UI/UIBag", Cache = false });
		UIResources.Add(typeof(UIShop), new UIElements() { ResourcePath = "UI/UIShop", Cache = false });
	}

	~UIManagers()
	{ }

	//显示UI
	public T Show<T>()
	{
		Type type = typeof(T);

		//查看UIResources是否已经有该UI类型
		if (UIResources.ContainsKey(type))
		{
			UIElements info = UIResources[type];
			//查看GameObject是否已经存在
			if (info.Instance != null)
			{
				info.Instance.SetActive(true);
			}
			else
			{
				UnityEngine.Object prefab = Resources.Load(info.ResourcePath);
				if (prefab == null)
				{
					return default(T);
				}
				info.Instance = (GameObject)GameObject.Instantiate(prefab);
			}
			return info.Instance.GetComponent<T>();
		}

		return default (T);
	}

	//关闭UI
	public void Close(Type type)
	{
		if (UIResources.ContainsKey(type))
		{
			UIElements info = UIResources[type];
			if (info.Cache)
			{
				info.Instance.SetActive(false);
			}
			else
			{
				GameObject.Destroy(info.Instance);
				info.Instance = null;
			}
		}
	}
}
