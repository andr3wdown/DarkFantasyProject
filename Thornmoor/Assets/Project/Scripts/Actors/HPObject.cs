using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HPObject : MonoBehaviour
{
    private int maxHp = 100;
    public int hp;
    public string thisHittedSound = "event:/hit_generic";
    float nextSoundTime = 0;
    bool init = false;
    public void TakeHP(int amount, bool deathMessage = false, bool hitMessage = false, string hitType = "blade", bool exp = false)
    {
        if (!init)
        {
            maxHp = hp;
            
        }
        hp -= amount;
        if (hitMessage)
        {
            gameObject.SendMessage("UpdateHPBar", DisplayRatio);
        }
        if (hp <= 0)
        {
            if (deathMessage)
            {
                if (exp)
                {
                    Inventory.instance.stats.AddExp(GetComponent<Enemy>().gainedExp);
                }
               
                gameObject.SendMessage("Dead");
            }
            hp = 0; 
        }
        float soundCooldown = 0.2f;

        if (Time.time > nextSoundTime)
        {
            nextSoundTime = Time.time + soundCooldown;
            if (hitType == "blade") FMODUnity.RuntimeManager.PlayOneShot("event:/hit_blade_generic", transform.position);
            else if (hitType == "stone")
            {
                FMODUnity.RuntimeManager.PlayOneShot("event:/hit_stone", transform.position);
                if (Random.value > 0.3f) FMODUnity.RuntimeManager.PlayOneShot("event:/hit_bone", transform.position);
            }
        }

        FMODUnity.RuntimeManager.PlayOneShot(thisHittedSound, transform.position);
       
    }
    public void HealHP(int amount)
    {
        hp += amount;
        if(hp >= maxHp)
        {
            hp = maxHp;
        }
        gameObject.SendMessage("UpdateHPBar", DisplayRatio);
    }
    public float DisplayRatio
    {
        get
        {
            return (float)hp / (float)maxHp;
        }
    }
    public bool Dead
    {
        get
        {
            return hp <= 0;
        }
    }
}
