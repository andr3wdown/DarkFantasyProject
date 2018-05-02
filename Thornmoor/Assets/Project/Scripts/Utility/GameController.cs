using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
    public static bool ended = false;
    Cooldown c = new Cooldown(2f, true);
    private void OnEnable()
    {
        ended = false;
    }
    public void GameEnded()
    {
        Debug.Log("ended");
        c = new Cooldown(2f, true);
        ended = true;
    }
    private void Update()
    {
        if (ended)
        {
            c.CountDown();
            if (c.TriggerReady())
            {
                SceneManager.LoadScene(1);
            }
        }
    }

}
