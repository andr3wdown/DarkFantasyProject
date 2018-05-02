using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryToggle : MonoBehaviour
{
    public static bool isPaused = false;
    public GameObject invCanvas;
	void Update ()
    {
        if(Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.Joystick1Button6))
        {
            isPaused = !isPaused;
            invCanvas.SetActive(isPaused);
        }
	}
}
