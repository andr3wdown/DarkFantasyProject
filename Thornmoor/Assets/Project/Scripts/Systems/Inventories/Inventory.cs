using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Inventory : MonoBehaviour
{
    public const int SLOT_COUNT = 32;

    #region Singleton
    public static Inventory instance;
    private void Awake()
    {
        if(instance != null)
        {
            Destroy(gameObject);
        }
        instance = this;
    }
    #endregion

    public int[] materials;
    public string[] materialNames;
    public Text[] materialDisplayers;
    public CharacterStats stats;

    public List<Item> inventory = new List<Item>();
    

    public delegate void OnInvChanged();
    public OnInvChanged invChangeCallback;



    public void AddMaterial(int i, int amount)
    {
        materials[i] += amount;
        materialDisplayers[i].text = materialNames[i] + ": " + materials[i];
    }
    public bool AddItem(Item item)
    {
        if(inventory.Count < SLOT_COUNT)
        {
            inventory.Add(item);
            if (invChangeCallback != null)
                invChangeCallback.Invoke();
         
            return true;
        }
        else
        {
            //TODO: ADD MESSAGING FUNCTION!  
            Debug.Log("Inventory full!");
            return false;                   
        }
    }
    public void RemoveItem(Item item)
    {
        inventory.Remove(item);
        if (invChangeCallback != null)
            invChangeCallback.Invoke();
        
    }
	
}
