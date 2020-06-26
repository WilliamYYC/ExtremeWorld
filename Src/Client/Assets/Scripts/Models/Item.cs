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
    public Item(NItemInfo info) : this(info.Id, info.Count)
    {
         
    }

    public Item(int id, int count)
    {
        this.itemId = id;
        this.count = count;
        this.itemDefine = DataManager.Instance.Items[this.itemId];
    }

    public override string ToString()
    {
        return string.Format("ID {0}  Count {1}",this.itemId, this.count);
    }

}
