using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Jopi.UPG;

[RequireComponent(typeof(HPObject))]
[RequireComponent(typeof(NavMeshAgent))]
[RequireComponent(typeof(EnemyVision))]
public class BasicEnemy : Enemy
{
    [HideInInspector]
    public NavMeshAgent agent;
    [Space(5)]
    [Header("Unit Properties")]
    
    public float hearingRadius = 10f;
    public float alertRadius = 20f;
    public LayerMask playerLayer;
    public LayerMask enemyLayer;
    public float patrolTolerance = 0.5f;
    public float runningSpeed = 5f;
    public float walkingSpeed = 2f;
    public float walkingTreshold = 6f;
    public float circlingDistance = 6f;

    [Space(5)]
    [Header("Drop Properties")]
    public float hpChance = 0.05f;
    public float launchStrength = 7f;
    public int dropAmount = 5;
    public GameObject[] drops;

    [Space(5)]
    [Header("Debugging")]
    public bool showGizmos = true;
    public Color hearingGizmoColor;
    public Color alertGizmoColor;

    Transform patrolTarget;
    private EnemyVision ev;
    private EnemyAttack ea;
    private PatrolNode patrolNode;
    public bool stopped = false;

    public AudioClip attackSound;
    SoundController sc;
    Animator anim;
    public ActionPhase phase = ActionPhase.patrolling;

    public string[] tauntAnimations;
    public float[] animationDuration;
    public Cooldown tauntCooldown = new Cooldown(4f, true);

    public override void Start()
    {
        if(transform.parent != null)
        {
            patrolNode = transform.parent.GetComponent<NavigationSetup>().node;
        }     
        transform.parent = null;
        anim = transform.GetChild(0).GetComponent<Animator>();
        base.Start();
        sc = FindObjectOfType<SoundController>();
        ev = GetComponent<EnemyVision>();
        ea = GetComponent<EnemyAttack>();
        agent = GetComponent<NavMeshAgent>();
    }
    public void GetAlerted(Transform targ)
    {
        target = targ;;
        Alert();
    }
    private void Alert()
    {
        if(target != null)
        {
            if (TauntChance(50f))
                TriggerTaunt();
            Collider[] alertRangeUnits = Physics.OverlapSphere(transform.position, alertRadius, enemyLayer);
            if (alertRangeUnits.Length > 0)
            {
                for (int i = 0; i < alertRangeUnits.Length; i++)
                {
                    if (alertRangeUnits[i].GetComponent<BasicEnemy>().target == null)
                    {
                        StartCoroutine(AlertDelay(alertRangeUnits[i].GetComponent<BasicEnemy>()));
                    }
                }
            }
        }

    }
    IEnumerator AlertDelay(BasicEnemy be, float delay=1f)
    {
        yield return new WaitForSeconds(delay);
        be.GetAlerted(target);
    }
    public override void Update()
    {
       
        base.Update();
        if (!stopped && !GameController.ended)
        {
            phase = EnemyStateController.GetPhase(this);
            float ratio = 0;
            if (agent != null)
            {
                ratio = agent.velocity.magnitude / runningSpeed;
            }
            
            if (anim != null)
                anim.SetFloat("SpeedRatio", ratio);

            if (phase == ActionPhase.patrolling)
            {
                agent.speed = walkingSpeed;
                agent.stoppingDistance = 0;
                VisionChecks();
                Patrol();
            }
            else if(phase == ActionPhase.attacking)
            {
                TauntControl();
                if(Vector3.Distance(transform.position, target.position) < walkingTreshold)
                {
                    agent.speed = walkingSpeed;
                    
                }
                else
                {
                    agent.speed = runningSpeed;
                }
                
                agent.stoppingDistance = 2f;
                
                Movement(target);
            }
            else if(phase == ActionPhase.observing)
            {
                Observe();
            }
            else
            {

            }
        }
    }
    void TriggerTaunt()
    {
        int index = Random.Range(0, tauntAnimations.Length);
        anim.SetTrigger(tauntAnimations[index]);
        StopMessage(animationDuration[index]);
    }
    void TauntControl(float d=4f)
    {
        if (Character.surroundingEnemyCount > 2)
        {
            tauntCooldown.CountDown();
            if (tauntCooldown.TransformingTrigger(d))
            {
                if (TauntChance(33f))
                {
                    TriggerTaunt();
                    return;
                }
            }
        }
    }
    bool TauntChance(float chance)
    {
            return Random.Range(0, 100) < chance;
    }
    Cooldown c = new Cooldown(3f, true);
    Cooldown ck = new Cooldown(1f, false);
    public virtual void Patrol()
    {
        if(patrolTarget != null)
        {
            if (Vector3.Distance(transform.position, patrolTarget.position) < patrolTolerance)
            {
                c.CountDown();
                if (c.TriggerReady())
                {
                    agent.angularSpeed = 250f;
                    patrolTarget = patrolNode.GetRandomPoint();
                }             
            }
            else
            {
                if(agent.isOnNavMesh)
                    agent.SetDestination(patrolTarget.position);
            }
            
        }
        else
        {
            patrolTarget = patrolNode.GetRandomPoint();
        }
        
    }
    Vector3 nextSpot = Vector3.zero;
    
