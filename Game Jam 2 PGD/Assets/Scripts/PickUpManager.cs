using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUpManager : MonoBehaviour
{
    public int arrowCount;

    public int maxArrows = 5;

    public void PickUpItem()
    {
        if(arrowCount < maxArrows)
            arrowCount++;
    }

}
