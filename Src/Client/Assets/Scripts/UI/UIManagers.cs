using System;
using System.Collections;
using System.Collections.Generic;
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
		UIResources.Add(typeof(UISetting), new UIElements() { ResourcePath = "UI/UISetting", Cache = true });
		
		UIResources.Add(typeof(UIBag), new UIElements() { ResourcePath = "UI/UIBag", Cache = false });
		UIResources.Add(typeof(UIShop), new UIElements() { ResourcePath = "UI/UIShop", Cache = false });
		UIResources.Add(typeof(UICharEquip), new UIElements() { ResourcePath = "UI/UICharEquip", Cache = false });
		UIResources.Add(typeof(UIQuestDialog), new UIElements() { ResourcePath = "UI/UIQuestDialog", Cache = false });
		UIResources.Add(typeof(UIQuestSystem), new UIElements() { ResourcePath = "UI/UIQuestSystem", Cache = false });
		UIResources.Add(typeof(UIFriend), new UIElements() { ResourcePath = "UI/UIFriends", Cache = false });

		UIResources.Add(typeof(UIGuild), new UIElements() { ResourcePath = "UI/Guild/UIGuild", Cache = false });
		UIResources.Add(typeof(UIGuildList), new UIElements() { ResourcePath = "UI/Guild/UIGuildList", Cache = false });
		UIResources.Add(typeof(UIGuidPopNoGuid), new UIElements() { ResourcePath = "UI/Guild/UIGuidPopNoGuid", Cache = false });
		UIResources.Add(typeof(UIPopCreate), new UIElements() { ResourcePath = "UI/Guild/UIGuidCreate", Cache = false });
		UIResources.Add(typeof(UIGuildApplyList), new UIElements() { ResourcePath = "UI/Guild/UIGuildApplyList", Cache = false });
		UIResources.Add(typeof(UIPopChar), new UIElements() { ResourcePath = "UI/UIPopChar", Cache = false });
		UIResources.Add(typeof(UIRide), new UIElements() { ResourcePath = "UI/UIRide", Cache = false });
		UIResources.Add(typeof(UISystemConfig), new UIElements() { ResourcePath = "UI/UISystemConfig", Cache = false });
	}

	~UIManagers()
	{ }

	//显示UI
	public T Show<T>()
	{
		SoundManager.Instance.PlaySound(SoundDefine.SFX_UI_Win_Open);
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
