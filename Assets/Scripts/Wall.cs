using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wall : MonoBehaviour
{
    [SerializeField] GameObject collusionCounter;

    void OnTriggerEnter2D(Collider2D other) {
        other.GetComponent<Rigidbody2D>().velocity *= Vector2.left;
        collusionCounter.GetComponent<Objects>().collusionCount++;
    }
}
