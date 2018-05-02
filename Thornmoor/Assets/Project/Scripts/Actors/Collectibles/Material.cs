using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Material : Collectible
{
    Inventory inv;
    public int materialIndex = 0;
    public int amount = 10;
    public GameObject obj;
    public override void Start()
    {
        base.Start();
        inv = FindObjectOfType<Inventory>();
    }
    public override void OnTriggerEnter(Collider other)
    {
        if (started)
        {
            if (other.GetComponent<Character>() != null)
            {
                obj.transform.parent = null;
                inv.AddMaterial(materialIndex, amount);
                Destroy(gameObject);
            }
        }
     
    }

}
