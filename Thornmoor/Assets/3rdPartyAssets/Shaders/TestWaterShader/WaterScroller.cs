using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterScroller : MonoBehaviour
{
    UnityEngine.Material m;
    public Vector2 scrollSpeed;
    float xScroll = 0;
    float yScroll = 0;
    private void Start()
    {
        m = GetComponent<MeshRenderer>().material;
    }
    void Update ()
    {
        xScroll += scrollSpeed.x * Time.deltaTime;
        yScroll += scrollSpeed.y * Time.deltaTime;
        Vector4 n = m.GetVector("_Scroll");
        n.x = xScroll;
        n.y = yScroll;
        m.SetVector("_Scroll", n);
	}
}
