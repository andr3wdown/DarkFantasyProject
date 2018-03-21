using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HPObject : MonoBehaviour
{
    private int maxHp = 100;
    public int hp;
    public virtual void Start()
    {
        maxHp = hp;
    }
    public void TakeHP(int amount, bool deathMessage = false, bool hitMessage=false)
    {
        hp -= amount;
        if (hitMessage)
        {
            gameObject.SendMessage("UpdateHPBar", DisplayRatio);
        }
        if (hp <= 0)
        {    
            if (deathMessage)
            {
                gameObject.SendMessage("Dead");
            }         
            hp = 0;
        }
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
