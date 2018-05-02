using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrailMover : MonoBehaviour
{
    public GameObject target;
    Vector3 prevVel;
	void LateUpdate ()
    {
        transform.rotation = Quaternion.Slerp(transform.rotation, target.transform.rotation, 360f * Time.deltaTime);
        transform.position = Vector3.Lerp(transform.position, target.transform.position, 40f * Time.deltaTime);
	}
}
