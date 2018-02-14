using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapGenerator : MonoBehaviour {

    public bool autoUpdate; // only use to CustomEditor - MapGeneratedEditor

    [SerializeField] int _mapWidth;
    [SerializeField] int _mapHeigh;
    [SerializeField] float _noiseScale;
    

    public void GenerateMap()
    {
        float[,] noiseMap = Noise.GenerateNoiseMap(_mapWidth, _mapHeigh, _noiseScale);

        MapDisplay mapDisplay = FindObjectOfType<MapDisplay>();
        mapDisplay.DrawNoiseMap(noiseMap);   
    }

}
