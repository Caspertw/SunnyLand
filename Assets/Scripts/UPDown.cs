using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UPDown : MonoBehaviour
{
    public Transform upPoint, downPoint;
    private Rigidbody2D rb;
    public float speed;
    private float upY, downY;
    private bool faceUp = true;
    


    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        upY = upPoint.position.y;
        downY = downPoint.position.y;
        Destroy(upPoint.gameObject);
        Destroy(downPoint.gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        Movement();
    }
    void Movement()
    {
        if (faceUp)
        {
            rb.velocity = new Vector2(rb.velocity.x, speed);
            if (transform.position.y > upY)
            {
                faceUp = false;
            }
        }
        else
        {
            rb.velocity = new Vector2(rb.velocity.x, -speed);
            if (transform.position.y < downY)
            {
                faceUp = true;
            }
        }
    }
}
