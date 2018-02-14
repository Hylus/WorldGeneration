using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Noise {

	public static float [,] GenerateNoiseMap(int mapWidth, int mapHeight, float scale)
    {
        float[,] noiseMap = new float[mapWidth, mapHeight];

        if(scale<=0)
        {
            scale = 0.001f;
        }

        for (int y = 0; y < mapHeight; y++)
        {
            for (int x = 0; x< mapWidth; x++)
            {
                float sampleX = x / scale;
                float sampleY = y / scale;

                float perlinNoise = Mathf.PerlinNoise(sampleX, sampleY); //Returns value between 0.0 and 1.0
                noiseMap[x, y] = perlinNoise;
            }
        }

        return noiseMap;
    }
}
