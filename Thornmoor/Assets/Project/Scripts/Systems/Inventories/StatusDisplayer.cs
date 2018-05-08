using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StatusDisplayer : MonoBehaviour
{
    Inventory inventory;
    public Text[] sText;
    public string[] displayNames;
	void Start ()
    {       
        
        CharacterStats.statChangeListener += UpdateStatus;
	}
    private void OnEnable()
    {
        if(inventory == null)
            inventory = Inventory.instance;
        UpdateStatus();
    }
    void UpdateStatus()
    {
        sText[0].text = displayNames[0] + inventory.stats.level.ToString();
        sText[1].text = displayNames[1] + inventory.stats.atk.ToString();
        sText[2].text = displayNames[2] + inventory.stats.def.ToString();
        sText[3].text = displayNames[3] + inventory.stats.spd.ToString();
        sText[4].text = displayNames[4] + inventory.stats.vit.ToString();
        sText[5].text = displayNames[5] + inventory.stats.statPoints.ToString();
        sText[6].text = displayNames[6] + inventory.stats.exp.ToString();
        sText[7].text = displayNames[7] + inventory.stats.nextLevel.ToString();
    }
}
