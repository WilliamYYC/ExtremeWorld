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
    public EquipDefine EquipInfo;
    public Item(NItemInfo info) : this(info.Id, info.Count)
    {
         
    }

    public Item(int id, int count)
    {
        this.itemId = id;
        this.count = count;
        DataManager.Instance.Items.TryGetValue(this.itemId, out this.itemDefine);
        DataManager.Instance.Equips.TryGetValue(this.itemId, out this.EquipInfo);
    }

    public override string ToString()
    {
        return string.Format("ID {0}  Count {1}",this.itemId, this.count);
    }

}
