using System.Collections;
using System.Collections.Generic;
using Mediapipe.Unity;
using UnityEngine;

public class GetBodyPoints : MonoBehaviour
{
  public PoseLandmarkListAnnotation poseLandMarkListAnnotation;
  private void Awake()
  {
    poseLandMarkListAnnotation.OnPointsCreated.AddListener((points) => { BodyPartAngles.bodyPoints = points.children; });
  }
  // Start is called before the first frame update
  void Start()
  {

  }

  // Update is called once per frame
  void Update()
  {
    Debug.Log(BodyPartAngles.ReturnMidPointOfShoulders());
  }
}
