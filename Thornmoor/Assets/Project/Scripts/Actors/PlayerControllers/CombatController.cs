using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatController : MonoBehaviour
{

    public KeyCode lightAttack;
    public KeyCode altLightAttack;
    public KeyCode heavyAttack;
    public KeyCode altHeavyAttack;

    public Cooldown attackCooldown;
    Animator anim;

    public AudioClip attackSound;
    public void Start()
    {
        anim = transform.GetChild(0).GetComponent<Animator>();
    }
    public void Update()
    {
        bool attackPressed = Input.GetKeyDown(lightAttack) || Input.GetKeyDown(altLightAttack);
        bool heavyPressed = Input.GetKeyDown(heavyAttack) || Input.GetKeyDown(altHeavyAttack);

        attackCooldown.CountDown();

        if (attackPressed && attackCooldown.TransformingTrigger(0.25f))
        {
            Attack("Attack1", 0.25f);
        }
        if(heavyPressed && attackCooldown.TransformingTrigger(0.923f))
        {
            Attack("Attack2", 0.923f);
        }
    }
    void Attack(string attackCommand, float duration)
    {
        anim.SetTrigger(attackCommand);
        this.gameObject.SendMessage("StopMessage", duration);
    }
    IEnumerator DestructionCounter(float duration, GameObject gm)
    {
        yield return new WaitForSeconds(duration);
        Destroy(gm);
    }
}
