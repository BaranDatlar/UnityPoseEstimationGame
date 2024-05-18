using UnityEngine;
using UnityEngine.Events;
using System.Collections.Generic;
using System.Collections;

public class RoadSpawner : MonoBehaviour
{
    public GameObject planePrefab; // Prefab olarak kullanılacak plane objesi
    public GameObject collisionWall;
    public GameObject initialPlane;

    public Transform roadSpawner;
    public float spawnInterval = 2f; // Yeni plane objesinin yaratılma süresi2
    public int maxPlanes = 5; // Aynı anda sahnede bulunacak maksimum plane sayısı
    public float destroyDelay = 2f;

    private List<GameObject> planes = new List<GameObject>(); // Yaratılan plane objelerini saklayacak liste
    private float planeLength; // Plane objesinin uzunluğu

    public UnityEvent onSpawnPlane; // Plane yaratma eventi


    void Start()
    {
        // Prefab'ın uzunluğunu hesapla
        MeshRenderer renderer = planePrefab.transform.GetChild(0).GetComponent<MeshRenderer>();
        if (renderer != null)
        {
            planeLength = renderer.bounds.size.z; // Plane'in Z ekseni boyunca uzunluğunu al
        }
        Debug.Log("PLANE LENGTH" + planeLength);

        // İlk plane objesini yarat ve listeye ekle
        //GameObject initialPlane = Instantiate(planePrefab, Vector3.zero, Quaternion.identity);
        planes.Add(initialPlane);

        // Plane yaratma eventini ayarla ve başlat
        onSpawnPlane = new UnityEvent();
        onSpawnPlane.AddListener(SpawnPlane);
    
        InvokeRepeating(nameof(InvokeSpawnPlane), spawnInterval, spawnInterval);
    }

    void InvokeSpawnPlane()
    {
        onSpawnPlane.Invoke();
    }

    void SpawnPlane()
    {
        // Son plane objesinin pozisyonunu al
        Vector3 spawnPosition = planes[planes.Count - 1].transform.position + new Vector3(0, 0, planeLength);

        // Yeni plane objesini yarat ve listeye ekle
        GameObject newPlane = Instantiate(planePrefab, spawnPosition, Quaternion.identity, roadSpawner);
        planes.Add(newPlane);
        Transform collisionWallTransform = newPlane.transform.GetChild(4);


        // Eski plane objesini yok et ve listeden çıkar
        if (planes.Count > maxPlanes)
        {
            Destroy(planes[0]);
            planes.RemoveAt(0);
        }
    }

    void RemoveOldPlanes()
    {
        // CollisionWall'dan sonra objeleri yok et
        StartCoroutine(DestroyOldPlanesAfterDelay());
    }

    IEnumerator DestroyOldPlanesAfterDelay()
    {
        // Belirtilen süre kadar bekle
        yield return new WaitForSeconds(destroyDelay);

        if (planes.Count > 0)
        {
            Destroy(planes[0]);
            planes.RemoveAt(0);
        }
    }
}
