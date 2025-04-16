using UnityEngine;

public class GameManager : MonoBehaviour, IEventObserver
{
    [SerializeField]
    private MapManager mapManager;

    [SerializeField]
    private UIManager uiManager;

    [SerializeField]
    private CombatantsManager combatantsManager;

    [SerializeField]
    private GameUpdateManager gameUpdateManager;

    private void Start()
    {
        EventManager.Instance.AddEventListener(EventId.ON_START_GAME_EVENT, this);
        BulletPoolManager.Instance.Initialize();
        gameUpdateManager.Initialize();
        combatantsManager.Initialize(gameUpdateManager);
        uiManager.Initialize();
        mapManager.Initialize();
    }

    private void StartBattle(int combatantCount)
    {
        gameUpdateManager.Pause();
        LoadMap();
        LoadCombatants(combatantCount);
        uiManager.ShowLoading(true);
        combatantsManager.ActivateCombatants();
        gameUpdateManager.Play();
    }

    private void LoadMap()
    {
        MapConfiguration newMapConfig = new MapConfiguration();
        newMapConfig.MapSizeX = 10;
        newMapConfig.MapSizeY = 10;
        mapManager.SetMapConfiguration(newMapConfig);
        mapManager.LoadMapTiles();
    }

    private void LoadCombatants(int combatantCount)
    {
        CombatConfiguration combatConfiguration = new CombatConfiguration();
        combatConfiguration.CombatantCount = combatantCount;
        combatantsManager.SetCombatConfiguration(combatConfiguration);
        combatantsManager.LoadCombatants();
        combatantsManager.LoadCombatantAI();
    }

    public void OnEvent(EventId eventId, object payload)
    {
        if (eventId == EventId.ON_START_GAME_EVENT)
        {
            OnStartGamePayload data = (OnStartGamePayload)payload;
            StartBattle(data.CombatantCount);
        }
    }
}

public class OnStartGamePayload
{
    public int CombatantCount;
}