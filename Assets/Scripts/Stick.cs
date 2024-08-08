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
    float gama = 0; //the angle that is the ratio of the differences of axies static - kinetic
    double angle;

    GameObject movingObject;
    GameObject nonMovingObject;
    PhysicsObject movingPO;
    PhysicsObject nonMovingPO;
    bool hasPutThemInPlace = false;


    void Start()
    {
        widht = transform.lossyScale.x;
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

    void Update()
    {
        CalculateDistance();
        float x = dx/2 + object1.transform.position.x;
        float y = dy/2 + object1.transform.position.y;
        transform.position = new Vector3(x, y, 0);
        transform.eulerAngles = new Vector3(0, 0, Mathf.Atan2(dy,dx) * Mathf.Rad2Deg);

        if(constantIndex == 0)
        {}
        else{
            OneOfThemIsStabilized();
        }
    }
    void OneOfThemIsStabilized()
    {
        float thetaAngle =Mathf.Atan2(dx, dy);
        Debug.Log(thetaAngle);
        Vector2 mainVelocity = movingPO.GetVelocity();
        float centriputalMagnitude = movingPO.GetMass() * mainVelocity.magnitude * mainVelocity.magnitude / widht;
        float gravitationalMagnitude = movingPO.GetMass() *
         FindAnyObjectByType<UniversalConstants>().GetSmallG().magnitude * (float) Math.Cos(thetaAngle);
        float magnitude = centriputalMagnitude + gravitationalMagnitude;
        forceOfTension = new Vector3( magnitude* (float) Math.Sin(thetaAngle), magnitude * (float) Math.Cos(thetaAngle), 156);
        movingPO.ChangeForce(forceOfTension);
    }
}
