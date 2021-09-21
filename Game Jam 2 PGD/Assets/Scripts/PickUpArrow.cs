using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUpArrow : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        PickUpManager manager = collision.GetComponent<PickUpManager>();
        
        if(manager)
        {
            if (manager.arrowCount < manager.maxArrows)
            {
                manager.PickUpItem();
                Destroy(gameObject);
            }
        }     
    }
}
