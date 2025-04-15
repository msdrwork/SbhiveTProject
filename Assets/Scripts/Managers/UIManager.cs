using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

using TMPro;

public class UIManager : MonoBehaviour
{
    [SerializeField]
    private CanvasGroup menuCanvasGroup;

    [SerializeField]
    private CanvasGroup loadingCanvasGroup;

    [SerializeField]
    private Button startButton;

    [SerializeField]
    private TMP_InputField inputField;

    private UnityAction<int> onStartClicked;

    public void Initialize(UnityAction<int> onStartClicked)
    {
        this.onStartClicked = onStartClicked;
        startButton.onClick.AddListener(OnStartClicked);

    }

    private void OnStartClicked()
    {
        menuCanvasGroup.alpha = 0;
        menuCanvasGroup.blocksRaycasts = false;
        menuCanvasGroup.interactable = false;
        onStartClicked?.Invoke(int.Parse(inputField.text));
    }

    public void ShowLoading(bool isShow)
    {
        loadingCanvasGroup.alpha = isShow ? 1 : 0;
        loadingCanvasGroup.blocksRaycasts = isShow;
        loadingCanvasGroup.interactable = isShow;
    }
}
