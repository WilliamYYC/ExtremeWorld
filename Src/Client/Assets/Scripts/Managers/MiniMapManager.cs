using Models;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Managers
{
    class MiniMapManager : Singleton<MiniMapManager>
    {

        public UIMiniMap mini;
        private Collider miniBoundingBox;

        public Collider MiniBoundingBox
        {
            get {
                return miniBoundingBox;
            }
        }

        public Transform PlayerTranform
        {
            get{
                if (User.Instance.CurrentCharacterObject == null)
                {
                    return null;
                }
                return User.Instance.CurrentCharacterObject.transform;
            }
        }
        public Sprite LoadCurrentMinimap()
        {
            return Resloader.Load<Sprite>("UI/Minimap/" + User.Instance.CurrentMapData.MiniMap);
        }

        public void UpdateMiniMap(Collider miniBoundingBox)
        {
            this.miniBoundingBox = miniBoundingBox;
            if (this.mini != null)
            {
                this.mini.UpdateMap();
            }
        }
    }
}
