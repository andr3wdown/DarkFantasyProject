using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Speaker : MonoBehaviour
{
    AudioSource sr;
	void Start ()
    {
        sr = GetComponent<AudioSource>();
	}

	void Update ()
    {
        if (!sr.isPlaying)
        {
            gameObject.SetActive(false);
        }
	}
}
