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
    [SerializeField] Vector3 springForce;
    Vector3 positiondifference;
    public bool hasStarted = false;
    float startingLenght;
    float rotationInRadians;
    float instanteniousLenght;
    float displacement;
    float magnitudeForce;
    //for expr. puposes only delete later
    [SerializeReference] float maxForce;
    GameObject mainObject;
    bool hasPutThemInPlace = false;
    

    // Start is called before the first frame update
    void Start()
    {
        startingLenght = transform.lossyScale.x;
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
        Vector3 startingDiffVector = startingLenght * new Vector3(Mathf.Sin(rotationInRadians), Mathf.Cos(rotationInRadians), 0);
        object1.transform.position = transform.position - startingDiffVector/2;
        object2.transform.position = transform.position + startingDiffVector/2;
        hasPutThemInPlace = true;
    }
 

    // Update is called once per frame
    void Update()
    {
        if(!hasStarted) {return;}
        positiondifference = object2.transform.position - object1.transform.position;
        ChangeLenght();
        CaculateTheForce();
    }

    void ChangeLenght()
    {
        if(!hasPutThemInPlace) {return;}
        float newLenght = positiondifference.magnitude;
        float angle = Mathf.Atan2(positiondifference.y, positiondifference.x) * Mathf.Rad2Deg;
        transform.position = object1.transform.position + positiondifference/2;
        transform.localScale = new Vector3(newLenght, transform.localScale.y, transform.localScale.z);
        transform.eulerAngles = new Vector3(0, 0, angle);
    }


    private void CaculateTheForce()
    {
        instanteniousLenght = transform.localScale.x;
        displacement = instanteniousLenght - lenght0;
        magnitudeForce = springConstant * displacement;
        springForce = magnitudeForce * positiondifference.normalized;
        springForce.z = 143;
        if(magnitudeForce > maxForce) maxForce = magnitudeForce;
        if (constantIndex == 0){
            BothAreKinetic();
        }
        else{
            OneIsStabilized();
        }
        
    }

    private void BothAreKinetic()
    {
        object1.GetComponent<PhysicsObject>().
        ChangeForce(springForce);
        object2.GetComponent<PhysicsObject>().
        ChangeForce(springForce);
    }

    private void OneIsStabilized()
    {      
        mainObject.GetComponent<PhysicsObject>().
        ChangeForce(springForce);
    }
}
