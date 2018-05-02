using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class CharacterStats
{
    public const int hp = 50;
    public int atk = 10;
    public int def = 10;
    public int spd = 10;
    public int vit = 10;
    [Space(20)]
    public int level;
    public int exp;
}
