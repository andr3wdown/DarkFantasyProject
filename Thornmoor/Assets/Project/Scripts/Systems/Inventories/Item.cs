using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
[CreateAssetMenu(fileName = "newItem", menuName = "InventorySystem/Item", order = 0)]
public class Item : ScriptableObject
{
    [Header("Item Properties")]
    public string itemName = "NewItem";
    public string description = "";
    public Sprite UISprite;
    public int itemValue = 0;
    public int[] equippedBonuses = { 0,0,0,0 };
    public bool isEquipped = false;
    public GameObject scenePrefab;
    public void UseItem(Transform swordHandle)
    {
        
        
    }
}
