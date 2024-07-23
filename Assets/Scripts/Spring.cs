using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.UIElements;

public class Spring : MonoBehaviour
{

    [SerializeField] float springConstant;
    [SerializeField] GameObject object1;
    [SerializeField] GameObject object2;
    [SerializeField] float lenght0;
    [Header("0 for none, 1 for the first, 2 for the second")]
    [Range(0,2)] [SerializeField] int constantIndex;
    public bool hasStarted = false;
    float strartingLenght;
    float rotationInRadians;
    float dx = 0;
    float dy = 0;
    GameObject mainObject;
    bool hasPutThemInPlace = false;
    

    // Start is called before the first frame update
    void Start()
    {
        strartingLenght = transform.lossyScale.x;
        switch (constantIndex){
            case 1:
                mainObject = object1;
                break;
            case 2:
                mainObject = object2;
                break;
            default:
                mainObject = object1;
                break;
        }
        PutThemInPlace();
    }

    void PutThemInPlace()
    {
        rotationInRadians = transform.localEulerAngles.z * Mathf.Deg2Rad;
        float lenghtInY = strartingLenght * Mathf.Sin(rotationInRadians);
        float lenghtInX = strartingLenght * Mathf.Cos(rotationInRadians);
        float x1 = transform.position.x - lenghtInX/2;
        float y1 = transform.position.y - lenghtInY/2;
        float x2 = transform.position.x + lenghtInX/2;
        float y2 = transform.position.y + lenghtInY/2;
        object1.transform.position = new Vector3(x1,y1,0);
        object2.transform.position = new Vector3(x2,y2,0);
        hasPutThemInPlace = true;
    }
 

    // Update is called once per frame
    void Update()
    {
        if(!hasStarted) {return;}
        rotationInRadians = transform.localEulerAngles.z * Mathf.Deg2Rad;
        ChangeLenght();
        CaculateTheForce();
    }

    void ChangeLenght()
    {
        if(!hasPutThemInPlace) {return;}
        float newX = CalculateXPosition();
        float newY = CalculateYPosition();
        float newLenght = Mathf.Sqrt(Mathf.Pow(dx,2) + Mathf.Pow(dy,2));
        float angle = Mathf.Atan2(dy,dx) * Mathf.Rad2Deg;
        transform.position = new Vector3(newX, newY, 0);
        transform.localScale = new Vector3(newLenght, transform.localScale.y, transform.localScale.z);
        transform.eulerAngles = new Vector3(0, 0, angle);
    }

    float CalculateXPosition()
    {
        dx = object2.transform.position.x - object1.transform.position.x;
        return dx/2 + object1.transform.position.x;
    }
    float CalculateYPosition()
    {
        dy = object2.transform.position.y - object1.transform.position.y;
        return dy/2 + object1.transform.position.y;
    }


    private void CaculateTheForce()
    {
        if (constantIndex == 0){
            BothAreKinetic();
        }
        else{
            OneIsStabilized();
        }
        
    }

    private void BothAreKinetic()
    {
        rotationInRadians = transform.localEulerAngles.z * Mathf.Deg2Rad;
        float instanteniousLenght = transform.localScale.x;
        float displacement = instanteniousLenght - lenght0;
        float magnitudeForce = springConstant * displacement;
        float angle = Mathf.Atan2(dy, dx);
        object1.GetComponent<PhysicsObject>().
        ChangeForce(new Vector3(magnitudeForce * Mathf.Cos(angle),
                magnitudeForce * Mathf.Sin(angle), 143));
        object2.GetComponent<PhysicsObject>().
        ChangeForce(new Vector3(- magnitudeForce * Mathf.Cos(angle),
                -magnitudeForce * Mathf.Sin(angle), -143));
    }

    private void OneIsStabilized()
    {
        rotationInRadians = transform.localEulerAngles.z * Mathf.Deg2Rad;
        float instanteniousLenght = transform.localScale.x;
        float displacement = instanteniousLenght - lenght0;
        float magnitudeForce = springConstant * displacement;
        float angle = Mathf.Atan2(dy, dx);
        mainObject.GetComponent<PhysicsObject>().
        ChangeForce(new Vector3(magnitudeForce * Mathf.Cos(angle),
                magnitudeForce * Mathf.Sin(angle), 143));
    }
}
