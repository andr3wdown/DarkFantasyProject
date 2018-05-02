using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemPickup : Interactable
{
    public Item item;
    public override void Interact()
    {
        base.Interact();
        Pickup();
    }
    void Pickup()
    {
        bool wasSuccesful = Inventory.instance.AddItem(item);
        if (wasSuccesful)
        {
            Debug.Log("Picked up " + item.itemName);
            Destroy(gameObject);
        }
            
    }
    public void Prepare()
    {
        GetComponent<Collider>().enabled = false;
        GetComponent<Rigidbody>().isKinematic = true;
        GetComponent<Rigidbody>().useGravity = false;
        transform.GetChild(0).GetChild(0).gameObject.SetActive(true);
        transform.GetChild(0).GetChild(1).gameObject.SetActive(false);
    }
}
