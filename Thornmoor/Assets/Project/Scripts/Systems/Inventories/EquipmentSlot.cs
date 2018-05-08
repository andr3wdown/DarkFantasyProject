using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EquipmentSlot : MonoBehaviour
{
    Item item;
    public Image icon;
    public void AddItem(Item _item)
    {
        item = _item;
        icon.sprite = item.UISprite;
        icon.enabled = true;
    }
    public void None()
    {
        item = null;
        icon.enabled = false;
        icon.sprite = null;
    }
}
