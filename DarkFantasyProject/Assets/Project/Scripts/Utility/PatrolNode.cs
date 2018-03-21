using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatrolNode : MonoBehaviour
{
    public List<Transform> points = new List<Transform>();
    public bool connectNodes = false;
    public Transform GetRandomPoint()
    {
        return points[Random.Range(0, points.Count)];
    }
    private void OnDrawGizmos()
    {
        if(points.Count > 0)
        {
            for(int i = 0; i < points.Count; i++)
            {
                Gizmos.color = Color.green;
                Gizmos.DrawSphere(points[i].position, 0.3f);
                if (connectNodes)
                {
                    if(i == 0)
                    {
                        
                        Gizmos.DrawLine(points[points.Count - 1].position, points[i].position);
                    }
                    else
                    {
                        Gizmos.DrawLine(points[i - 1].position, points[i].position);
                    }
                    
                }
            }
        }
    }
}
