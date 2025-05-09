using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public int gold = 0;  // 현재 소유 골드

    [Header("UI")]
    public TextMeshProUGUI goldText; // 🆕 골드 표시용 텍스트

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        UpdateGoldUI();
    }

    public void AddGold(int amount)
    {
        gold += amount;
        Debug.Log($"💰 골드 추가됨: 현재 골드 = {gold}");
        UpdateGoldUI();
    }

    public void SpendGold(int amount)
    {
        if (gold >= amount)
        {
            gold -= amount;
            Debug.Log($"🛒 골드 사용: 현재 골드 = {gold}");
            UpdateGoldUI();
        }
        else
        {
            Debug.LogWarning("⚠️ 골드가 부족합니다!");
        }
    }

    private void UpdateGoldUI()
    {
        if (goldText != null)
        {
            goldText.text = $"Gold: {gold}";
        }
    }
}
