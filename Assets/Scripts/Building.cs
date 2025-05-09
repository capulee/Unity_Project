using UnityEngine;

public class Building : MonoBehaviour
{
    public int buildingId;
    public int taxAmount = 100;
    public float taxCooldown = 10f;
    private float lastCollectedTime = -999f;

    public Transform taxDestination;
    public GameObject landCarrierPrefab;
    public GameObject seaCarrierPrefab;
    public Transform spawnPoint;
    public float taxDistanceThreshold = 10f;

    public float safetyLevel = 0.9f;
    public bool isTaxInTransit = false;

    private void Awake()
    {
        if (!CompareTag("Capital"))
        {
            GameObject center = GameObject.FindGameObjectWithTag("Capital");
            if (center != null)
            {
                taxDestination = center.transform;

                Building centerBuilding = center.GetComponent<Building>();
                if (centerBuilding != null)
                {
                    safetyLevel = centerBuilding.safetyLevel;
                }
            }
            else
            {
                Debug.LogWarning("수도를 찾을 수 없습니다.");
            }
        }
    }

    private void OnMouseDown()
    {
        if (!isTaxInTransit)
        {
            TaxUi.Instance.OpenTaxUI(this);
        }
    }

    public bool CanCollect() => Time.time >= lastCollectedTime + taxCooldown;

    public float GetCooldownRemaining() =>
        Mathf.Max(0, (lastCollectedTime + taxCooldown) - Time.time);

    public void CollectTax()
    {
        if (!CanCollect()) return;
        lastCollectedTime = Time.time;

        if (CompareTag("Capital"))
        {
            Debug.Log("중심 건물 세금 즉시 징수");
            GameManager.Instance.AddGold(taxAmount);
        }
        else
        {
            float distance = Vector3.Distance(transform.position, taxDestination.position);
            Debug.Log($"세금 수송 시작 (거리: {distance:F1})");

            isTaxInTransit = true;

            bool useBoat = HasOceanBetween(transform.position, taxDestination.position);
            GameObject prefabToUse = useBoat ? seaCarrierPrefab : landCarrierPrefab;
            GameObject carrierObject = Instantiate(prefabToUse, spawnPoint.position, Quaternion.identity);

            if (useBoat)
            {
                Port nearestPort = FindNearestPort();
                if (nearestPort == null)
                {
                    Debug.LogWarning("항구를 찾을 수 없습니다.");
                    isTaxInTransit = false;
                    return;
                }

                carrierObject.GetComponent<SeaTaxCarrier>().Init(
                    taxAmount,
                    nearestPort.transform.position,      // portPosition
                    taxDestination.position,             // finalDestination
                    this,
                    safetyLevel,
                    nearestPort
                );
            }
            else
            {
                carrierObject.GetComponent<LandTaxCarrier>().Init(
                    taxAmount,
                    taxDestination.position,
                    this,
                    safetyLevel
                );
            }
        }
    }

    private Port FindNearestPort()
    {
        Port[] ports = GameObject.FindObjectsOfType<Port>();
        Port nearest = null;
        float minDistance = Mathf.Infinity;

        foreach (var port in ports)
        {
            float distance = Vector3.Distance(transform.position, port.transform.position);
            if (distance < minDistance)
            {
                minDistance = distance;
                nearest = port;
            }
        }

        return nearest;
    }

    public virtual void OnTaxDelivered()
    {
        isTaxInTransit = false;
    }

    // Raycast로 경로에 바다가 있는지 확인하는 함수
    private bool HasOceanBetween(Vector3 start, Vector3 end)
    {
        Vector3 direction = (end - start).normalized;
        float distance = Vector3.Distance(start, end);
        RaycastHit[] hits = Physics.RaycastAll(start + Vector3.up, direction, distance);

        foreach (var hit in hits)
        {
            if (hit.collider.CompareTag("Ocean"))
            {
                return true; // 경로에 Ocean이 있으면 true 반환
            }
        }

        return false; // Ocean이 없으면 false 반환
    }
}
