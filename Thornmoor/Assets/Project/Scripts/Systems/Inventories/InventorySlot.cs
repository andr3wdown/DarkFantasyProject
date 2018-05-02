using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventorySlot : MonoBehaviour
{
    Item item;
    public Image icon;
    public Button itemButton;
    public Button discard;


    public void AddItem(Item newItem)
    {  
        item = newItem;

        icon.enabled = true;
        icon.sprite = item.UISprite;
        discard.interactable = true;
        itemButton.interactable = true;
    }
    public void None()
    {

        item = null;
        icon.enabled = false;
        icon.sprite = null;
        discard.interactable = false;
        itemButton.interactable = false;
    }
    public void Discard()
    {
        GameObject g = item.scenePrefab;
        Inventory.instance.RemoveItem(item);
        GameObject go = Instantiate(g, Character.currentPosition, Quaternion.identity);
        go.GetComponent<Rigidbody>().AddForce(new Vector3(Random.Range(0f, 1f), 2f, Random.Range(0f, 1f)).normalized, ForceMode.Impulse);
        
    }
    public void Use()
    {
        Debug.Log("used " + item.itemName);
        Transform last = Character.handHolder.GetChild(0);
        if (last != null)
        {
            Inventory.instance.AddItem(last.GetComponent<ItemPickup>().item);
        }
        Destroy(last.gameObject);

        
        GameObject go = Instantiate(item.scenePrefab, Character.handHolder.position, Quaternion.Euler(new Vector3(Character.handHolder.rotation.eulerAngles.x + 90, Character.handHolder.rotation.eulerAngles.y, Character.handHolder.rotation.eulerAngles.z)));
        go.GetComponent<ItemPickup>().Prepare();
        go.transform.parent = Character.handHolder;
        go.transform.localPosition = Vector3.zero;
        go.transform.localRotation = Quaternion.Euler(Vector3.zero);
        Inventory.instance.RemoveItem(item);
    }
}
