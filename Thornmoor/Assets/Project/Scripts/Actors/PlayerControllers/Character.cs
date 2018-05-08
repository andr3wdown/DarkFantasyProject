using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Jopi.UPG;

public class Character : MonoBehaviour
{
    [Header("Properties")]
    [Range(1f, 20f)]
    public float characterSpeed;
    [Range(5f, 25f)]
    public float jumpForce;
    [Range(1f,360f)]
    public float turnSpeed;
    public float dashForce = 20f;
    public Transform cameraDirector; 
    public Transform target;
    public Transform hand;
    public bool isLocked = true;
    public LayerMask groundLayer;
    public LayerMask enemyLayer;
    public LayerMask interactables;
    public Transform pointer;
    public float enemyCheckRadius = 4f;
    public float interactionRadius = 3f;
    public GameObject trail;
    [Space(5)]
    [Header("Controls")]
    public string horizontalAxis = "Horizontal";
    public string verticalAxis = "Vertical";
    public KeyCode jumpButton = KeyCode.Joystick1Button0;
    public KeyCode altJumpButton = KeyCode.Space;
    public KeyCode lockButton = KeyCode.Joystick1Button9;
    public KeyCode altLockButton = KeyCode.C;
    public KeyCode dashButton = KeyCode.Joystick1Button1;
    public KeyCode altDashButton = KeyCode.K;
    public KeyCode interactButton = KeyCode.Joystick1Button7;
    public KeyCode altInteractButton = KeyCode.F;
    public string targetSwitchingAxis = "Horizontal2";

    //private variables
    private Vector2 inputVector;
    private Rigidbody rb;
    private bool stopped = false;
    private bool dashing = false;
    private bool knocking = false;
    private Animator anim;
    private Cooldown targetingCooldown = new Cooldown(0.75f);
    [HideInInspector]
    public Inventory inventory;

    //static variables
    public static Vector3 currentPosition;
    public static int surroundingEnemyCount = 0;
    public static Transform handHolder;


