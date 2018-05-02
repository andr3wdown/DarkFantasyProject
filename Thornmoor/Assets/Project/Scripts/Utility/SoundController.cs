using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundController : MonoBehaviour
{
    public GameObject speaker;
    List<GameObject> speakers = new List<GameObject>();
    public int speakerCount = 10;
    private void Start()
    {
        for(int i = 0; i < speakerCount; i++)
        {
            GameObject go = Instantiate(speaker, Vector3.zero, Quaternion.identity);
            speakers.Add(go);
            go.transform.parent = transform;
            go.SetActive(false);
        }
    }
    public void PlaySound(AudioClip sound, Transform trans)
    {
        return;
        GameObject free = null;
        for(int i = 0; i < speakers.Count; i++)
        {
            if (!speakers[i].activeInHierarchy)
            {
                free = speakers[i];
                break;
            }
        }
        if(free == null)
        {
            GameObject go = Instantiate(speaker, Vector3.zero, Quaternion.identity);
            speakers.Add(go);
            go.transform.parent = transform;
            free = go;
        }
        free.SetActive(true);
        free.transform.position = trans.position;
        free.transform.rotation = trans.rotation;
        free.GetComponent<AudioSource>().PlayOneShot(sound);
    }
}
