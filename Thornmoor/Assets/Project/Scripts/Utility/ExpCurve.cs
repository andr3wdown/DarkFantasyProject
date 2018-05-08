using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ExpCurve
{
    public const int firstLevel = 100;
	public static int GetLevelUpExp(int nextLevel, int multiplier = 2)
    {
        return nextLevel * multiplier * firstLevel; 
    }
}
