using Common.Data;
using Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityScript.Steps;

namespace Managers
{
    public class ShopManager : Singleton<ShopManager>
    {
        public void Init()
        {
            NpcManager.Instance.RegisterNpcEvent(NpcFunction.InvokeShop, OnOpenShop);

        }

        private bool OnOpenShop(NpcDefine Npc)
        {
            this.ShowShop(Npc.param);
            return true;
        }

        public void ShowShop(int shopId)
        {
            ShopDefine shop;
            if (DataManager.Instance.Shops.TryGetValue(shopId, out shop))
            {
                UIShop uiShop = UIManagers.Instance.Show<UIShop>();
                if (uiShop != null)
                {
                    uiShop.setShop(shop);
                }
            }
        }


        public bool BuyItem(int shopId, int shopItemId)
        {
            ItemService.Instance.SendItemBuy(shopId, shopItemId);
            return true;
        }
    }
}
