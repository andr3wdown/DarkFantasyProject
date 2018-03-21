using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHitbox : Hitbox
{
    public override void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<Character>() != null)
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

            other.GetComponent<Character>().TriggerKnockback(toFrom, knockBack);
            other.GetComponent<HPObject>().TakeHP(damage, true, true);
        }
    }

}
