using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class PhysicsObject : MonoBehaviour
{
    [Header("Input Values")]
    [SerializeField] float mass = 1;
    [SerializeField] float gravityScale = 1;
    [SerializeField] Vector3 v0;
    public List<Vector3> forces = new List<Vector3>();
    Vector3 netForce;
    Vector3 gravityAcceleration = new Vector3(0,-10,0);

    [Header("ValueReader")]
    [SerializeField] Vector3 velocityReader;
    public Vector3 velocity;
    [SerializeField] Vector3 accelerationReader;
    Vector3 acceleration;

    [Header("Energy Values")]
    [SerializeField] float groundLevel = 0;
    [SerializeField] float kitenicEnergy;
    [SerializeField] float gravityPotentialEnergy;
    [SerializeField] float totalEnergy;
    
    bool isChangingVelocity = false;
    

    void Start()
    {
        velocity = v0;
    }

    void Update()
    {
        CaculateMotion();
        CaculateEnergy();
    }

    void  OnDisable()
    {
        forces = new List<Vector3>();
    }

    void CaculateMotion()
    {
        if (isChangingVelocity) {return;}
        netForce = CaculateNetForce();
        acceleration = gravityAcceleration * gravityScale + (netForce / mass);
        accelerationReader = acceleration;
        velocity += acceleration * Time.deltaTime;
        velocityReader = velocity;
        transform.position += velocity * Time.deltaTime;
    }

    Vector3 CaculateNetForce()
    {
        Vector3 netForce =  Vector3.zero;
        foreach(Vector3 force in forces)
        {
            netForce += force;
        }
        netForce -= new Vector3(0,0,netForce.z);
        return netForce;
    }

    public float GetMass()
    {
        return mass;
    }

    public Vector3 GetVelocity()
    {
        return velocity;
    }

    public void SetVelocity(Vector3 newVelocity)
    {
        acceleration = Vector3.zero;
        isChangingVelocity = true;
        velocity = newVelocity;
        isChangingVelocity = false;
    }

    void CaculateEnergy()
    {
        kitenicEnergy = mass * Mathf.Pow(velocity.magnitude, 2)/2;
        gravityPotentialEnergy = mass * gravityScale * gravityAcceleration.magnitude * (transform.position.y - groundLevel);
        totalEnergy = kitenicEnergy + gravityPotentialEnergy;
    }

    public void AddForce(float x,float y, float z)
    {
        forces.Add(new Vector3(x,y,z));
    }
    public void ChangeForce(Vector3 force)
    {
        bool hasFoundIt = false;
        for(int i =0; i < forces.Count; i++)
        {
            if(forces[i].z == force.z)
            {
                forces[i] = force;
                hasFoundIt = true;
            }
        }
        if(!hasFoundIt)
        {
            AddForce(force.x,force.y,force.z);
        }
    }
}
