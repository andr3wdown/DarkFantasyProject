using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public static class NameGenerator
{
    const string path = "/Project/TextGeneratorDatabase/Database/";
    private static string[] firstNames;
    private static string[] middleNames;
    private static string[] lastNames;
    private static bool hasBeenInitialized = false;
    public static string GenerateWeaponName(int rarity = 0, int type = 0, int status = 0)
    {
        if (!hasBeenInitialized)
        {
            string whole = Application.dataPath + path;
            string[] lines1 = File.ReadAllLines(@whole + "FirstNames.txt");
            firstNames = lines1;
            lines1 = File.ReadAllLines(@whole + "MiddleNames.txt");
            middleNames = lines1;
            lines1 = File.ReadAllLines(@whole + "LastNames.txt");
            lastNames = lines1;
            hasBeenInitialized = true;
        }
        string name = "";
        name += firstNames[rarity];
        int index = 0;
        switch (status)
        {
            default:
                index = status;
                break;
            case 3:
                index = Random.Range(3, 5);
                break;
            case 4:
                index = Random.Range(3, 5);
                break;
        }
        name += " " + lastNames[index];
        name += " " + middleNames[type < 1 ? Random.Range(0, 3) : Random.Range(3, 5)];

        return name;
    }

}
