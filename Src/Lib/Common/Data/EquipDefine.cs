using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SkillBridge.Message;

namespace Common.Data
{
    public class EquipDefine
    {
        public int ID { set; get; }

        public EquipSlot Slot { set; get; }

        public string Category { set; get; }
        public float STR { set; get; }
        public float INT { set; get; }
        public float DEX { set; get; }
        public float HP { set; get; }
        public float MP { set; get; }

        public float AD { set; get; }
        public float AP { set; get; }
        public float DEF { set; get; }
        public float MDEF { set; get; }
        public float SPD { set; get; }
        public float CRI { set; get; }
    }
}
