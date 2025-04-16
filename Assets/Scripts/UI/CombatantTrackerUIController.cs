using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CombatantTrackerUIController : MonoBehaviour, IEventObserver
{
    [SerializeField]
    private Image healthBarFill;

    [SerializeField]
    private TextMeshProUGUI countTxt;

    [SerializeField]
    private TextMeshProUGUI nameTxt;

    [SerializeField]
    private TextMeshProUGUI combatantStateTxt;

    private Combatant combatant;

    public void Initialize(string countTxt, Combatant combatant)
    {
        EventManager.Instance.AddEventListener(EventId.ON_COMBATANT_DAMAGED_EVENT, this);
        EventManager.Instance.AddEventListener(EventId.ON_COMBATANT_DEAD_EVENT, this);
        this.combatant = combatant;

        this.countTxt.text = countTxt;
        nameTxt.text = combatant.CombatantName;

        combatantStateTxt.text = "State: Alive";
    }

    public void OnEvent(EventId eventId, object payload)
    {
        if (eventId == EventId.ON_COMBATANT_DAMAGED_EVENT)
        {
            OnCombatantDamagedPayload data = (OnCombatantDamagedPayload)payload;
            if (data.CombatantId == combatant.CombatantId)
            {
                UIUtils.UpdateHealthBar(healthBarFill, data.healthPercentage);
            }
        }
        else if (eventId == EventId.ON_COMBATANT_DEAD_EVENT)
        {
            OnCombatantDeadPayload data = (OnCombatantDeadPayload)payload;
            if (data.CombatantId == combatant.CombatantId)
            {
                combatantStateTxt.text = "State: Dead";
            }
        }
    }

    public void Destroy()
    {
        EventManager.Instance.RemoveEventListener(EventId.ON_COMBATANT_DAMAGED_EVENT, this);
        EventManager.Instance.RemoveEventListener(EventId.ON_COMBATANT_DEAD_EVENT, this);

        Destroy(this.gameObject);
    }
}
