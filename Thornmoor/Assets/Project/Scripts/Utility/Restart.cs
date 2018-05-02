using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Restart : MonoBehaviour
{
    Cooldown cooldown = new Cooldown(1, true);
    private void Update()
    {
        if (!Input.GetKey(KeyCode.R))
        {
            cooldown.ResetTimer();
        }
        else
        {
            cooldown.CountDown();
            if (cooldown.TriggerReady())
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            }
        }
    }
}
