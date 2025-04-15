using UnityEngine;

public class GameManager : MonoBehaviour
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
        gameUpdateManager.Initialize();
        combatantsManager.Initialize(gameUpdateManager);
        uiManager.Initialize(StartBattle);
        mapManager.Initialize();
    }

    private void StartBattle(int combatantCount)
    {
        gameUpdateManager.Pause();
        //LoadMap();
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
}
