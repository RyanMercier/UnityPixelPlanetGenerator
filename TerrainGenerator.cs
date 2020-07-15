using System;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEditor.SceneManagement;
using UnityEngine;

public class TerrainGenerator : MonoBehaviour
{
    private int width;
    private int height;
    private int radius;
    private float scale;
    private float surfaceLevel = 0.3f;

    public int maxPlanetSize = 15;
    public int minPlanetSize = 3;
    public int resolution = 4;

    private PerlinNoise noise;
    private Color32 surfaceColor;
    private Color32 noiseColor;

    [HideInInspector]
    public Texture2D terrain;

    void Start()
    {
        //Generate Planet Colors
        surfaceColor = new Color32((byte)UnityEngine.Random.Range(0, 255), (byte)UnityEngine.Random.Range(0, 255), (byte)UnityEngine.Random.Range(0, 255), 255);
        noiseColor = new Color32((byte)UnityEngine.Random.Range(0, 255), (byte)UnityEngine.Random.Range(0, 255), (byte)UnityEngine.Random.Range(0, 255), 255);

        //Generate Terrain Texture
        int planetSize = UnityEngine.Random.Range(minPlanetSize, maxPlanetSize);
        transform.localScale = new UnityEngine.Vector3(planetSize, planetSize, 1);


        width = planetSize * resolution + 1;
        height = planetSize * resolution + 1;
        radius = width / 2;

        scale = UnityEngine.Random.Range(5, planetSize);

        terrain = new Texture2D(width, height, TextureFormat.ARGB32, false);
        noise = new PerlinNoise();
        Texture2D noiseMap = noise.GenerateTexture(width, height, scale);

        for (int x = 0; x < noiseMap.width; x++)
        {
            for(int y = 0; y < noiseMap.height; y++)
            {
                if (UnityEngine.Vector2.Distance(new UnityEngine.Vector2(radius, radius), new UnityEngine.Vector2(x, y)) > radius)
                {
                    terrain.SetPixel(x, y, Color.clear);
                }
                
                else if (noiseMap.GetPixel(x, y).r < surfaceLevel)
                {
                    terrain.SetPixel(x, y, surfaceColor);
                }

                else
                {
                    Gradient g = new Gradient();
                    GradientColorKey[] gck = new GradientColorKey[2];
                    GradientAlphaKey[] gak = new GradientAlphaKey[2];
                    gck[0].color = noiseColor;
                    gck[0].time = 1.0F;
                    gck[1].color = surfaceColor;
                    gck[1].time = -1.0F;
                    gak[0].alpha = 1.0F;
                    gak[0].time = 1.0F;
                    gak[1].alpha = 1.0F;
                    gak[1].time = 1.0F;
                    g.SetKeys(gck, gak);

                    terrain.SetPixel(x, y, g.Evaluate(noiseMap.GetPixel(x, y).r));
                }
            }
        }

        terrain.filterMode = FilterMode.Point;
        terrain.Apply();
        GetComponent<Renderer>().material.mainTexture = terrain;
    }
}
