using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class TargetingController
{

    public static List<Enemy> visibleEnemies = new List<Enemy>();
    public static int selectionIndex = 0;
    public static Enemy currentEnemy;

    public static void RefreshList()
    {     
        if(visibleEnemies.Count < 1)
        {
            return;
        }
        visibleEnemies = BubbleSort();
        selectionIndex = 0;
        currentEnemy = visibleEnemies[selectionIndex];

    }
    public static void RefreshList(int index)
    {
        if(visibleEnemies.Count < 1)
        {
            return;
        }
        visibleEnemies = BubbleSort();
        selectionIndex = index;
        if (selectionIndex < 0)
        {
            selectionIndex = visibleEnemies.Count - 1;
        }
        if (selectionIndex >= visibleEnemies.Count - 1)
        {
            selectionIndex = 0;
        }
        currentEnemy = visibleEnemies[selectionIndex];
    }
    public static void ScrollEnemy(int direction)
    {
        selectionIndex += direction;
        if(selectionIndex < 0)
        {
            selectionIndex = visibleEnemies.Count - 1;
        }
        if(selectionIndex >= visibleEnemies.Count - 1)
        {
            selectionIndex = 0;
        }
        visibleEnemies = BubbleSort();
        currentEnemy = visibleEnemies[selectionIndex];
    }
    public static void ScrollIndex()
    {
        if(visibleEnemies.Count < 1)
        {
            return;
        }
        selectionIndex -= 1;
        if (selectionIndex < 0)
        {
            selectionIndex = visibleEnemies.Count - 1;
        }
        currentEnemy = visibleEnemies[selectionIndex];
    }

    static List<Enemy> BubbleSort()
    {
        List<Enemy> newList = new List<Enemy>();
        List<float> distances = new List<float>();
        for (int i = 0; i < visibleEnemies.Count; i++)
        {
            newList.Add(visibleEnemies[i]);
            distances.Add(Vector3.Distance(Character.currentPosition, newList[i].transform.position));
        }
        List<Enemy> finalTemp = new List<Enemy>();
        while(newList.Count > 0)
        {
            Enemy n = NextEnemy(newList, distances);
            int index = newList.IndexOf(n);
            newList.Remove(n);
            distances.Remove(distances[index]);
            finalTemp.Add(n);
        }
        return finalTemp;
        
    }
    static Enemy NextEnemy(List<Enemy> givenList, List<float> dist)
    {
        int index = -1;
        float smallestDst = float.MaxValue;
        for(int i = 0; i < givenList.Count; i++)
        {
            if(dist[i] < smallestDst)
            {
                smallestDst = dist[i];
                index = i;
            }
        }
        return givenList[index];
    }
}
