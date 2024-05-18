using System.Collections;
using System.Collections.Generic;
using Mediapipe.Unity;
using UnityEngine;

public class BodyPartAngles
{
  public static List<PointAnnotation> bodyPoints = new();

  public static float CalculateAngle(Vector3 pointA, Vector3 pointB, Vector3 pointC)
  {
    Vector3 vectorAB = pointA - pointB;
    Vector3 vectorBC = pointC - pointB;

    float angle = Vector3.Angle(vectorAB, vectorBC);
    return angle;
  }

  public static Vector3 CalculateThirdPoint(Vector3 pointA, Vector3 pointB, float angle)
  {
    float lengthAB = Vector3.Distance(pointA, pointB);

    float x = pointA.x + lengthAB * Mathf.Cos(angle);
    float y = pointA.y + lengthAB * Mathf.Sin(angle);
    float z = pointB.z;

    return new Vector3(x, y, z);
  }

  public static Vector3 CalculateMidPoint(Vector3 pointA, Vector3 pointB)
  {
    return Vector3.Lerp(pointA, pointB, 0.5f);
  }


  public static Vector3 ReturnMidPointOfShoulders()
    {
        Vector3 middleOfShoulders = CalculateMidPoint(bodyPoints[11].transform.position, bodyPoints[12].transform.position);
        return middleOfShoulders;
    }  

  public static (float, int) GetBodyPartAngle(BodyParts bodyParts)
  {
    switch (bodyParts)
    {
      case BodyParts.LEFTSHOULDER:
        return LeftShoulderAngle();
      case BodyParts.LEFTARM:
        return LeftArmAngle();
      case BodyParts.LEFTHIPOUT:
        return LeftHipOutAngle();
      case BodyParts.LEFTHIPIN:
        return LeftHipInAngle();
      case BodyParts.LEFTKNEE:
        return LeftKneeAngle();
      case BodyParts.LEFTFOOT:
        return LeftFootAngle();
      case BodyParts.LEFTHEAD:
        return LeftHeadAngle();
      case BodyParts.RIGHTSHOULDER:
        return RightShoulderAngle();
      case BodyParts.RIGHTARM:
        return RightArmAngle();
      case BodyParts.RIGHTHIPOUT:
        return RightHipOutAngle();
      case BodyParts.RIGHTHIPIN:
        return RightHipInAngle();
      case BodyParts.RIGHTKNEE:
        return RightKneeAngle();
      case BodyParts.RIGHTFOOT:
        return RightFootAngle();
      case BodyParts.RIGHTHEAD:
        return RightHeadAngle();
      case BodyParts.MIDDLEHEAD:
        return SideMiddleHeadAngle();
      case BodyParts.MIDDLEHEADLEFTROTATION:
        return MiddleHeadLeftRotation();
      case BodyParts.MIDDLEHEADRIGHTROTATION:
        return MiddleHeadRightRotation();
      case BodyParts.LEFTSHOULDERBACK:
        return LeftShoulderBack();
      case BodyParts.LEFTSHOULDERFRONT:
        return LeftShoulderFront();
      case BodyParts.RIGHTSHOULDERBACK:
        return RightShoulderBack();
      case BodyParts.RIGHTSHOULDERFRONT:
        return RightShoulderFront();
      default:
        return (0, 0);
    }
  }

  public static Transform GetBodyPart(BodyParts bodyParts)
  {
    switch (bodyParts)
    {
      case BodyParts.LEFTSHOULDER:
        return bodyPoints[11].transform;
      case BodyParts.LEFTARM:
        return bodyPoints[13].transform;
      case BodyParts.LEFTHAND:
        return bodyPoints[15].transform;
      case BodyParts.LEFTHIPOUT:
        return null;
      case BodyParts.LEFTHIPIN:
        return null;
      case BodyParts.LEFTKNEE:
        return null;
      case BodyParts.LEFTFOOT:
        return null;
      case BodyParts.LEFTHEAD:
        return null;
      case BodyParts.RIGHTSHOULDER:
        return bodyPoints[12].transform;
      case BodyParts.RIGHTARM:
        return bodyPoints[14].transform;
      case BodyParts.RIGHTHAND:
        return bodyPoints[16].transform;
      case BodyParts.RIGHTHIPOUT:
        return null;
      case BodyParts.RIGHTHIPIN:
        return null;
      case BodyParts.RIGHTKNEE:
        return null;
      case BodyParts.RIGHTFOOT:
        return null;
      case BodyParts.RIGHTHEAD:
        return null;
      case BodyParts.MIDDLEHEAD:
        return null;
      case BodyParts.MIDDLEHEADLEFTROTATION:
        return null;
      case BodyParts.MIDDLEHEADRIGHTROTATION:
        return null;
      case BodyParts.MIDDLEHEADSTAND:
        return null;
      case BodyParts.RIGHTSHOULDERBACK:
        return null;
      case BodyParts.RIGHTSHOULDERFRONT:
        return null;
      case BodyParts.LEFTSHOULDERBACK:
        return null;
      case BodyParts.LEFTSHOULDERFRONT:
        return null;
      default:
        return null;
    }
  }

