using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStateController : MonoBehaviour
{
    static List<Enemy> attacking = new List<Enemy>();
    static List<Enemy> observing = new List<Enemy>();
    public int enemyCombatCount = 3;

    static EnemyStateController instance;
    private void OnEnable()
    {
        instance = this;
    }
    void UpdateLists()
    {
        attacking.Clear();      
        observing.Clear();
        List<Enemy> e = new List<Enemy>();
        for (int i = 0; i < Enemy.attackingEnemies.Count; i++)
        {
            e.Add(Enemy.attackingEnemies[i]);
        }

        if (Enemy.attackingEnemies.Count <= enemyCombatCount)
        {
            attacking = e;
        }
        else
        {  
            for (int i = 0; i < enemyCombatCount; i++)
            {
                Enemy enemy = e[GetBestIndex(e)];
                e.Remove(enemy);
                attacking.Add(enemy);
            }
            observing = e;
        }

       
       
       
    }
    int GetBestIndex(List<Enemy> es)
    {
        float closest = float.MaxValue;
        int val = 0;
        for (int i = 0; i < es.Count; i++)
        {
            float dst = Vector3.Distance(es[i].transform.position, es[i].target.position);
            if (dst < closest)
            {
                closest = dst;
                val = i;
            }
        }
        return val;
    }   
    public static ActionPhase GetPhase(Enemy e)
    {
        if (GameController.ended)
        {
            e.target = null;
            return ActionPhase.patrolling;
        }
        instance.UpdateLists();
        if (Enemy.attackingEnemies.Contains(e))
        {
            if (attacking.Contains(e))
            {
                return ActionPhase.attacking;
            }
            else if (observing.Contains(e))
            {
                return ActionPhase.observing;
            }
            else
            {
                Debug.Log(Enemy.attackingEnemies.Contains(e));
                Debug.LogError("Enemy state error!");
                return 0;
            }
        }
        else
        {
            return ActionPhase.patrolling;
        }
    }
    public static bool inCombat
    {
        get
        {
            return Enemy.attackingEnemies.Count > 0;
        }
    }
}
