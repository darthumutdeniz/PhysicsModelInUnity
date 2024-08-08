using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UniversalConstants : MonoBehaviour
{
    [SerializeField] Vector2 gravitationalAcceleration = new Vector3(0, -10, 0);

    public Vector3 GetSmallG()
    {
        return gravitationalAcceleration;
    }
}
