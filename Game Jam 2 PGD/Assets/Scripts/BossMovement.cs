using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossMovement : MonoBehaviour
{
    [SerializeField] private int speed;

    [SerializeField] private bool left;


    // Start is called before the first frame update
    void Start()
    {
        left = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (transform.position.x < -18)
        {
            left = false;
            Debug.Log("Right");
        }
        else if (transform.position.x > 3) 
        {
            left = true;
            Debug.Log("Left");
        }

        if (left)
        {
            transform.position -= transform.right * speed * Time.deltaTime;
        }
        else if (!left)
        {
            transform.position += transform.right * speed * Time.deltaTime;
        }
    }
}
