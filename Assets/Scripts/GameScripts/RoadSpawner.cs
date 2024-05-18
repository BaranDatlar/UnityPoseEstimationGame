using UnityEngine;
using UnityEngine.Events;
using System.Collections.Generic;
using System.Collections;

public class RoadSpawner : MonoBehaviour
{
    public GameObject planePrefab; 
    public GameObject collisionWall;
    public GameObject initialPlane;

    public Transform roadSpawner;

    public float spawnInterval = 2f; 
    public int maxPlanes = 5; 
    public float destroyDelay = 2f;

    private List<GameObject> planes = new List<GameObject>(); 
    private float planeLength; 

    public UnityEvent onSpawnPlane; 


    void Start()
    {
        
        MeshRenderer renderer = planePrefab.transform.GetChild(0).GetComponent<MeshRenderer>();
        if (renderer != null)
        {
            planeLength = renderer.bounds.size.z; // Plane'in Z ekseni boyunca uzunluğunu al
        }
        Debug.Log("PLANE LENGTH" + planeLength);

        // İlk plane objesini yarat ve listeye ekle
        //GameObject initialPlane = Instantiate(planePrefab, Vector3.zero, Quaternion.identity);
        planes.Add(initialPlane);

        onSpawnPlane = new UnityEvent();
        onSpawnPlane.AddListener(SpawnPlane);
        CameraCalibration.instance.onCalibrationFinished.AddListener(() =>
        {
            InvokeRepeating(nameof(InvokeSpawnPlane), spawnInterval, spawnInterval);
        });
    }

    void InvokeSpawnPlane()
    {
        onSpawnPlane.Invoke();
    }

    void SpawnPlane()
    {
        Vector3 spawnPosition = planes[planes.Count - 1].transform.position + new Vector3(0, 0, planeLength);

        GameObject newPlane = Instantiate(planePrefab, spawnPosition, Quaternion.identity, roadSpawner);
        planes.Add(newPlane);
        Transform collisionWallTransform = newPlane.transform.GetChild(4);

        if (collisionWallTransform != null)
        {
            CollisionWall collisionWall = collisionWallTransform.gameObject.GetComponent<CollisionWall>();
            collisionWall.onPlayerCollision.AddListener(RemoveOldPlanes);
        }

        // Eski plane objesini yok et ve listeden çıkar
        //if (planes.Count > maxPlanes)
        //{
        //    Destroy(planes[0]);
        //    planes.RemoveAt(0);
        //}
    }

    void RemoveOldPlanes()
    {      
        StartCoroutine(DestroyOldPlanesAfterDelay());
    }

    IEnumerator DestroyOldPlanesAfterDelay()
    {
        yield return new WaitForSeconds(destroyDelay);

        if (planes.Count > 0)
        {
            Destroy(planes[0]);
            planes.RemoveAt(0);
        }
    }
}
