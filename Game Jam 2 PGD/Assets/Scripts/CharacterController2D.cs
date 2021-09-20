using UnityEngine;
using UnityEngine.Events;

public class CharacterController2D : MonoBehaviour
{
	[SerializeField] private float _jumpForce = 400f;
	[SerializeField] private float _moveSpeed = 40f;

	[Range(0, 1)]	[SerializeField] private float _crouchSpeed = .36f;
	[Range(0, .3f)] [SerializeField] private float _movementSmoothing = .05f;

	[SerializeField] private LayerMask _whatIsGround;

	[SerializeField] private Transform _groundCheck;
	[SerializeField] private Transform _ceilingCheck;

	[SerializeField] private Collider2D _crouchDisableCollider;

	const float _groundedRadius = .2f;
	const float _ceilingRadius = .2f;
	
	private Rigidbody2D _rb;
	
	private Vector3 _velocity = Vector3.zero;

	private float _horizontalMove = 0;

	private bool _wasCrouching = false;
	private bool _jump = false;
	private bool _crouch = false;
	private bool _grounded = false;
	private bool _facingRight = true;

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
		_horizontalMove = Input.GetAxis("Horizontal") * _moveSpeed;

		if (Input.GetKeyDown(KeyCode.Space))
			_jump = true;

		if (Input.GetKeyDown(KeyCode.LeftControl))
			_crouch = !_crouch;
	}

    private void FixedUpdate()
    {
		GroundCheck();
		Move(_horizontalMove * Time.fixedDeltaTime, _crouch, _jump);
		_jump = false;
	}

    private void GroundCheck()
    {
		bool wasGrounded = _grounded;
		_grounded = false;

		Collider2D[] colliders = Physics2D.OverlapCircleAll(_groundCheck.position, _groundedRadius, _whatIsGround);
		for (int i = 0; i < colliders.Length; i++)
		{
			if (colliders[i].gameObject != gameObject)
			{
				_grounded = true;
				if (!wasGrounded)
					OnLandEvent.Invoke();
			}
		}
	}

	private void Move(float move, bool crouch, bool jump)
	{
		if (!crouch)
		{
			if (Physics2D.OverlapCircle(_ceilingCheck.position, _ceilingRadius, _whatIsGround))
				crouch = true;
		}

		if (crouch)
		{
			if (!_wasCrouching)
			{
				_wasCrouching = true;
				OnCrouchEvent.Invoke(true);
			}

			move *= _crouchSpeed;

			if (_crouchDisableCollider != null)
				_crouchDisableCollider.enabled = false;
		}
		else
		{
			if (_crouchDisableCollider != null)
				_crouchDisableCollider.enabled = true;

			if (_wasCrouching)
			{
				_wasCrouching = false;
				OnCrouchEvent.Invoke(false);
			}
		}

		Vector3 targetVelocity = new Vector2(move * 10f, _rb.velocity.y);
		_rb.velocity = Vector3.SmoothDamp(_rb.velocity, targetVelocity, ref _velocity, _movementSmoothing);

		if (move > 0 && !_facingRight)
			Flip();
		else if (move < 0 && _facingRight)
			Flip();

		if (_grounded && jump)
		{
			_grounded = false;
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