    public virtual void Observe()
    {
        if (!stopped)
        {
            agent.speed = runningSpeed;
            if (nextSpot == Vector3.zero || Vector3.Distance(transform.position, nextSpot) < patrolTolerance)
            {
                ck.CountDown();
                if (ck.TriggerReady())
                {
                    ck.d = Random.Range(1f, 2.5f);
                    agent.angularSpeed = 250f;
                    Vector3 dir = new Vector3(Random.Range(-1f, 1f), 0, Random.Range(-1f, 1f)).normalized;
                    dir.y = 0;
                    nextSpot = target.position + (dir * circlingDistance);
                }
                else
                {
                    TauntControl(2f);
                    agent.velocity = Vector3.zero;
                    agent.angularSpeed = 0;
                    Quaternion lookr = Quaternion.LookRotation(target.position - transform.position, Vector3.up);
                    transform.rotation = Quaternion.Slerp(transform.rotation, lookr, ea.responseSpeed * Time.deltaTime);
                }

            }
            else
            {
                TauntControl(5f);
                agent.SetDestination(nextSpot);
                agent.angularSpeed = 250f;
            }
        }
    }
    public virtual void VisionChecks()
    {
        Transform targ;
        if(ev.HasVisible(out targ))
        {
            target = targ;
            Alert();
        }

        Collider[] heard = Physics.OverlapSphere(transform.position, hearingRadius, playerLayer);
        if(heard.Length > 0)
        {
            target = heard[0].transform;
            Alert();
        }
    }
    public virtual void StopMessage(float duration)
    {
        if (!stopped)
        {
            StartCoroutine(AttackPattern(duration));
        }
        else
        {
            Debug.Log(gameObject.name + "is already attacking!");
        }
    }
    IEnumerator AttackPattern(float duration)
    {
        stopped = true;
        agent.isStopped = true;
        yield return new WaitForSeconds(duration);
        stopped = false;
        agent.isStopped = false;

    }
    public virtual void Movement(Transform t)
    {
        if (target != null && !stopped)
        {
            agent.SetDestination(t.position);
        }
    }
    public virtual void Dead()
    {
        Drops();
        Destroy(gameObject);
    }
    public override void TriggerKnockback(Vector3 dir, float force, ForceMode fm = ForceMode.Impulse)
    {
        StopMessage(2f);
        Quaternion lookr = Quaternion.LookRotation((transform.position - dir) - transform.position, Vector3.up);
        transform.rotation = lookr;
        anim.SetTrigger("Knockback");
        rb.AddForce(dir * force, fm);
    }
    IEnumerator Waiter()
    {
        yield return new WaitForSeconds(0.5f);
        stopped = false;
    }
    public virtual void Drops()
    {
        for(int i = 0; i < dropAmount; i++)
        {
            int index = Random.Range(0f, 1f) < hpChance ? 1 : 0;
            Vector2 randomVector = new Vector2(Random.Range(-1f, 1f), Random.Range(-1f, 1f)).normalized;
            Vector3 launchVector = new Vector3(randomVector.x,2.5f,randomVector.y).normalized;
            GameObject go = Instantiate(drops[index], transform.position, Quaternion.LookRotation(transform.position + launchVector, Vector3.up));
            go.GetComponent<Rigidbody>().AddForce(launchVector * (launchStrength * Random.Range(0.75f, 1.25f)), ForceMode.Impulse);
        }
    }
}
