using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Inventory : MonoBehaviour
{
    public int[] materials;
    public string[] materialNames;
    public Text[] materialDisplayers;
    public void AddMaterial(int i, int amount)
    {
        materials[i] += amount;
        materialDisplayers[i].text = materialNames[i] + ": " + materials[i];
    }
	
}
