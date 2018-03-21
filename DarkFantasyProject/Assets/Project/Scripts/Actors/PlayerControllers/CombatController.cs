using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatController : MonoBehaviour
{

    public KeyCode lightAttack;
    public KeyCode altLightAttack;
    public KeyCode heavyAttack;
    public KeyCode altHeavyAttack;

    public GameObject attackPrefab;
    public Cooldown attackCooldown;
    Animator anim;
    SoundController sc;

    public AudioClip attackSound;
    public void Start()
    {
        sc = FindObjectOfType<SoundController>();
        anim = transform.GetChild(0).GetComponent<Animator>();
    }
    public void Update()
    {
        bool attackPressed = Input.GetKeyDown(lightAttack) || Input.GetKeyDown(altLightAttack);
        bool heavyPressed = Input.GetKeyDown(heavyAttack) || Input.GetKeyDown(altHeavyAttack);

        attackCooldown.CountDown();

        if (attackPressed && attackCooldown.TransformingTrigger(0.29f))
        {
            Attack();
        }
        if(heavyPressed && attackCooldown.TransformingTrigger(0.45f))
        {
            HeavyAttack();
        }
    }
    void Attack()
    {
        sc.PlaySound(attackSound, transform);
        //GameObject go = Instantiate(attackPrefab, transform.position, transform.rotation);
        anim.SetTrigger("Attack1");
        this.gameObject.SendMessage("StopMessage", 0.286f);
        //StartCoroutine(DestructionCounter(0.286f, go));
    }
    void HeavyAttack()
    {
        sc.PlaySound(attackSound, transform);
        anim.SetTrigger("Attack2");
        this.gameObject.SendMessage("StopMessage2", 0.444f);
    }
    IEnumerator DestructionCounter(float duration, GameObject gm)
    {
        yield return new WaitForSeconds(duration);
        Destroy(gm);
    }
}
