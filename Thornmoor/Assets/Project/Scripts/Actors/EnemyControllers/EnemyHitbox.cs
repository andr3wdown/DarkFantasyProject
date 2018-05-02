using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHitbox : Hitbox
{
    //type of hitting for sound etc.
    [SerializeField]
    string thisHitType = "stone";

    bool hit = false;
    public override void OnTriggerEnter(Collider other)
    {  
        if (other.GetComponent<Character>() != null)
        {
            if (!hit)
            {
                hit = true;
                Vector3 toFrom = Vector3.zero;
                if (rootMode)
                {
                    toFrom = (other.transform.position - transform.root.position).normalized;
                    
                }
                else
                {
                    toFrom = (other.transform.position - transform.position).normalized;
                }
                Debug.Log(toFrom);

                other.GetComponent<Character>().TriggerKnockback(toFrom, knockBack);
                other.GetComponent<HPObject>().TakeHP(damage, true, true, thisHitType);
            }
        }
        
       
    }
    private void OnEnable()
    {
        hit = false;
    }

}
