using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Cooldown
{
    public float d;
    float c;
    public Cooldown(float _d, bool initC=false)
    {
        d = _d;
        if (initC)
        {
            c = d;
        }
    }
    public void CountDown(float rate=1f)
    {
        c -= Time.deltaTime * rate;         
    }
    public bool TriggerReady()
    {
        if (c <= 0)
        {
            c = d;
            return true;

        }
        else
        {
            return false;
        }
    }
    public bool TransformingTrigger(float next, float rate = 1f)
    {
        if (c <= 0)
        {
            c = next;
            return true;
        }
        else
        {
            return false;
        }
    }
    
}
