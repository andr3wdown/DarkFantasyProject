using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HpCollectible : Collectible
{
    public int amount;
    public GameObject obj;
    public override void OnTriggerEnter(Collider other)
    {
        if (started)
        {
            if (other.GetComponent<Character>() != null)
            {
                obj.transform.parent = null;
                other.GetComponent<HPObject>().HealHP(amount);
                Destroy(gameObject);
            }
        }
     
    }
}
