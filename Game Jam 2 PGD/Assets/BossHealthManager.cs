using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossHealthManager : MonoBehaviour
{
    [SerializeField] private int health;

    // Start is called before the first frame update
    void Start()
    {
        health = 5;
    }

    // Update is called once per frame
    void Update()
    {
        Die();

        if (Input.GetKeyDown(KeyCode.P)) 
        {
            GetHit();
        }
    }

    public void GetHit() 
    {
        health -= 1;
    }

    private void Die() 
    {
        if (health <= 0) 
        {
            Destroy(gameObject);
        }
    }
}
