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
    public bool isLocked = true;
    public LayerMask groundLayer;
    public Transform pointer;
    [Space(5)]
    [Header("Controls")]
    public string horizontalAxis = "Horizontal";
    public string verticalAxis = "Vertical";
    public KeyCode jumpButton = KeyCode.Joystick1Button0;
    public KeyCode altJumpButton = KeyCode.Space;
    public KeyCode lockButton = KeyCode.Joystick1Button9;
    public KeyCode altLockButton = KeyCode.F;
    public KeyCode dashButton = KeyCode.Joystick1Button1;
    public KeyCode altDashButton = KeyCode.K;
    public string targetSwitchingAxis = "Horizontal2";
    bool hasReturnedAxis = true;

    //private variables
    private Vector2 inputVector;
    private Rigidbody rb;
    private bool stopped = false;
    private bool dashing = false;
    private Animator anim;
    private SoundController sc;

    //static variables
    public static Vector3 currentPosition;

    private void Start()
    {
        sc = FindObjectOfType<SoundController>();
        anim = transform.GetChild(0).GetComponent<Animator>();
        rb = GetComponent<Rigidbody>();
    }
    private void Update()
    {
        HandleInput();
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
    void Movement()
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
       
      
        
      
        if (Input.GetKeyDown(jumpButton) && isGrounded || Input.GetKeyDown(altJumpButton) && isGrounded)
        {
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            anim.SetTrigger("Jump");
            sc.PlaySound(jumpSound, transform);
        }

        Targeting();
        

        if (Input.GetKeyDown(dashButton) || Input.GetKeyDown(altDashButton))
        {
            if (!dashing)
            {
                
                Vector3 dashVector = (cameraDirector.forward * inputVector.normalized.y) + (cameraDirector.right * inputVector.normalized.x);
                sc.PlaySound(dashSound, transform);
                StartCoroutine(Dash(dashVector));
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
            TargetingController.RefreshList();
            if(TargetingController.currentEnemy != null)
            {
                target = TargetingController.currentEnemy.transform;
                isLocked = !isLocked;
            }
          
        }


        if (isLocked && Mathf.Abs(Input.GetAxisRaw(targetSwitchingAxis)) > 0.2f && hasReturnedAxis)
        {
            hasReturnedAxis = false;
            if (Input.GetAxis(targetSwitchingAxis) > 0)
            {
                TargetingController.ScrollEnemy(-1);
            }
            else
            {
                TargetingController.ScrollEnemy(1);
            }
            target = TargetingController.currentEnemy.transform;
        }

        if (Mathf.Abs(Input.GetAxis(targetSwitchingAxis)) < 0.2f)
        {
            hasReturnedAxis = true;
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
                isLocked = !isLocked;
                if (target == null)
                {
                    isLocked = false;
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
        characterSpeed = 2f;    
        turnSpeed = 1f;
        yield return new WaitForSeconds(duration);
        yield return null;
        characterSpeed = 8f;
        turnSpeed = 15f;
        stopped = false;
    }
    IEnumerator Attack2Pattern(float duration)
    {
        stopped = true;
        turnSpeed = 1f;
        yield return new WaitForSeconds(duration);
        yield return null;
        turnSpeed = 8f;
        stopped = false;
    }

    public void StopMessage(float duration)
    {
        if (!stopped)
        {
            StartCoroutine(AttackPattern(duration));
        }
        else
        {
            Debug.Log("Already attacking");
        }
      
    }
    public void StopMessage2(float duration)
    {
        if (!stopped)
        {
            StartCoroutine(Attack2Pattern(duration));
        }
        else
        {
            Debug.Log("Already attacking");
        }

    }
    public virtual void TriggerKnockback(Vector3 dir, float force, ForceMode fm = ForceMode.Impulse)
    {
        rb.AddForce(dir * force, fm);
    }
}
