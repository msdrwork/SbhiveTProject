using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MapManager : MonoBehaviour, IEventObserver
{
    [SerializeField]
    private GameObject mapTileContainer;

    private List<GameObject> mapTiles;
    private MapConfiguration mapConfig;

    public void Initialize()
    {
        EventManager.Instance.AddEventListener(EventId.ON_RESET_GAME_EVENT, this);
        mapTiles = new List<GameObject>();
    }

    public void SetMapConfiguration(MapConfiguration mapConfig)
    {
        this.mapConfig = mapConfig;
    }

    // TODO: (ADR) If theres time, convert this into a map maker
    public IEnumerator LoadMapTiles()
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
        yield return null;
    }

    public void ClearMapTiles()
    {
        for (int i = 0; i < mapTiles.Count; i++)
        {
            Destroy(mapTiles[i].gameObject);
        }
        mapTiles.Clear();
    }

    public void OnEvent(EventId eventId, object payload)
    {
        if (eventId == EventId.ON_RESET_GAME_EVENT)
        {
            ClearMapTiles();
        }
    }
}