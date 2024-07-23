using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stick : MonoBehaviour
{

    [SerializeField] GameObject object1;
    [SerializeField] GameObject object2;
    [Header("0 for none, 1 for the first, 2 for the second")]
    [Range(0,2)] [SerializeField] int constantIndex;
    [SerializeField] Vector3 forceOfTension;
    float widht;
    float rotationInRadians;
    float dx = 0;
    float dy = 0;
    float gama = 0; //the angle that is the ratio of the differences of axies static - kinetic

    GameObject mainObject;
    GameObject otherObject;
    bool hasPutThemInPlace = false;


    void Start()
    {
        widht = transform.lossyScale.x;
        if (constantIndex == 1)
        {
            mainObject = object1;
            otherObject = object2;
        }
        else if(constantIndex == 2)
        {
            mainObject = object2;
            otherObject = object1;
        }
        else
        {
            mainObject = object1;
            otherObject = object2;
        }
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
        if(constantIndex != 0){
            SetTheStartingVelocity();
        }
    }

    void SetTheStartingVelocity(){
        PhysicsObject mainPO = mainObject.GetComponent<PhysicsObject>();
        PhysicsObject otherPO = otherObject.GetComponent<PhysicsObject>();
        mainPO.AddForce(1,0,0);
        /*float angle = Mathf.Atan2(dy, dx);
        Debug.Log("Rotation of Stick (in radians)" + angle);
       
        otherObject.GetComponent<PhysicsObject>().velocity = new Vector3(0,0,0);
        Debug.Log("Velocity of the static" + otherObject.GetComponent<PhysicsObject>().velocity);
        Vector2 mainVelocity = mainPO.GetVelocity();
        float ratioOfVelocity = Mathf.Atan2(mainVelocity.y, mainVelocity.x);
        float magnitudeOfNewSpeed = mainVelocity.magnitude* Mathf.Cos(Mathf.PI/2 -angle -ratioOfVelocity);
        Vector3 newVelocity = new Vector3(magnitudeOfNewSpeed* Mathf.Sin(angle), magnitudeOfNewSpeed* Mathf.Cos(angle), 0);
        Debug.Log("new velocity" + newVelocity);
        mainPO.velocity = newVelocity;
        Debug.Log("kinetic ones velocity" + mainPO.velocity);*/
        
    }

    void Update()
    {
        dx = mainObject.transform.position.x - otherObject.transform.position.y;
        dy = mainObject.transform.position.y - otherObject.transform.position.y;
        if(constantIndex == 0)
        {}
        else{
            OneOfThemIsStabilized();
        }
    }
    void OneOfThemIsStabilized()
    {
        float angle = Mathf.Atan2(dx,-dy);
        PhysicsObject mainPO = mainObject.GetComponent<PhysicsObject>();
        Vector2 mainVelocity = mainPO.GetVelocity();
    }
}
