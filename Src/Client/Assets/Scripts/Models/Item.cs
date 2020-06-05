using Common.Data;
using Managers;
using SkillBridge.Message;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item {

    public int itemId;
    public int count;
    public ItemDefine itemDefine;
    public Item(NItemInfo info)
    {
        this.itemId = info.Id;
        this.count = info.Count;
        this.itemDefine = DataManager.Instance.Items[info.Id];
    }

    public override string ToString()
    {
        return string.Format("ID {0}  Count {1}",this.itemId, this.count);
    }

}
