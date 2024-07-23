using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pendulum : MonoBehaviour
{


    [SerializeField] float startingAngle = 0;
    [SerializeField] float startingAngularSpeed = 0;
    float angle = 0; 
    float angularSpeed = 0;
    float angularAcceleration = 0;
    float lenght = 1;

    // Start is called before the first frame update
    void Start()
    {
        angle = startingAngle * Mathf.Deg2Rad;
        angularSpeed = startingAngularSpeed;
        transform.eulerAngles = new Vector3(0, 0,-90 + startingAngle);
        lenght = transform.lossyScale.x;
    }

    // Update is called once per frame
    void Update()
    {
        angularAcceleration = -10/lenght * Mathf.Sin(angle);
        angularSpeed += angularAcceleration * Time.deltaTime;
        angle += angularSpeed * Time.deltaTime;
        float rotation = -90 + angle * Mathf.Rad2Deg;
        transform.eulerAngles = new Vector3(0, 0, rotation);
    }
}
