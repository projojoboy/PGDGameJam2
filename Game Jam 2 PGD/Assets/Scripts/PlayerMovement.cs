using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float _jumpForce = 200;
    [SerializeField] private float _movementSpeed = 5;
    [SerializeField] private float _maxSpeed = 10;

    [SerializeField] private LayerMask _whatIsGround;
    [SerializeField] private Transform _groundCheck;

    private Rigidbody2D rb;

    private bool _isGrounded = false;

    const float _groundedRadius = .2f;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        
    }

    private void FixedUpdate()
    {
        Jumping();
        Movement();
        IsGrounded();
    }

    private void IsGrounded()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(_groundCheck.position, _groundedRadius, _whatIsGround);
        for (int i = 0; i < colliders.Length; i++)
        {
            if (colliders[i].gameObject != gameObject)
            {
                _isGrounded = true;
            }
        }
    }

    private void Jumping()
    {
        if (_isGrounded && Input.GetKeyDown(KeyCode.Space))
        {
            Debug.Log("heya");
            rb.AddForce(new Vector2(0, _jumpForce));
            _isGrounded = false;
        }
    }

    private void Movement()
    {
        rb.velocity = new Vector2(Mathf.Clamp(rb.velocity.x, -_maxSpeed, _maxSpeed), rb.velocity.y);

        float h = Input.GetAxis("Horizontal");

        rb.AddForce(new Vector2(h * _movementSpeed, 0));

        rb.velocity = new Vector2(rb.velocity.x * .8f, rb.velocity.y);

        if (rb.velocity.magnitude < .5f && h == 0)
            rb.velocity = new Vector2(0, rb.velocity.y);
    }
}
