using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hitbox : MonoBehaviour
{
    public int damage = 10;
    public float knockBack = 10;
    public bool rootMode = true;
    [SerializeField]
    string hitType = "blade";
    public virtual void OnTriggerEnter(Collider other)
    {
        if(other.GetComponent<Enemy>() != null)
        {
            Vector3 toFrom = Vector3.zero;
            if (rootMode)
            {
                toFrom = (other.transform.position - transform.root.position).normalized;
            }
            else
            {
                toFrom = (other.transform.position - transform.position).normalized;
            }
             
            
            other.GetComponent<Enemy>().TriggerKnockback(toFrom, knockBack);
            other.GetComponent<HPObject>().TakeHP(damage, true, false, hitType, exp: true);                 
        }

    }

}