  public static Vector3 GetBodyPartDirection(BodyParts bodyParts)
  {
    switch (bodyParts)
    {
      case BodyParts.LEFTSHOULDER:
        return (bodyPoints[24].transform.position - bodyPoints[12].transform.position).normalized;
      case BodyParts.RIGHTSHOULDER:
        return (bodyPoints[23].transform.position - bodyPoints[11].transform.position).normalized;
      default:
        return Vector3.zero;
    }
  }

  public static (float, int) LeftShoulderBack()
  {
    Vector3 leftShoulderBack = bodyPoints[11].transform.position - new Vector3(1, 0, 0);
    return (CalculateAngle(leftShoulderBack,
                   bodyPoints[11].transform.position,
                   bodyPoints[13].transform.position), 11);
  }

  public static (float, int) LeftShoulderFront()
  {
    Vector3 leftShoulderBack = bodyPoints[11].transform.position + new Vector3(1, 0, 0);
    return (CalculateAngle(leftShoulderBack,
                   bodyPoints[11].transform.position,
                   bodyPoints[13].transform.position), 11);
  }

  public static (float, int) RightShoulderBack()
  {
    Vector3 rightShoulderBack = bodyPoints[12].transform.position - new Vector3(1, 0, 0);
    return (CalculateAngle(rightShoulderBack,
                   bodyPoints[12].transform.position,
                   bodyPoints[14].transform.position), 12);
  }

  public static (float, int) RightShoulderFront()
  {
    Debug.Log("Shoulder pos : " + bodyPoints[12].transform.position);
    Vector3 rightShoulderBack = bodyPoints[12].transform.position + new Vector3(1, 0, 0);
    Debug.Log("Shoulder Back pos : " + rightShoulderBack);
    return (CalculateAngle(rightShoulderBack,
                   bodyPoints[12].transform.position,
                   bodyPoints[14].transform.position), 12);
  }

  public static (float, int) MiddleHeadStand()
  {
    Vector3 middelOfMouth = CalculateMidPoint(bodyPoints[9].transform.position, bodyPoints[10].transform.position);
    Vector3 middelOfShoulders = CalculateMidPoint(bodyPoints[11].transform.position, bodyPoints[12].transform.position);
    return (CalculateAngle(bodyPoints[0].transform.position,
                   middelOfMouth,
                   middelOfShoulders), 11);
  }

  public static (float, int) MiddleHeadLeftRotation()
  {
    return (CalculateAngle(bodyPoints[2].transform.position,
                   bodyPoints[12].transform.position,
                   bodyPoints[11].transform.position), 12);
  }

  public static (float, int) MiddleHeadRightRotation()
  {
    return (CalculateAngle(bodyPoints[5].transform.position,
                   bodyPoints[11].transform.position,
                   bodyPoints[12].transform.position), 11);
  }

  public static (float, int) SideMiddleHeadAngle()
  {
    Vector3 middlePointOfShoulders = CalculateMidPoint(bodyPoints[11].transform.position, bodyPoints[12].transform.position);
    Vector3 backOfMiddleShoulderPoint = middlePointOfShoulders - new Vector3(1, 0, 0);
    Debug.Log("Shoulder Z : " + middlePointOfShoulders.z);
    return (CalculateAngle(bodyPoints[0].transform.position,
                           backOfMiddleShoulderPoint,
                           new Vector3(middlePointOfShoulders.x, bodyPoints[0].transform.position.y, middlePointOfShoulders.z)), 11);
  }

