using Common.Data;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Managers
{
	public class NpcManager : Singleton<NpcManager>
	{
		//使用委托注册事件,使用字典来管理
		public delegate bool NpcActionHandler(NpcDefine Npc);
		public Dictionary<NpcFunction, NpcActionHandler> eventMap = new Dictionary<NpcFunction, NpcActionHandler>();

		public void RegisterNpcEvent(NpcFunction function, NpcActionHandler action)
		{
			if (!eventMap.ContainsKey(function))
			{
				eventMap[function] = action;
			}
			else
				eventMap[function] += action;
		}

		//获取NPC信息
		public NpcDefine GetNpcDefine(int NpcID)
		{
			NpcDefine npc = null;
			DataManager.Instance.Npcs.TryGetValue(NpcID , out  npc);
			return npc;
		}

		//用户交互函数
		public bool Interactive(int npcId)
		{
			if (DataManager.Instance.Npcs.ContainsKey(npcId))
			{
				var npc = DataManager.Instance.Npcs[npcId];
				return Interactive(npc);
			}
			return false;
		}

		public bool Interactive(NpcDefine npcs)
		{
			if (npcs.Type == NpcType.Task)
			{
				return DoTaskInteractive(npcs);
			}
			else if (npcs.Type == NpcType.Functional)
			{
				return DoFunctionInteractive(npcs);
			}
			return false;
		}

		public bool DoFunctionInteractive(NpcDefine npcs)
		{
			if (npcs.Type != NpcType.Functional)
			{
				return false;
			}

			if (!eventMap.ContainsKey(npcs.Function))
			{
				return false;
			}
			return eventMap[npcs.Function](npcs);
		}

		public bool DoTaskInteractive(NpcDefine npcs)
		{
			MessageBox.Show("点击了Npc  "+npcs.Name, "Npc对话");
			return true;
		}
	}


}

