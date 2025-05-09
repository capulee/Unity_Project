using UnityEngine;

public class LandTaxCarrier : MonoBehaviour
{
    public GameObject humanPrefab;

    private GameObject humanInstance;
    private int goldAmount;
    private Vector3 destination;
    private Building origin;
    private float safetyLevel;
    private bool hasDelivered = false;

    public void Init(int gold, Vector3 targetPosition, Building originBuilding, float safety)
    {
        goldAmount = gold;
        destination = targetPosition;
        origin = originBuilding;
        safetyLevel = Mathf.Clamp01(safety);

        humanInstance = Instantiate(humanPrefab, transform);
        humanInstance.transform.localPosition = Vector3.zero;
    }

    private void Update()
    {
        if (hasDelivered) return;

        float moveSpeed = 2f;
        Vector3 direction = (destination - transform.position).normalized;
        float distanceToTarget = Vector3.Distance(transform.position, destination);
        float step = moveSpeed * Time.deltaTime;

        if (distanceToTarget <= step)
        {
            transform.position = destination;
            TryDeliverGold();
            return;
        }

        transform.position += direction * step;
    }

    private void TryDeliverGold()
    {
        hasDelivered = true;

        float roll = Random.value;
        bool success = roll <= safetyLevel;

        if (success)
        {
            Debug.Log($"✅ 지상 수송 성공! +{goldAmount} 골드");
            GameManager.Instance.AddGold(goldAmount);
        }
        else
        {
            Debug.LogWarning($"❌ 지상 수송 실패! 세금 {goldAmount} 손실됨");
        }

        origin.OnTaxDelivered();
        Destroy(gameObject);
    }
}
