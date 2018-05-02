using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Jopi.UPG;

public class Enemy : MonoBehaviour
{

    public static List<Enemy> attackingEnemies = new List<Enemy>();
    [Header("General Properties")]
    public Transform target;
    protected Rigidbody rb;
    public virtual void Start()
    {
        TargetingController.visibleEnemies.Add(this);
        rb = GetComponent<Rigidbody>();
    }
    public virtual void Update()
    {
        if (target != null)
        {
            if (!attackingEnemies.Contains(this))
            {
                attackingEnemies.Add(this);
            }
        }
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
        if (attackingEnemies.Contains(this))
        {
            attackingEnemies.Remove(this);
        }
        Remove();
        if(TargetingController.currentEnemy = this)
        {
            TargetingController.RefreshList();
        }
        
    }
    private void OnBecameInvisible()
    {     
        Remove();
    }

}
public enum ActionPhase
{
    patrolling,
    attacking,
    observing,
    special
}
