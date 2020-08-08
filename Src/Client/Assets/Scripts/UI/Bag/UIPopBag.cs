using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class UIPopBag : UIWindows, IDeselectHandler
{
    public Image Icon;
    public Text NameText;
    public Text sellPrice;
    public Text Count;
    public Text Decription;


    public void OnDeselect(BaseEventData eventData)
    {
        PointerEventData pointerEvent = eventData as PointerEventData;
        if (pointerEvent.hovered.Contains(this.gameObject))
        {
            return;
        }

        this.Close(WinowResult.None);
    }


    public void OnEnable()
    {
        this.GetComponent<Selectable>().Select();
        this.Root.transform.position = Input.mousePosition + new Vector3(80, 0, 0);
    }

    public void OnUseItem()
    {
        this.Close(WinowResult.No);
    }

    public void OnDeleteItem()
    {
        this.Close(WinowResult.No);
    }
}
