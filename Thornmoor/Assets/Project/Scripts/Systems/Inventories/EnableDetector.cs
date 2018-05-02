using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnableDetector : MonoBehaviour
{
    private void OnEnable()
    {
        Time.timeScale = 0;
        Inventory.instance.invChangeCallback.Invoke();
    }
    private void OnDisable()
    {
        Time.timeScale = 1;
    }

}
