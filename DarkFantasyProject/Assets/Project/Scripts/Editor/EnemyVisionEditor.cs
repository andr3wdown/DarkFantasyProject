using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(EnemyVision))]
public class EnemyVisionEditor : Editor
{
    private void OnSceneGUI()
    {
        EnemyVision ev = (EnemyVision)target;
        Handles.color = Color.red;
        Handles.DrawWireArc(ev.transform.position, Vector3.up, Vector3.forward, 360, ev.viewDist);
        Vector3 viewAngleA = ev.DirFromAngle(-ev.visionAngle / 2f);
        Vector3 viewAngleB = ev.DirFromAngle(ev.visionAngle / 2f);
        Handles.color = Color.magenta;
        Handles.DrawLine(ev.transform.position, ev.transform.position + viewAngleA * ev.viewDist);
        Handles.DrawLine(ev.transform.position, ev.transform.position + viewAngleB * ev.viewDist);


    }

}
