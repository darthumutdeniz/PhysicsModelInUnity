using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Objects : MonoBehaviour
{

    [SerializeField] GameObject otherOne;
    [SerializeField] float velocity;
    public int collusionCount = 0;
    float time =  0;

    Rigidbody2D mainrb2d;
    Rigidbody2D otherrb2d;
    

    void Start()
    {
        mainrb2d = GetComponent<Rigidbody2D>();
        otherrb2d = otherOne.GetComponent<Rigidbody2D>();
        mainrb2d.velocity = new Vector2 (velocity,0);

    }

    void Update()
    {
        time += Time.deltaTime;
    }

    void OnCollisionEnter2D(Collision2D other) 
    {
        collusionCount++;
        if(other.gameObject.tag != "Kinetics") {return;}
        float u1 = mainrb2d.velocity.x;
        float u2 = otherrb2d.velocity.x;
        otherrb2d.velocity = Vector2.zero;
        mainrb2d.velocity = Vector2.zero;
        float d = Mathf.Abs(otherOne.transform.position.x - transform.position.x );
        if (d < 1)
        {
            if(transform.position.x > otherOne.transform.position.x)
            {
                transform.position += new Vector3(0.5f-d,0,0);
            }
            else if(transform.position.x < otherOne.transform.position.x)
            {
                otherOne.transform.position += new Vector3(0.5f-d,0,0);
            }
        }
        float v0 = 0;
        float m1 = 0;
        float v1;
        float m2 = 0;
        float v2;
        GameObject go1 = gameObject;
        GameObject go2 = gameObject;
        if(u1 > u2)
        {
            v0 = u1 - u2;
            m1 = mainrb2d.mass;
            m2 = otherrb2d.mass;
            go1 = gameObject;
            go2 = otherOne;
            
        }
        else if(u1 < u2)
        {
            v0 = u2-u1;
            m2 = mainrb2d.mass;
            m1 = otherrb2d.mass;
            go2 = gameObject;
            go1 = otherOne;
        }
        v2 = v0 * (2*m1/(m1+m2));
        v1 = v2 - v0;
        go1.GetComponent<Rigidbody2D>().velocity = new Vector2(v1+u2,0);
        go2.GetComponent<Rigidbody2D>().velocity = new Vector2(v2+u2,0);
        
    }
}
