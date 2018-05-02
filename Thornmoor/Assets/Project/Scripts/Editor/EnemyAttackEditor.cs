using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(EnemyAttack))]
public class EnemyAttackEditor : Editor
{
    public void OnSceneGUI()
    {
        EnemyAttack ea = (EnemyAttack)target;
        Color c = Color.cyan;
        c.a = 0.3f;
        Handles.color = c;       
        Handles.DrawSolidArc(ea.transform.position, Vector3.up, ea.transform.forward, 360, ea.attackRange);
    }

}
