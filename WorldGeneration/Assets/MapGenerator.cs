using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapGenerator : MonoBehaviour {

    public enum DrawMode {
        NoiseMap,
        ColorMap
    }
    public DrawMode drawMode;

    public bool autoUpdate; // only use to CustomEditor - MapGeneratedEditor

    [SerializeField] int _mapWidth;
    [SerializeField] int _mapHeigh;
    [SerializeField] float _noiseScale;

    [SerializeField] int _octaves;
    [Range(0,1)]
    [SerializeField] float _persistance;
    [SerializeField] float _lacunarity;

    [SerializeField] int _seed;
    [SerializeField] Vector2 _offset;

    [SerializeField] TerrainType[] regions;

    public void GenerateMap()
    {
        float[,] noiseMap = Noise.GenerateNoiseMap(_mapWidth, _mapHeigh, _seed, _noiseScale,_octaves,_persistance,_lacunarity, _offset);

        Color[] colorMap = new Color[_mapWidth * _mapHeigh];
        for (int y = 0; y < _mapHeigh; y++)
        {
            for (int x = 0; x < _mapWidth; x++)
            {
                float currentHeight = noiseMap[x, y];
                for (int i = 0; i < regions.Length; i++)
                {
                    if (currentHeight <= regions[i].Height)
                    {
                        colorMap[y * _mapWidth + x] = regions[i].Color;
                        break;
                    }
                }
            }
        }

        MapDisplay mapDisplay = FindObjectOfType<MapDisplay>();
        if (drawMode == DrawMode.NoiseMap)
        {
            mapDisplay.DrawTexture(TextureGenerator.TextureFromHeightMap(noiseMap));
        }
        else if (drawMode == DrawMode.ColorMap)
        {
            mapDisplay.DrawTexture(TextureGenerator.TextureFromColorMap(colorMap, _mapWidth, _mapHeigh));
        }

    }

    private void OnValidate()
    {
        if(_mapWidth < 1)
        {
            _mapWidth = 1;
        }
        if(_mapHeigh <1)
        {
            _mapHeigh = 1;
        }
        if(_lacunarity <1)
        {
            _lacunarity = 1;
        }
        if(_octaves <0)
        {
            _octaves = 0;
        }
    }
}

