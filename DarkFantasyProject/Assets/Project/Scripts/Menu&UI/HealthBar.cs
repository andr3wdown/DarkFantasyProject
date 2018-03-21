using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class HealthBar : MonoBehaviour
{
    public Image img;
    GameController gc;
    private void Start()
    {
        gc = FindObjectOfType<GameController>();
        Time.timeScale = 1;
    }
    public void UpdateHPBar(float ratio)
    {
        img.rectTransform.localScale = new Vector3(ratio, 1, 1);
    }
    public void Dead()
    {
        gc.GameEnded();
        Destroy(gameObject);       
    }



}
