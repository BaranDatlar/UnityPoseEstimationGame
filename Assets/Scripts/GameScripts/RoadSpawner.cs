using UnityEngine;
using UnityEngine.Events;
using System.Collections.Generic;
using System.Collections;

public class RoadSpawner : MonoBehaviour
{
    public Transform roadSpawner;

    public float spawnInterval = 2f; 
    public int maxPlanes = 5; 
    public float destroyDelay = 0;

    public List<GameObject> randomPlanePrefabList = new List<GameObject>();
    public List<GameObject> planes = new List<GameObject>();

    private float planeLength; 

    public UnityEvent onSpawnPlane; 


    void Start()
    {
        Debug.Log("PLANES START COUNT " + planes.Count);
        MeshRenderer renderer = planes[0].transform.GetChild(0).GetComponent<MeshRenderer>();
        if (renderer != null)
        {
            planeLength = renderer.bounds.size.z; // Plane'in Z ekseni boyunca uzunluğunu al
        }

        onSpawnPlane = new UnityEvent();
        onSpawnPlane.AddListener(SpawnPlane);

        CameraCalibration.instance.onCalibrationFinished.AddListener(() =>
        {
            InvokeSpawnPlane();
        });

        

        for (int i = 2; i <= 4; i++)
        {
            Transform collisionWallTransform = planes[i].transform.GetChild(4);

            if (collisionWallTransform != null)
            {
                CollisionWall collisionWall = collisionWallTransform.gameObject.GetComponent<CollisionWall>();
                collisionWall.onPlayerCollision.AddListener(RemoveOldPlanes);
            }
            else
            {
                Debug.Log("collision null");
            }
        }

        
    }

    void InvokeSpawnPlane()
    {
        onSpawnPlane.Invoke();
    }

    void SpawnPlane()
    {
        Debug.Log("PLANES COUNT " + planes.Count);
        Vector3 spawnPosition = planes[planes.Count - 1].transform.position + new Vector3(0, 0, planeLength);
        var randomPlanePrefab = ChooseRandomPrefab(randomPlanePrefabList);
        GameObject newPlane = Instantiate(randomPlanePrefab, spawnPosition, Quaternion.identity, roadSpawner);
        planes.Add(newPlane);
        Transform collisionWallTransform = newPlane.transform.GetChild(4);

        if (collisionWallTransform != null)
        {
            CollisionWall collisionWall = collisionWallTransform.gameObject.GetComponent<CollisionWall>();
            collisionWall.onPlayerCollision.AddListener(RemoveOldPlanes);
        }
    }

    GameObject ChooseRandomPrefab(List<GameObject> planePrefab)
    {
        int randomPlaneIndex = Random.Range(0, planePrefab.Count);
        return planePrefab[randomPlaneIndex];
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
        InvokeSpawnPlane();
    }
}
