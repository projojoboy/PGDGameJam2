using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowMovement : MonoBehaviour
{
    public bool _isFlying = false;

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
}
