using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoisyWater : MonoBehaviour
{
    public MeshFilter mf;
    public ScrollData[] layers;
    public float universalHeight;

    public float speedScale = 0.01f;
    void Start ()
    {
        mf = GetComponent<MeshFilter>();
	}

	void Update ()
    {
        Mesh m = mf.mesh;
        ZeroNoise(m);
        for(int i = 0; i < layers.Length; i++)
        {
            AddPerlinNoiseLayer(ref m, layers[i]);
        }
        mf.mesh = m;
	}
    void ZeroNoise(Mesh _m)
    {
        Vector3[] vertices = _m.vertices;
        float scale = 0;
        for (int j = 0; j < layers.Length; j++)
        {
            scale += layers[j].heightScale;
        }
        scale /= layers.Length;
        scale *= universalHeight;
        for (int i = 0; i < vertices.Length; i++)
        {
            
            vertices[i].y = -scale / 2f; 
        }
        _m.vertices = vertices;
        _m.RecalculateNormals();
        _m.RecalculateBounds();
    }
    void AddPerlinNoiseLayer(ref Mesh _m, ScrollData data)
    {
        data.scroll.x += data.scrollSpeed.x * Time.deltaTime * speedScale;
        data.scroll.y += data.scrollSpeed.y * Time.deltaTime * speedScale;
        Vector3[] vertices = _m.vertices;
        for (int i = 0; i < vertices.Length; i++)
        {
            float y = Mathf.PerlinNoise(((vertices[i].x) + data.scroll.x) * data.noiseScale, ((transform.position.z - vertices[i].z) + data.scroll.y) * data.noiseScale);
            vertices[i].y += y * data.heightScale * universalHeight;
        }
        _m.vertices = vertices;
        _m.RecalculateNormals();
        _m.RecalculateBounds();
    }
}
[System.Serializable]
public class ScrollData
{
    public float heightScale;
    public float noiseScale;
    public Vector2 scrollSpeed;
    [HideInInspector]
    public Vector2 scroll = Vector2.zero;

    public ScrollData(float hScale, float nScale, Vector2 sSpeed)
    {
        heightScale = hScale;
        noiseScale = nScale;
        scrollSpeed = sSpeed;
    }
}

