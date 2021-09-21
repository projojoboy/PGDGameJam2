using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowMovement : MonoBehaviour
{
    public bool _isFlying = false;
    public bool _isSticky = true;

    [SerializeField] private int _speed;

    private float lifeTime;

    private void Awake()
    {
        _isFlying = true;
    }

    // Update is called once per frame
    void Update()
    {
        lifeTime += Time.deltaTime;

        if (_isFlying) 
        {
            transform.position += transform.right * _speed * Time.deltaTime;
        }

        if (lifeTime > 10) 
        {
            Destroy(gameObject);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Boss")
        {
            Debug.Log("dsad");
            collision.gameObject.GetComponent<BossHealthManager>().GetHit();
            Destroy(this.gameObject);
        }

        if (collision.gameObject.tag == "Ground")
        {
            if (_isSticky)
            {
                _isFlying = false;
                GetComponent<Collider2D>().isTrigger = true;
            }
            else
                Destroy(this.gameObject);
        }

        if (collision.gameObject.tag == "Player")
        {
            collision.gameObject.GetComponent<HealthController>().LoseHP(1);
            Destroy(this.gameObject);
        }
    }
}
