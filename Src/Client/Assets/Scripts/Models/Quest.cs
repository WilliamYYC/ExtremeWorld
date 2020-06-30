using Common.Data;
using Managers;
using SkillBridge.Message;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Models
{
    public class Quest
    {
        public QuestDefine questDefine;
        public NQuestInfo Info;

        public Quest()
        { }

        public Quest(NQuestInfo info)
        {
            this.Info = info;
            this.questDefine = DataManager.Instance.Quests[info.QuestId];
        }

        public Quest(QuestDefine define)
        {
            this.questDefine = define;
            this.Info = null;
        }


        public string GetTypeName()
        {
            return EnumUtil.GetEnumDescription(this.questDefine.Type);
        }
    }
}
