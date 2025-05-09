using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TaxUi : MonoBehaviour
{
    public static TaxUi Instance;

    public GameObject taxPanel;
    public Button collectButton;
    public TextMeshProUGUI cooldownText;

    private Building currentBuilding;

    private void Awake()
    {
        Instance = this;
        taxPanel.SetActive(false);
    }

    public void OpenTaxUI(Building building)
    {
        currentBuilding = building;
        taxPanel.SetActive(true);

        UpdateCollectButtonState();
    }

    public void OnClickCollectTax()
    {
        if (currentBuilding != null && currentBuilding.CanCollect())
        {
            currentBuilding.CollectTax();
            taxPanel.SetActive(false);
        }
    }

    public void CloseUI()
    {
        taxPanel.SetActive(false);
    }

    private void Update()
    {
        if (taxPanel.activeSelf && currentBuilding != null)
        {
            UpdateCollectButtonState();
        }
    }

    void UpdateCollectButtonState()
    {
        if (currentBuilding.CanCollect())
        {
            collectButton.interactable = true;
            cooldownText.text = "세금 징수 가능!";
        }
        else
        {
            collectButton.interactable = false;
            float remaining = currentBuilding.GetCooldownRemaining();
            cooldownText.text = $"남은 쿨타임: {remaining:F1}초";
        }
    }
}
