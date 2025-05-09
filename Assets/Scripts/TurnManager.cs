using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class TurnManager : MonoBehaviour
{
    public int currentTurn = 1;
    public int maxActionsPerTurn = 3;
    private int remainingActions;

    public TextMeshProUGUI turnText;
    public TextMeshProUGUI actionText;
    public Button endTurnButton;

    public delegate void OnTurnChanged(int newTurn);
    public static event OnTurnChanged TurnChanged;

    void Start()
    {
        StartTurn();

        if (endTurnButton != null)
            endTurnButton.onClick.AddListener(EndTurn);
    }

    void StartTurn()
    {
        remainingActions = maxActionsPerTurn;
        UpdateTurnText();
        UpdateActionText();

        if (endTurnButton != null)
            endTurnButton.interactable = true;

        TurnChanged?.Invoke(currentTurn);
    }

    public bool UseAction()
    {
        if (remainingActions <= 0) return false;

        remainingActions--;
        UpdateActionText();

        // �� �̻� �ൿ�� �� ���ٸ�, ��ư�� ���ܵε� �ൿ �Ұ�
        return true;
    }

    public void EndTurn()
    {
        currentTurn++;
        StartTurn();
    }

    void UpdateTurnText()
    {
        if (turnText != null)
            turnText.text = $"{currentTurn}";
    }

    void UpdateActionText()
    {
        if (actionText != null)
            actionText.text = $"{remainingActions}/{maxActionsPerTurn}";
    }

    public bool CanAct()
    {
        return remainingActions > 0;
    }
}