    private void Start()
    {
        handHolder = hand;
        anim = transform.GetChild(0).GetComponent<Animator>();
        rb = GetComponent<Rigidbody>();
    }
    private void Update()
    {
        GetEnemyCount();
        HandleInput();
        //trail.SetActive(stopped);
        currentPosition = transform.position;
    }
    private void FixedUpdate()
    { 
        Movement();
        if(new Vector3(rb.velocity.x, 0, rb.velocity.z).magnitude > 0.1f && !dashing)
        {
            rb.velocity = UPGMath.SlowPlanarRigidbodyVelocity(rb.velocity, 0.9f);
        }
    }
    void GetEnemyCount()
    {
        surroundingEnemyCount = Physics.OverlapSphere(transform.position, enemyCheckRadius, enemyLayer).Length;        
    }
    void Movement()
    {
        if (!knocking)
        {
            if (inputVector.magnitude > 0.2f)
            {
                inputVector.Normalize();
                Vector3 movementVector = (cameraDirector.forward * inputVector.y) + (cameraDirector.right * inputVector.x);
                if (!isLocked)
                {

                    transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(movementVector, Vector3.up), turnSpeed * Time.deltaTime);
                    transform.position += transform.forward * characterSpeed * Time.deltaTime;
                }
                else
                {
                    transform.position += movementVector.normalized * characterSpeed * Time.deltaTime;
                }

            }
            if (isLocked)
            {
                if (target != null)
                {

                    transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(new Vector3(target.position.x, transform.position.y, target.position.z) - transform.position, Vector3.up), turnSpeed * Time.deltaTime);
                }
            }
        }
        if (!GroundCheck())
        {
            if (rb.velocity.y < 0)
            {
                rb.AddForce(-Vector3.up * 20f, ForceMode.Acceleration);
            }
            else
            {

            }
        }
    }
    [HideInInspector]
    public bool isGrounded = false;
    public AudioClip jumpSound;
    public AudioClip dashSound;
    void HandleInput()
    {      
        if(Input.GetKeyDown(interactButton) || Input.GetKeyDown(altInteractButton))
        {
            InteractionCheck();
        }
        inputVector = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
        isGrounded = GroundCheck();
        if (!stopped)
        {
            if (!isGrounded)
            {
                turnSpeed = 1.5f;
            }
            else
            {
                turnSpeed = 15f;
            }
        }



        if (!knocking && !stopped)
        {
            if (Input.GetKeyDown(jumpButton) && isGrounded || Input.GetKeyDown(altJumpButton) && isGrounded)
            {
                rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
                anim.SetTrigger("Jump");
            }

            Targeting();


            if (Input.GetKeyDown(dashButton) || Input.GetKeyDown(altDashButton))
            {
                if (!dashing)
                {

                    Vector3 dashVector = (cameraDirector.forward * inputVector.normalized.y) + (cameraDirector.right * inputVector.normalized.x);
                    StartCoroutine(Dash(dashVector));
                }

            }
        }
   
        anim.SetBool("isGrounded", isGrounded);
        anim.SetBool("isWalking", inputVector.magnitude > 0.1f && isGrounded);
        anim.SetFloat("spdMult", characterSpeed / 15f);
    }

    void Targeting()
    {
        if (TargetingController.visibleEnemies.Count <= 0)
        {
            isLocked = false;
        }
        if (Input.GetKeyDown(lockButton) || Input.GetKeyDown(altLockButton))
        {
            Debug.Log(TargetingController.visibleEnemies.Count);
            TargetingController.RefreshList();
            if(TargetingController.currentEnemy != null)
            {
                target = TargetingController.currentEnemy.transform;
                isLocked = !isLocked;
            }
          
        }

        
        if (isLocked && Vector3.Magnitude(new Vector3(Input.GetAxis("Horizontal2"),0, Input.GetAxis("Vertical2"))) > 0.2f)
        {
            targetingCooldown.CountDown();
            if (targetingCooldown.TriggerReady())
            {
                TargetingController.GetEnemyFromAngle(cameraDirector);
                target = TargetingController.currentEnemy.transform;
            }

        }
        else
        {
            targetingCooldown.ZeroTimer();
        }


        if (isLocked)
        {
            
            pointer.gameObject.SetActive(true);
            if (target != null)
            {
                pointer.transform.position = target.transform.position + Vector3.up * 2f;
            }
            else
            {
                TargetingController.RefreshList();
                target = TargetingController.currentEnemy.transform;
                if (target == null)
                {
                    isLocked = false;
                    pointer.gameObject.SetActive(false);
                    return;
                }
                else
                {
                    pointer.transform.position = target.transform.position + Vector3.up * 2f;
                }

            }
        }
        else
        {
            pointer.gameObject.SetActive(false);
        }
    }
    IEnumerator Dash(Vector3 ip)
    {
            dashing = true;
            anim.SetTrigger("Dash");
            rb.AddForce(ip.normalized * dashForce, ForceMode.Impulse);
            yield return new WaitForSeconds(0.2f);
            while (new Vector3(rb.velocity.x, 0, rb.velocity.z).magnitude > 0.1f)
            {
                rb.velocity = UPGMath.SlowPlanarRigidbodyVelocity(rb.velocity, 0.9f);
                yield return new WaitForEndOfFrame();
            }
            dashing = false;
      
    }
    bool GroundCheck(float charHeigth = 1.2f)
    {
        return Physics.Raycast(transform.position, -transform.up, charHeigth, groundLayer);
    }
    IEnumerator AttackPattern(float duration)
    {
        stopped = true;  
        turnSpeed = 1f;
        yield return new WaitForSecondsRealtime(duration);      
        turnSpeed = 15f;
        stopped = false;
    }
    public void StopMessage(float duration)
    {
        if (!stopped && !knocking)
        {
            StartCoroutine(AttackPattern(duration));
        }
        else
        {
            Debug.Log("Already attacking");
        }
    }
    void KnockMessage()
    {
        StopCoroutine(KnockPattern());
        StartCoroutine(KnockPattern());
    }
    IEnumerator KnockPattern()
    {
        knocking = true;
        yield return new WaitForSecondsRealtime(0.6f);
        knocking = false;
    }
    void InteractionCheck()
    {
        Collider[] interactablesInRange = Physics.OverlapSphere(transform.position, interactionRadius, interactables);
        if(interactablesInRange.Length > 0)
        {
            Transform closest = UPGMath.GetClosest(interactablesInRange, transform);
            closest.GetComponent<Interactable>().Interact();
        }
    }
    
    public void TriggerKnockback(Vector3 d, float force, ForceMode fm = ForceMode.Impulse)
    {
        KnockMessage();
        d.y = 0;
        d.Normalize();
        Quaternion lookr = Quaternion.LookRotation((transform.position - d) - transform.position, Vector3.up);
        transform.rotation = lookr;
        anim.SetTrigger("Knockback");
        rb.AddForce(d * force, fm);
    }
}
