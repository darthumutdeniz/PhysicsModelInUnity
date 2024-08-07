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
        float dx = otherPosition.x - thisPosition.x;
        float dy = otherPosition.y - thisPosition.y;
        float forceMagnitude = (thisMass * otherMass)/(Mathf.Pow(dx,2)+Mathf.Pow(dy,2));
        float angle = Mathf.Atan2(dy,dx);
        Vector2 NewForce = new Vector2(forceMagnitude * Mathf.Cos(angle), forceMagnitude * Mathf.Sin(angle));
        pOs.ChangeForce(NewForce);

    }
}
