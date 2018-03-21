using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrailDestroyer : MonoBehaviour
{

	void Update ()
    {
		if(transform.parent == null)
        {
            transform.GetComponent<TrailRenderer>().autodestruct = true;
        }
	}
}
