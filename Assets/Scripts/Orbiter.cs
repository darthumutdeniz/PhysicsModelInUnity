using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Orbiter : MonoBehaviour
{
    [SerializeField] GameObject square;
    PhysicsObject pOs;

    // Start is called before the first frame update
    void Start()
    {
        pOs = square.GetComponent<PhysicsObject>();
        pOs.AddForce(0, 0, 451);
    }

    // Update is called once per frame
    void Update()
    {
        float thisMass = GetComponent<PhysicsObject>().GetMass();
        float otherMass = square.GetComponent<PhysicsObject>().GetMass();
        Vector3 thisPosition = transform.position;
        Vector3 otherPosition = square.transform.position;
        Vector3 positionDifference = otherPosition - thisPosition;
        Vector3 force = -thisMass * otherMass * positionDifference.normalized /positionDifference.sqrMagnitude;
        force.z = 321;
        pOs.ChangeForce(force);

    }
}
