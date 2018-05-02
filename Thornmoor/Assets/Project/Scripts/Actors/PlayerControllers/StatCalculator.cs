using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class StatCalculator
{
    public static float GetSpeedRatio(int stat)
    {
        stat -= 10;
        float value = 1;
        value += 0.3f * (stat / 20f);
        return value;
    }
    public static int GetHP(int stat)
    {
        int hp = 50;
        hp += stat * 5;
        return hp;
    }
    public static int GetDamage(int stat, int weaponDamage, float attackMultiplier)
    {
        float damage = weaponDamage * attackMultiplier * (0.667f + stat / 30f);
        return Mathf.RoundToInt(damage);
    }
    public static int GetPlayerDamage(int mobDamage, int stat)
    {
        stat -= 10;
        float damage = mobDamage * (1f - (0.33f * (stat / 20f)));
        return Mathf.RoundToInt(damage);
    }

}
