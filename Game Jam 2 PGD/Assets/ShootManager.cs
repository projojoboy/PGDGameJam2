using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootManager : MonoBehaviour
{
    [SerializeField] private Transform[] cannon = new Transform[8];
    [SerializeField] private GameObject arrowPrefab;

    [SerializeField] private float cooldown = 5;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        cooldown -= Time.deltaTime;
        if (cooldown < 0)
        {
            for (int i = 0; i < cannon.Length; i++) 
            {
                Instantiate(arrowPrefab, cannon[i].transform.position, cannon[i].rotation);
            }

            cooldown = 5;
        }
    }
}
