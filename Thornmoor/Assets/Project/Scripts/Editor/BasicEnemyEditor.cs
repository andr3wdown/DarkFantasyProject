using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(BasicEnemy))]
public class BasicEnemyEditor : Editor {

    private void OnSceneGUI()
    {
        BasicEnemy be = (BasicEnemy)target;
        Handles.color = be.hearingGizmoColor;
        Handles.DrawWireArc(be.transform.position, Vector3.up, be.transform.forward, 360, be.hearingRadius);
        Handles.color = be.alertGizmoColor;
        Handles.DrawWireArc(be.transform.position, Vector3.up, be.transform.forward, 360, be.alertRadius);
    }
}
