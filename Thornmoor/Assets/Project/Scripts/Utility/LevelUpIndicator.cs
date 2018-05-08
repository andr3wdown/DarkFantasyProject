using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelUpIndicator : MonoBehaviour
{
    public Text text;
    public Cooldown blinkCooldown = new Cooldown(3, true);
    bool isBlinking = false;
    float val = 0;
    private void Start()
    {
        CharacterStats.lvlListener += LevelUp;
    }
    void LevelUp()
    {
        blinkCooldown.ResetTimer();
        val = 0;
        isBlinking = true;
    }
    private void Update()
    {
        if (isBlinking)
        {
            text.gameObject.SetActive(true);
            blinkCooldown.CountDown();
            if (blinkCooldown.TriggerReady())
            {
                isBlinking = false;
            }
        }
        else
        {
            text.gameObject.SetActive(false);
        }
    }

}
