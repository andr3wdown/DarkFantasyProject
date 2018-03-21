using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyVision : MonoBehaviour
{
    public LayerMask playerLayer;
    public LayerMask obstacleLayers;

    public float viewDist;
    [Range(0, 360)]
    public float visionAngle;


    private Transform target;


    public Vector3 DirFromAngle(float angleDeg, bool angleIsGlobal=false)
    {
        if (!angleIsGlobal)
        {
            angleDeg += transform.eulerAngles.y;
        }
        return new Vector3(Mathf.Sin(angleDeg * Mathf.Deg2Rad), 0, Mathf.Cos(angleDeg * Mathf.Deg2Rad));
    }
    public bool HasVisible(out Transform targ)
    {
        Collider[] targetsInRange = Physics.OverlapSphere(transform.position, viewDist, playerLayer);
        for(int i = 0; i < targetsInRange.Length; i++)
        {
            Transform target = targetsInRange[i].transform;
            Vector3 dirToTarget = (target.position - transform.position).normalized;
            if(Vector3.Angle(transform.forward, dirToTarget) < visionAngle / 2f)
            {
                float dstToTarget = Vector3.Distance(transform.position, target.position);
                if(!Physics.Raycast(transform.position, dirToTarget, dstToTarget, obstacleLayers))
                {
                    targ = target;
                    return true;
                }
            }
        }
        targ = null;
        return false;
    }

}
