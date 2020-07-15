using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PerlinNoise
{
    private int width;
    private int height;
    private float scale;
    private float offsetX = 0;
    private float offsetY = 0;

    public Texture2D GenerateTexture(int width, int height, float scale)
    {
        this.width = width;
        this.height = height;
        this.scale = scale;
        offsetX = Random.Range(0, 99999f);
        offsetY = Random.Range(0, 99999f);

        Texture2D texture = new Texture2D(width, height);

        for (int x = 0; x < width; x++)
        {
            for(int y = 0; y < height; y++)
            {
                Color color = CalculateColor(x, y);
                texture.SetPixel(x, y, color);
            }
        }

        texture.Apply();
        return texture;
    }

    Color CalculateColor(int x, int y)
    {
        float xCoord = (float)x / width * scale + offsetX;
        float yCoord = (float)y / height * scale + offsetY;
        
        float sample = Mathf.PerlinNoise(xCoord, yCoord);
        return new Color(sample, sample, sample);
    }
}
