using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using TMPro;

public class UIManager : MonoBehaviour, IEventObserver
{
    [SerializeField]
    private CanvasGroup menuCanvasGroup;

    [SerializeField]
    private CanvasGroup loadingCanvasGroup;

    [SerializeField]
    private CanvasGroup gameCanvasGroup;

    [SerializeField]
    private GameObject winScreen;

    [SerializeField]
    private TextMeshProUGUI winScreenText;

    [SerializeField]
    private TextMeshProUGUI winScreenWinner;

    [SerializeField]
    private Image winScreenImage;

    [SerializeField]
    private Button resetButton;

    [SerializeField]
    private Transform combatantTrackerContainer;

    [SerializeField]
    private Button startButton;

    [SerializeField]
    private TMP_InputField inputField;

    private List<CombatantTrackerUIController> combatantTrackerUIControllers;
    private CombatantTrackerUIController combatantTrackerRef;
    private float combatantCount;

    public void Initialize()
    {
        EventManager.Instance.AddEventListener(EventId.ON_COMBATANT_CREATED_EVENT, this);
        EventManager.Instance.AddEventListener(EventId.ON_COMBATANT_WIN_EVENT, this);
        EventManager.Instance.AddEventListener(EventId.ON_ALL_COMBATANT_DEAD_EVENT, this);

        resetButton.onClick.AddListener(OnResetClicked);
        startButton.onClick.AddListener(OnStartClicked);
        combatantCount = 0;

        combatantTrackerUIControllers = new List<CombatantTrackerUIController>();
    }

    private void OnResetClicked()
    {
        ShowMenu(true);
        ShowGameScreen(false);       
        ClearCombatantCounter();
        winScreen.gameObject.SetActive(false);
        inputField.text = string.Empty;
        startButton.enabled = true;
        combatantCount = 0;

        EventManager.Instance.SendEvent(EventId.ON_RESET_GAME_EVENT, null);
    }

    private void OnStartClicked()
    {
        startButton.enabled = false;
        ShowMenu(false);
        ShowGameScreen(true);
        EventManager.Instance.SendEvent(EventId.ON_START_GAME_EVENT, new OnStartGamePayload()
        {
            CombatantCount = int.Parse(inputField.text),
        });
    }

    private void ShowGameScreen(bool isShow)
    {
        gameCanvasGroup.alpha = isShow ? 1 : 0;
        gameCanvasGroup.blocksRaycasts = isShow;
        gameCanvasGroup.interactable = isShow;
    }

    public void ShowMenu(bool isShow)
    {
        menuCanvasGroup.alpha = isShow ? 1 : 0;
        menuCanvasGroup.blocksRaycasts = isShow;
        menuCanvasGroup.interactable = isShow;
    }

    public void ShowLoading(bool isShow)
    {
        loadingCanvasGroup.alpha = isShow ? 1 : 0;
        loadingCanvasGroup.blocksRaycasts = isShow;
        loadingCanvasGroup.interactable = isShow;
    }

    private void ShowWinScreen(Combatant combatant)
    {
        winScreen.gameObject.SetActive(true);
        winScreenText.text = "Winner Winner Chicken Dinner";
        winScreenWinner.text = combatant.CombatantName;
        winScreenImage.gameObject.SetActive(true);
        winScreenImage.sprite = combatant.CombatantSprite;
    }

    private void ShowDrawScreen()
    {
        winScreen.gameObject.SetActive(true);
        winScreenText.text = "All my combatants are dead, push me to edge";
        winScreenWinner.text = "Grim Reaper";
        winScreenImage.gameObject.SetActive(false);
    }

    private void CreateCombatantTracker(Combatant combatant)
    {
        if (combatantTrackerRef == null)
        {
            combatantTrackerRef = Resources.Load<CombatantTrackerUIController>("Prefabs/CombatantTrackerUI");
        }

        combatantCount++;
        CombatantTrackerUIController tracker = Instantiate(combatantTrackerRef, combatantTrackerContainer);
        tracker.Initialize(combatantCount.ToString(), combatant);
        combatantTrackerUIControllers.Add(tracker);
    }

    private void ClearCombatantCounter()
    {
        for (int i = 0; i < combatantTrackerUIControllers.Count; i++)
        {
            combatantTrackerUIControllers[i].Destroy();
        }
        combatantTrackerUIControllers.Clear();
    }

    public void OnEvent(EventId eventId, object payload)
    {
        if (eventId == EventId.ON_COMBATANT_CREATED_EVENT)
        {
            OnCombatantCreatedPayload data = (OnCombatantCreatedPayload)payload;
            CreateCombatantTracker(data.Combatant);
        }
        else if (eventId == EventId.ON_COMBATANT_WIN_EVENT)
        {
            OnCombatantWinPayload data = (OnCombatantWinPayload)payload;
            ShowWinScreen(data.Combatant);
        }
        else if (eventId == EventId.ON_ALL_COMBATANT_DEAD_EVENT)
        {
            ShowDrawScreen();
        }
    }

    public void Destroy()
    {
        EventManager.Instance.RemoveEventListener(EventId.ON_COMBATANT_CREATED_EVENT, this);
        EventManager.Instance.RemoveEventListener(EventId.ON_COMBATANT_WIN_EVENT, this);
        EventManager.Instance.RemoveEventListener(EventId.ON_ALL_COMBATANT_DEAD_EVENT, this);
        ClearCombatantCounter();
    }
}
