using SkillBridge.Message;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Common.Data
{
    public class RideDefine
    {
        public int ID { set; get; }
        public string Name { set; get; }

        public string Descriptions { set; get; }
        public int Level { set; get; }

        public CharacterClass LimitClass { set; get; }
        public string Icon { set; get; }

        public string Resource { set; get; }
    }
}
