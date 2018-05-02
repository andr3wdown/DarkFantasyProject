using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAttack : MonoBehaviour
{
    [Header("AttackingProperties")]
    public Cooldown attackCooldown;
    public float attackRange = 2f;
    public float responseSpeed = 15f;
    public float angleTreshold;
    public LayerMask playerLayer;
    Animator anim;
    BasicEnemy be;

    [Header("AnimationProperties")]
    public string[] animationNames = { "Smash", "Swing" };
    public float[] animationDurations = { };
    public float[] attackDistances = { 3f, 1.5f };
    int nextAttack;
    private void Start()
    {
        be = GetComponent<BasicEnemy>();
        anim = transform.GetChild(0).GetComponent<Animator>();
        attackCooldown = new Cooldown(attackCooldown.d, false);
        SetAttack();
    }
    public virtual void Update()
    {
        if (!be.stopped)
        {
            switch (be.phase)
            {
                case ActionPhase.attacking:
                    if (InRange())
                    {
                        be.agent.angularSpeed = 0;
                        Quaternion lookr = Quaternion.LookRotation(be.target.position - transform.position, Vector3.up);
                        transform.rotation = Quaternion.Slerp(transform.rotation, lookr, responseSpeed * Time.deltaTime);
                        attackCooldown.CountDown();
                        if (attackCooldown.TriggerReady())
                        {
                            Attack(animationNames[nextAttack], animationDurations[nextAttack]);
                            SetAttack();
                        }
                    }
                    else
                    {
                        be.agent.angularSpeed = 250;
                    }
                    break;
                case ActionPhase.observing:

                    break;

            }
            if (!InRange())
            {
                attackCooldown.ZeroTimer();
            }
        }
  

    }
    public void SetAttack()
    {
        nextAttack = Random.Range(0, animationNames.Length);
        attackRange = attackDistances[nextAttack];
    }
    void Attack(string name, float animDuration)
    {
        anim.SetTrigger(name);
        gameObject.SendMessage("StopMessage", animDuration);
        
    }
    bool InRange()
    {
        Collider[] c = Physics.OverlapSphere(transform.position, attackRange, playerLayer);
        return  c.Length > 0;
    }
}
