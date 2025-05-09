using UnityEngine;

public class Port : MonoBehaviour
{
    public GameObject landCarrierPrefab;
    public Transform landSpawnPoint; // ← 이게 null일 가능성 높음

    public void DispatchLandCarrier(int gold, Vector3 targetPosition, Building origin, float safety)
    {
        GameObject carrier = Instantiate(landCarrierPrefab, landSpawnPoint.position, Quaternion.identity); // ⚠️ line 11
        carrier.GetComponent<LandTaxCarrier>().Init(gold, targetPosition, origin, safety);
    }
}
