using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Rigidbody2D))]
public class CharacterController2D : MonoBehaviour
{
	[SerializeField] private float _jumpForce = 400f;
	[SerializeField] private float _moveSpeed = 40f;
	[SerializeField] private float _slideForce = 200f;
	[SerializeField] private float _slideVelocityThreshold = 1.3f;

	[Range(0, .3f)] [SerializeField] private float _movementSmoothing = .05f;

	[SerializeField] private LayerMask _whatIsGround;

	[SerializeField] private Transform _groundCheck;

	[SerializeField] private Collider2D _slideDisableCollider;

	const float _groundedRadius = .2f;
	
	private Rigidbody2D _rb;
	
	private Vector3 _velocity = Vector3.zero;

	private bool _facingRight = true;
	private bool slide = false;

	public float horizontalMove = 0;

	public bool jump = false;
	public bool grounded = false;
	public bool sliding = false;

	[Header("Events")]
	[Space]

	public UnityEvent OnLandEvent;

	[System.Serializable]
	public class BoolEvent : UnityEvent<bool> { }

	public BoolEvent OnCrouchEvent;

	private void Awake()
	{
		_rb = GetComponent<Rigidbody2D>();

		if (OnLandEvent == null)
			OnLandEvent = new UnityEvent();

		if (OnCrouchEvent == null)
			OnCrouchEvent = new BoolEvent();
	}

	private void Update()
	{
		horizontalMove = Input.GetAxis("Horizontal") * _moveSpeed;

		if (Input.GetKeyDown(KeyCode.Space))
			jump = true;

		if (Input.GetKeyDown(KeyCode.LeftShift))
			slide = true;
	}

    private void FixedUpdate()
    {
		GroundCheck();
		Move(horizontalMove * Time.fixedDeltaTime, slide, jump);
		jump = false;
		slide = false;
	}

    private void GroundCheck()
    {
		bool wasGrounded = grounded;
		grounded = false;

		Collider2D[] colliders = Physics2D.OverlapCircleAll(_groundCheck.position, _groundedRadius, _whatIsGround);
		for (int i = 0; i < colliders.Length; i++)
		{
			if (colliders[i].gameObject != gameObject)
			{
				grounded = true;
				if (!wasGrounded)
					OnLandEvent.Invoke();
			}
		}
	}

	private void Move(float move, bool slide, bool jump)
	{
        if (!sliding)
        {
			if(slide)
            {
				sliding = true;
				_rb.velocity = Vector2.zero;
				_rb.AddForce(new Vector2(_slideForce * transform.localScale.x, 0));
				_slideDisableCollider.enabled = false;
			}
        }
		else if (sliding && _rb.velocity.magnitude < _slideVelocityThreshold)
        {
			sliding = false;
			_slideDisableCollider.enabled = true;
		}

        if (sliding)
			_rb.velocity = new Vector2(_rb.velocity.x * .95f, _rb.velocity.y);

		if (sliding)
			return;

		Vector3 targetVelocity = new Vector2(move * 10f, _rb.velocity.y);
		_rb.velocity = Vector3.SmoothDamp(_rb.velocity, targetVelocity, ref _velocity, _movementSmoothing);

		if (move > 0 && !_facingRight)
			Flip();
		else if (move < 0 && _facingRight)
			Flip();

		if (grounded && jump)
		{
			grounded = false;
			_rb.AddForce(new Vector2(0f, _jumpForce));
		}
	}

	private void Flip()
	{
		_facingRight = !_facingRight;

		Vector3 theScale = transform.localScale;
		theScale.x *= -1;
		transform.localScale = theScale;
	}
}
