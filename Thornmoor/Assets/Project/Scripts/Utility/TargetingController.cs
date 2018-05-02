using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class TargetingController
{

    public static List<Enemy> visibleEnemies = new List<Enemy>();
    public static Enemy currentEnemy;

    public static void RefreshList()
    {     
        if(visibleEnemies.Count < 1)
        {
            currentEnemy = null;
            return;
        }
        currentEnemy = GetClosest();

    }
    public static void GetEnemyFromAngle(Transform camDir)
    {
        if(visibleEnemies.Count < 1)
        {
            currentEnemy = null;
            return;
        }
        currentEnemy = ClosestAngle(camDir);
    }
    static Enemy ClosestAngle(Transform camDir)
    {
        float minAngle = float.MaxValue;
        int index = 0;
        for(int i = 0; i < visibleEnemies.Count; i++)
        {
            float angle = GetAngle(camDir, visibleEnemies[i].transform);
            if (angle < minAngle)
            {
                minAngle = angle;
                index = i;
            }
        }
        return visibleEnemies[index];
    }
    static float GetAngle(Transform camDir, Transform target)
    {
        Vector3 inputVector = (camDir.right * Input.GetAxis("Horizontal2")) - (camDir.forward * Input.GetAxis("Vertical2"));
        inputVector.Normalize();
        Vector3 toFromEnemy = new Vector3(target.position.x, 0, target.position.z) - new Vector3(Character.currentPosition.x, 0, Character.currentPosition.z);
        toFromEnemy.Normalize();
        return Vector3.Angle(inputVector, toFromEnemy);
    }
    public static float GetAngle(Vector3 facing, Vector3 targetPos, Vector3 askPos)
    {
        Vector3 toFromEnemy = new Vector3(targetPos.x, 0, targetPos.z) - new Vector3(askPos.x, 0, askPos.z);
        toFromEnemy.Normalize();
        return Vector3.Angle(facing, toFromEnemy);
    }
    static Enemy GetClosest()
    {
        float minDst = float.MaxValue;
        int index = 0;
        for (int i = 0; i < visibleEnemies.Count; i++)
        {
            float dst = Vector3.Distance(Character.currentPosition, visibleEnemies[i].transform.position);
            if (dst < minDst)
            {
                minDst = dst;
                index = i;
            }
        }
        return visibleEnemies[index];
    }

}
