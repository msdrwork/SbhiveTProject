using System.Collections;
using UnityEngine;
using UnityEngine.Events;

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

    private IEnumerator StartBattle(int combatantCount)
    {
        gameUpdateManager.Pause();
        uiManager.ShowMenu(false);
        uiManager.ShowLoading(true);
        yield return LoadMap();
        yield return LoadCombatants(combatantCount);
        uiManager.ShowLoading(false);
        combatantsManager.ActivateCombatants();
        gameUpdateManager.Play();
    }

    private IEnumerator LoadMap()
    {
        MapConfiguration newMapConfig = new MapConfiguration();
        newMapConfig.MapSizeX = 10;
        newMapConfig.MapSizeY = 10;
        mapManager.SetMapConfiguration(newMapConfig);
        yield return mapManager.LoadMapTiles();
    }

    private IEnumerator LoadCombatants(int combatantCount)
    {
        CombatConfiguration combatConfiguration = new CombatConfiguration();
        combatConfiguration.CombatantCount = combatantCount;
        combatantsManager.SetCombatConfiguration(combatConfiguration);
        yield return combatantsManager.LoadCombatants();
        combatantsManager.LoadCombatantAI();
    }

    public void OnEvent(EventId eventId, object payload)
    {
        if (eventId == EventId.ON_START_GAME_EVENT)
        {
            OnStartGamePayload data = (OnStartGamePayload)payload;
            StartCoroutine(StartBattle(data.CombatantCount));
        }
    }
}

public class OnStartGamePayload
{
    public int CombatantCount;
}