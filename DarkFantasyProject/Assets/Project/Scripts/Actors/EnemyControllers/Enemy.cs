using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Jopi.UPG;

public class Enemy : MonoBehaviour
{
    [Header("General Properties")]
    public Transform target;
    Rigidbody rb;
    public virtual void Start()
    {
        rb = GetComponent<Rigidbody>();
    }
    public virtual void FixedUpdate()
    {
        if(rb.velocity.magnitude > 0.1f)
            rb.velocity = UPGMath.SlowRigidbodyVelocity(rb.velocity, 0.95f);
    }
    public virtual void TriggerKnockback(Vector3 dir, float force, ForceMode fm = ForceMode.Impulse)
    {
        rb.AddForce(dir * force, fm);
    }
    private void OnBecameVisible()
    {
        if (!TargetingController.visibleEnemies.Contains(this))
        {
            TargetingController.visibleEnemies.Add(this);
        }
    }
    private void Remove()
    {
        if (TargetingController.visibleEnemies.Contains(this))
        {
            TargetingController.visibleEnemies.Remove(this);
        }
    }
    private void OnDisable()
    {
        Remove();
        TargetingController.ScrollIndex();
    }
    private void OnBecameInvisible()
    {     
        Remove();
    }

}
