using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAttack : MonoBehaviour
{
    public Cooldown attackCooldown;
    public float attackRange = 2f;
    public float animationDuration = 0.5f;
    public float responseSpeed = 15f;
    public LayerMask playerLayer;
    Animator anim;
    BasicEnemy be;
    private void Start()
    {
        be = GetComponent<BasicEnemy>();
        anim = GetComponent<Animator>();
        attackCooldown = new Cooldown(attackCooldown.d, true);
    }
    private void Update()
    {
        if (InRange() && !be.stopped)
        {
            be.agent.angularSpeed = 0;
            Quaternion lookr = Quaternion.LookRotation(be.target.position - transform.position, Vector3.up);
            transform.rotation = Quaternion.Slerp(transform.rotation, lookr, responseSpeed * Time.deltaTime);
            attackCooldown.CountDown();
            if (attackCooldown.TriggerReady())
            {
                Attack();
            }
        }
        else
        {
            be.agent.angularSpeed = 250;
        }
    }

    void Attack()
    {
        anim.SetTrigger("Attack");
        gameObject.SendMessage("StopMessage", animationDuration);
        
    }
    bool InRange()
    {
        Collider[] c = Physics.OverlapSphere(transform.position, attackRange, playerLayer);
        return  c.Length > 0;
    }
}
