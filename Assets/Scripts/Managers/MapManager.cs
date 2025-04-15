using UnityEngine;
using System.Collections.Generic;

public class MapManager : MonoBehaviour
{
    [SerializeField]
    private GameObject mapTileContainer;

    private List<GameObject> mapTiles;
    private MapConfiguration mapConfig;

    public void Initialize()
    {
        mapTiles = new List<GameObject>();
    }

    public void SetMapConfiguration(MapConfiguration mapConfig)
    {
        this.mapConfig = mapConfig;
    }

    // TODO: (ADR) If theres time, convert this into a map maker
    public void LoadMapTiles()
    {
        for (int y = 0; y < mapConfig.MapSizeY; y++)
        {
            for (int x = 0; x < mapConfig.MapSizeX; x++)
            {
                GameObject mapTile = Instantiate(Resources.Load<GameObject>("Prefabs/MapTile"), mapTileContainer.transform);
                mapTile.transform.position = new Vector3(x, y, 0);
                mapTiles.Add(mapTile);
            }
        }
    }
}