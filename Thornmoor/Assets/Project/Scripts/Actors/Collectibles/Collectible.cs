using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Collectible : MonoBehaviour
{
    Transform target;
    public LayerMask playerLayer;
    public float attractionRadius;
    public float speed = 10f;
    public float rotationSpeed = 25f;
    public float startTimer;
    protected bool started = false;
    SoundController sc;
    public AudioClip pickupSound;

    public virtual void Start()
    {
        StartCoroutine(StartDelay());
    }
    protected void FixedUpdate()
    {
        if (started)
        {
            if (target != null)
            {
                Quaternion rot = Quaternion.LookRotation(target.position - transform.position, Vector3.up);
                transform.rotation = Quaternion.Slerp(transform.rotation, rot, rotationSpeed * Time.fixedDeltaTime);
                transform.position = Vector3.Lerp(transform.position, target.position, speed * Time.fixedDeltaTime);
            }
            else
            {
                Collider[] c = Physics.OverlapSphere(transform.position, attractionRadius, playerLayer);
                if (c.Length > 0)
                {
                    target = c[0].transform;
                }
            }
        }
    }
    protected void OnDrawGizmosSelected()
    {
        Color c = Color.green;
        c.a = 0.3f;
        Gizmos.color = c;
        Gizmos.DrawSphere(transform.position, attractionRadius);
    }
    protected IEnumerator StartDelay()
    {
        yield return new WaitForSeconds(1.5f);
        started = true;
    }
    public abstract void OnTriggerEnter(Collider other);
}
