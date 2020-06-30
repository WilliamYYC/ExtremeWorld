using SkillBridge.Message;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace Common.Data
{
    public enum QuestType
    {
        [Description("主线")]
        Main,
        [Description("主线")]
        Branch
    }

    public enum QuestTarget
    {
        None,
        Kill,
        Item
    }
    public class QuestDefine
    {
        public int ID { set; get; }
        public string Name { set; get; }

        public int LimitLevel { set; get; }

        public CharacterClass LimitClass { set; get; }

        public int PreQuest { set; get; }

        public QuestType Type { set; get; }

        public int AcceptNPC { set; get; }
        public int SubmitNPC { set; get; }


        public string Overview { set; get; }
        public string Dialog { set; get; }
        public string DialogAccept { set; get; }
        public string DialogDeny { set; get; }
        public string DialogIncomplete { set; get; }
        public string DialogFinish { set; get; }



        public QuestTarget Target1 { set; get; }
        public int Target1ID { set; get; }
        public int Target1Num{ set; get; }

        public QuestTarget Target2 { set; get; }
        public int Target2ID { set; get; }
        public int Target2Num { set; get; }

        public QuestTarget Target3 { set; get; }
        public int Target3ID { set; get; }
        public int Target3Num { set; get; }


        public int RewardGold { set; get; }
        public int RewardExp { set; get; }

        public int RewardItem1 { set; get; }
        public int RewardItem1Count { set; get; }

        public int RewardItem2 { set; get; }
        public int RewardItem2Count { set; get; }

        public int RewardItem3 { set; get; }
        public int RewardItem3Count { set; get; }
    }
}
