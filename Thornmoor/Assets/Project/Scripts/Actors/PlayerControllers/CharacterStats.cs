using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class CharacterStats
{
    public const int hp = 50;
    public const int points_per_level = 3;
    public int atk = 10;
    public int def = 10;
    public int spd = 10;
    public int vit = 10;
    [Space(20)]
    public int statPoints;
    public int level;
    public int exp;
    public int nextLevel;

    public delegate void LevelUpListener();
    public static LevelUpListener lvlListener;

    public delegate void StatChangedListener();
    public static StatChangedListener statChangeListener;

    public void LevelUp()
    {
        if(statChangeListener != null)
        {
            statChangeListener.Invoke();
        }
        if(lvlListener != null)
        {
            lvlListener.Invoke();
        }
        level++;
        statPoints += points_per_level;
    }
    public void AddExp(int amount)
    {
        if (statChangeListener != null)
        {
            statChangeListener.Invoke();
        }
        exp += amount;
        if(exp >= nextLevel)
        {
            LevelUp();
            nextLevel = ExpCurve.GetLevelUpExp(level);
        }
    }
    public void IncreaseStat(string stat)
    {
        if (statChangeListener != null)
        {
            statChangeListener.Invoke();
        }
        if (stat == "atk")
        {
            atk++;
        }
        else if(stat == "def")
        {
            def++;
        }
        else if (stat == "spd")
        {
            spd++;
        }
        else if(stat == "vit")
        {
            vit++;
        }
        else
        {
            Debug.LogError("Invalid stat");
        }
    }
}
