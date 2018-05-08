using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryUI : MonoBehaviour
{
    Inventory inventory;
    public Transform inventoryParent;
    InventorySlot[] slots;
    public Transform equipmentParent;
    EquipmentSlot[] equipment;

    private void Start()
    {
        inventory = Inventory.instance;
        inventory.invChangeCallback += UpdateUI;


        equipment = new EquipmentSlot[5];
        for(int i = 0; i < equipmentParent.childCount; i++)
        {
            equipment[i] = equipmentParent.GetChild(i).GetComponent<EquipmentSlot>();
        }

        slots = new InventorySlot[Inventory.SLOT_COUNT];
        for (int i = 0; i < inventoryParent.childCount; i++)
        {
            slots[i] = inventoryParent.GetChild(i).GetComponent<InventorySlot>();
        }
        
    }
    private void UpdateUI()
    {
        for(int i = 0; i < slots.Length; i++)
        {
            if(i < inventory.inventory.Count)
            {
                slots[i].AddItem(inventory.inventory[i]);
            }
            else
            {
                slots[i].None();
            }
        }
        for(int i = 0; i < equipment.Length; i++)
        {
            if(inventory.equipment[i] != null)
            {
                equipment[i].AddItem(inventory.equipment[i]);
            }
            else
            {
                equipment[i].None();
            }
        }
    }
}
