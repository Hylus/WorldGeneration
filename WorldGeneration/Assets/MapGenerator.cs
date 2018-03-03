using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapGenerator : MonoBehaviour {

    public enum DrawMode {
        NoiseMap,
        ColorMap,
        Mesh,

    }
    public DrawMode drawMode;

    public bool autoUpdate; // only use to CustomEditor - MapGeneratedEditor

    const int _mapChunkSize = 241;
    [Range(0,6)]
    [SerializeField] int _levelOfDetail;

    [SerializeField] float _noiseScale;

    [SerializeField] int _octaves;
    [Range(0,1)]
    [SerializeField] float _persistance;
    [SerializeField] float _lacunarity;

    [SerializeField] int _seed;
    [SerializeField] Vector2 _offset;

    [SerializeField] TerrainType[] _regions;

    [SerializeField] float _meshHeightMultiplier;

    [SerializeField] AnimationCurve _meshHeightCurve;

    public void GenerateMap()
    {
        float[,] noiseMap = Noise.GenerateNoiseMap(_mapChunkSize, _mapChunkSize, _seed, _noiseScale,_octaves,_persistance,_lacunarity, _offset);

        Color[] colorMap = new Color[_mapChunkSize * _mapChunkSize];
        for (int y = 0; y < _mapChunkSize; y++)
        {
            for (int x = 0; x < _mapChunkSize; x++)
            {
                float currentHeight = noiseMap[x, y];
                for (int i = 0; i < _regions.Length; i++)
                {
                    if (currentHeight <= _regions[i].Height)
                    {
                        colorMap[y * _mapChunkSize + x] = _regions[i].Color;
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
            mapDisplay.DrawTexture(TextureGenerator.TextureFromColorMap(colorMap, _mapChunkSize, _mapChunkSize));
        }
        else if(drawMode == DrawMode.Mesh)
        {
            mapDisplay.DrawMesh(MeshGenerator.GenerateTerrainMesh(noiseMap, _meshHeightMultiplier, _meshHeightCurve, _levelOfDetail), TextureGenerator.TextureFromColorMap(colorMap, _mapChunkSize, _mapChunkSize));
        }

    }

    private void OnValidate()
    {
        
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

