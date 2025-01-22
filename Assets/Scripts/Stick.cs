using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stick : MonoBehaviour
{

    [SerializeField] GameObject object1;
    [SerializeField] GameObject object2;
    [Header("Moving Object's index 0 for none, 1 for the first, 2 for the second")]
    [Range(0,2)] [SerializeField] int constantIndex;
    [SerializeField] Vector3 forceOfTension;
    float widht;
    float rotationInRadians;
    float dx = 0;
    float dy = 0;
    double angle;
    float distance;
    GameObject movingObject;
    GameObject nonMovingObject;
    PhysicsObject movingPO;
    PhysicsObject nonMovingPO;


    void Start()
    {
        widht = transform.localScale.x;
        if (constantIndex == 1)
        {
            movingObject = object1;
            nonMovingObject = object2;
        }
        else if(constantIndex == 2)
        {
            movingObject = object2;
            nonMovingObject = object1;
        }
        else
        {
            movingObject = object1;
            nonMovingObject = object2;
        }
        movingPO = movingObject.GetComponent<PhysicsObject>();
        nonMovingPO = nonMovingObject.GetComponent<PhysicsObject>();
        PutThemInPlace();
        distance = Mathf.Sqrt(Mathf.Pow(dx,2) + Mathf.Pow(dy,2));
    }

    void PutThemInPlace()
    {
        rotationInRadians = transform.localEulerAngles.z * Mathf.Deg2Rad;
        Debug.Log(transform.localEulerAngles.z);
        float lenghtInY = widht * Mathf.Sin(rotationInRadians);
        float lenghtInX = widht * Mathf.Cos(rotationInRadians);
        float x1 = transform.position.x - lenghtInX/2;
        float y1 = transform.position.y - lenghtInY/2;
        float x2 = transform.position.x + lenghtInX/2;
        float y2 = transform.position.y + lenghtInY/2;
        object1.transform.position = new Vector3(x1,y1,0);
        object2.transform.position = new Vector3(x2,y2,0);
        SetTheStartingVelocity();
    }

    void SetTheStartingVelocity(){
        nonMovingPO.SetVelocity(Vector3.zero);
        CalculateDistance();
        angle = Math.Atan2(dy, dx);
        Vector2 v0 = movingPO.GetVelocity();
        Debug.Log("Rotation of Stick (in radians) " + angle);
        Debug.Log(nonMovingPO.GetVelocity() + " main velocity");
        Vector2 vY = v0.y * (float) Math.Cos(angle) * new Vector2((float) -Math.Sin(angle), (float) Math.Cos(angle));
        Vector2 vX = v0.x * (float) Math.Sin(angle) * new Vector2((float) Math.Sin(angle), -(float) Math.Cos(angle));
        movingPO.SetVelocity(vY + vX);
    }

    void CalculateDistance()
    {
        dx = object2.transform.position.x - object1.transform.position.x;
        dy = object2.transform.position.y - object1.transform.position.y;
    }

    void FixedUpdate()
    {
        CalculateDistance();
        float x = dx/2 + object1.transform.position.x;
        float y = dy/2 + object1.transform.position.y;

        if(constantIndex == 0)
        {}
        else{
            OneOfThemIsStabilized();
        }
        distance = Mathf.Sqrt(Mathf.Pow(dx,2) + Mathf.Pow(dy,2));
        transform.position = new Vector3(x, y, 0);
        transform.eulerAngles = new Vector3(0, 0, Mathf.Atan2(dy,dx) * Mathf.Rad2Deg);
    }
    void OneOfThemIsStabilized()
    {
        float thetaAngle = -Mathf.Atan2(dx, dy);
        Debug.Log(thetaAngle * Mathf.Rad2Deg);
        Vector2 mainVelocity = movingPO.GetVelocity();
        float centriputalMagnitude = movingPO.GetMass() * Mathf.Pow(mainVelocity.magnitude, 2) / distance;
        Debug.Log(centriputalMagnitude);
        float gravitationalMagnitude = movingPO.GetMass() *
         FindAnyObjectByType<UniversalConstants>().GetSmallG().magnitude * movingPO.GetGravityScale() * (float) Math.Cos(thetaAngle);
        Debug.Log(gravitationalMagnitude);
        float magnitude = centriputalMagnitude + gravitationalMagnitude;
        Debug.Log(magnitude + " x eksen " + -magnitude* (float) Math.Sin(thetaAngle)
         + " y eksen " +  magnitude * (float) Math.Cos(thetaAngle));
        forceOfTension = new Vector3(-magnitude * Mathf.Sin(thetaAngle), magnitude * Mathf.Cos(thetaAngle), 156);
        Debug.Log(distance);
        movingPO.ChangeForce(forceOfTension);
    }
}
