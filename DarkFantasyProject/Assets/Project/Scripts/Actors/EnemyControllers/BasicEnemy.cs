using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

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
    public PatrolNode patrolNode;
    public bool stopped = false;

    public AudioClip attackSound;
    SoundController sc;

    public override void Start()
    {
        base.Start();
        sc = FindObjectOfType<SoundController>();
        ev = GetComponent<EnemyVision>();
        agent = GetComponent<NavMeshAgent>();
    }
    public void GetAlerted(Transform targ)
    {
        target = targ;
        Alert();
    }
    private void Alert()
    {
        Collider[] alertRangeUnits = Physics.OverlapSphere(transform.position, alertRadius, enemyLayer);
        if(alertRangeUnits.Length > 0)
        {
            for(int i = 0; i < alertRangeUnits.Length; i++)
            {
                if(alertRangeUnits[i].GetComponent<BasicEnemy>().target == null)
                {
                    //alertRangeUnits[i].GetComponent<BasicEnemy>().GetAlerted(target);
                    StartCoroutine(AlertDelay(alertRangeUnits[i].GetComponent<BasicEnemy>()));
                }
            }
        }
    }
    IEnumerator AlertDelay(BasicEnemy be, float delay=1f)
    {
        yield return new WaitForSeconds(delay);
        be.GetAlerted(target);
    }
    public virtual void Update()
    {
        if(target == null)
        {
            agent.speed = walkingSpeed;
            agent.stoppingDistance = 0;
            VisionChecks();
            Patrol();
        }
        else
        {
            agent.speed = runningSpeed;
            agent.stoppingDistance = 3f;
            Movement();
        }

        
        
    }
    Cooldown c = new Cooldown(3f, true);
    public virtual void Patrol()
    {
        if(patrolTarget != null)
        {
            if (Vector3.Distance(transform.position, patrolTarget.position) < patrolTolerance)
            {
                c.CountDown();
                if (c.TriggerReady())
                {
                    patrolTarget = patrolNode.GetRandomPoint();
                }             
            }
            else
            {
                agent.SetDestination(patrolTarget.position);
            }
            
        }
        else
        {
            patrolTarget = patrolNode.GetRandomPoint();
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
        sc.PlaySound(attackSound, transform);
        stopped = true;
        agent.isStopped = true;
        yield return new WaitForSeconds(duration);
        stopped = false;
        agent.isStopped = false;

    }
    public virtual void Movement()
    {
        if (target != null && !stopped)
        {
            agent.SetDestination(target.position);
        }
    }
    public virtual void Dead()
    {
        Drops();
        Destroy(gameObject);
    }
    
    public virtual void Drops()
    {
        for(int i = 0; i < dropAmount; i++)
        {
            int index = Random.Range(0f, 1f) < hpChance ? 1 : 0;
            Vector2 randomVector = new Vector2(Random.Range(-1f, 1f), Random.Range(-1f, 1f)).normalized;
            Vector3 launchVector = new Vector3(randomVector.x,2.5f,randomVector.y).normalized;
            GameObject go = Instantiate(drops[index], transform.position, Quaternion.LookRotation(transform.position + launchVector, Vector3.up));
            go.GetComponent<Rigidbody>().AddForce(launchVector * launchStrength, ForceMode.Impulse);
        }
    }
   

}