  public static (float, int) RightShoulderAngle()
  {
    return (CalculateAngle(
        bodyPoints[24].transform.position,
        bodyPoints[12].transform.position,
        bodyPoints[14].transform.position), 12);
  }

  public static (float, int) RightArmAngle()
  {
    return (CalculateAngle(
        bodyPoints[12].transform.position,
        bodyPoints[14].transform.position,
        bodyPoints[16].transform.position), 14);
  }

  public static (float, int) LeftShoulderAngle()
  {
    return (CalculateAngle(
        bodyPoints[23].transform.position,
        bodyPoints[11].transform.position,
        bodyPoints[13].transform.position), 11);
  }

  public static (float, int) LeftArmAngle()
  {
    return (CalculateAngle(
        bodyPoints[11].transform.position,
        bodyPoints[13].transform.position,
        bodyPoints[15].transform.position), 13);
  }

  public static (float, int) RightHipOutAngle()
  {
    return (180f - CalculateAngle(
        bodyPoints[12].transform.position,
        bodyPoints[24].transform.position,
        bodyPoints[26].transform.position), 24);
  }

  public static (float, int) RightHipInAngle()
  {
    return (CalculateAngle(
        bodyPoints[23].transform.position,
        bodyPoints[24].transform.position,
        bodyPoints[26].transform.position), 24);
  }

  public static (float, int) RightKneeAngle()
  {
    return (CalculateAngle(
        bodyPoints[24].transform.position,
        bodyPoints[26].transform.position,
        bodyPoints[28].transform.position), 26);
  }

  public static (float, int) RightFootAngle()
  {
    return (CalculateAngle(
        bodyPoints[26].transform.position,
        bodyPoints[28].transform.position,
        bodyPoints[32].transform.position), 28);
  }

  public static (float, int) RightHeadAngle()
  {
    return (CalculateAngle(
        bodyPoints[0].transform.position,
        CalculateMidPoint(bodyPoints[11].transform.position, bodyPoints[12].transform.position),
        bodyPoints[12].transform.position), 12);
  }

  public static (float, int) LeftHipOutAngle()
  {
    return (180f - CalculateAngle(
        bodyPoints[11].transform.position,
        bodyPoints[23].transform.position,
        bodyPoints[25].transform.position), 23);
  }

  public static (float, int) LeftHipInAngle()
  {
    return (CalculateAngle(
        bodyPoints[24].transform.position,
        bodyPoints[23].transform.position,
        bodyPoints[25].transform.position), 23);
  }

  public static (float, int) LeftKneeAngle()
  {
    return (CalculateAngle(
        bodyPoints[23].transform.position,
        bodyPoints[25].transform.position,
        bodyPoints[27].transform.position), 25);
  }

  public static (float, int) LeftFootAngle()
  {
    return (CalculateAngle(
        bodyPoints[25].transform.position,
        bodyPoints[27].transform.position,
        bodyPoints[31].transform.position), 27);
  }

  public static (float, int) LeftHeadAngle()
  {
    return (CalculateAngle(
        bodyPoints[0].transform.position,
        CalculateMidPoint(bodyPoints[11].transform.position, bodyPoints[12].transform.position),
        bodyPoints[11].transform.position), 11);
  }


}

public enum BodyParts
{
  LEFTSHOULDER,
  LEFTARM,
  LEFTHIPOUT,
  LEFTHIPIN,
  LEFTKNEE,
  LEFTFOOT,
  LEFTHEAD,

  RIGHTSHOULDER,
  RIGHTARM,
  RIGHTHIPOUT,
  RIGHTHIPIN,
  RIGHTKNEE,
  RIGHTFOOT,
  RIGHTHEAD,

  MIDDLEHEAD,
  MIDDLEHEADLEFTROTATION,
  MIDDLEHEADRIGHTROTATION,
  MIDDLEHEADSTAND,

  RIGHTSHOULDERBACK,
  RIGHTSHOULDERFRONT,

  LEFTSHOULDERBACK,
  LEFTSHOULDERFRONT,

  LEFTHAND,
  RIGHTHAND
}