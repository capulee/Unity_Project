using UnityEngine;

public class SeaTaxCarrier : MonoBehaviour
{
    public GameObject boatPrefab;
    public Vector3 finalDestination; // 추가

    private GameObject boatInstance;
    private int goldAmount;
    private Vector3 destination; // 항구 위치
    private Building origin;
    private float safetyLevel;
    private bool hasDelivered = false;
    private Port port;

    public void Init(int gold, Vector3 portPosition, Vector3 finalDestination, Building originBuilding, float safety, Port portRef)
    {
        goldAmount = gold;
        destination = portPosition;
        this.finalDestination = finalDestination;
        origin = originBuilding;
        safetyLevel = Mathf.Clamp01(safety);
        port = portRef;

        boatInstance = Instantiate(boatPrefab, transform);
        boatInstance.transform.localPosition = Vector3.zero;
    }

    private void Update()
    {
        if (hasDelivered) return;

        float moveSpeed = 3f;
        Vector3 direction = (destination - transform.position).normalized;
        float distanceToTarget = Vector3.Distance(transform.position, destination);
        float step = moveSpeed * Time.deltaTime;

        if (distanceToTarget <= step)
        {
            transform.position = destination;
            TryDeliverToPort();
            return;
        }

        transform.position += direction * step;
    }

    private void TryDeliverToPort()
    {
        if (hasDelivered) return;
        hasDelivered = true;

        float roll = Random.value;
        bool success = roll <= safetyLevel;

        if (success)
        {
            Debug.Log($"✅ 해상 수송 성공! 항구 도착");

            if (port != null)
            {
                port.DispatchLandCarrier(goldAmount, finalDestination, origin, safetyLevel);
            }
            else
            {
                Debug.LogWarning("❌ 항구 포인터가 null입니다. 사람을 파견할 수 없습니다.");
            }
        }
        else
        {
            Debug.LogWarning($"❌ 해상 수송 실패! 세금 {goldAmount} 손실됨");
        }

        origin.OnTaxDelivered();
        Destroy(gameObject);
    }
}

