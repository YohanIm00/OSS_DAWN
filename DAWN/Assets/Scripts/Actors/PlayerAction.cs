using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAction : MonoBehaviour
{
    // Player movement speed
    private float _speed = 5;

    // Variables to store input from the player
    private float _horizontal;
    private float _vertical;
    private bool _isHorizonMove; // Flag to determine if horizontal movement is prioritized

    private float _raycastDistance = 1.5f;
    private Vector2 _rayDirection;
    public RaycastHit2D hit;
    public Customer hitCustomer;

    // Components for Rigidbody2D and Animator
    private Rigidbody2D _rigid;
    private Animator _anim;

    // Initialize components on awake
    void Awake()
    {
        // Get references to Rigidbody2D and Animator components attached to the player
        _rigid = GetComponent<Rigidbody2D>();
        _anim = GetComponent<Animator>();
    }

    void Update()
    {
        // Update the direction of the ray based on player's movement
        RotateRay();

        // Generate and visualize the raycast in the scene
        GenerateRay();


        // Get input values for horizontal and vertical axes
        _horizontal = Input.GetAxisRaw("Horizontal");
        _vertical = Input.GetAxisRaw("Vertical");

        // Detect button down and up events for horizontal and vertical movement
        bool hDown = Input.GetButtonDown("Horizontal");
        bool vDown = Input.GetButtonDown("Vertical");
        bool hUp = Input.GetButtonUp("Horizontal");
        bool vUp = Input.GetButtonUp("Vertical");

        // Determine if the player should prioritize horizontal movement
        if (hDown)
            _isHorizonMove = true; // Start horizontal movement
        else if (vDown)
            _isHorizonMove = false; // Start vertical movement
        else if (hUp || vUp)
            _isHorizonMove = _horizontal != 0; // Continue horizontal movement if horizontal axis is not zero

        // Update the animator parameters based on player input
        if (_anim.GetInteger("hAxisRaw") != _horizontal)
        {
            _anim.SetBool("isChange", true); // Signal that animation needs to change
            _anim.SetInteger("hAxisRaw", (int)_horizontal); // Update horizontal axis in animator
        }
        else if (_anim.GetInteger("vAxisRaw") != _vertical)
        {
            _anim.SetBool("isChange", true); // Signal that animation needs to change
            _anim.SetInteger("vAxisRaw", (int)_vertical); // Update vertical axis in animator
        }
        else
        {
            _anim.SetBool("isChange", false); // No change needed in animation
        }
    }

    // FixedUpdate is used for consistent physics updates
    void FixedUpdate()
    {
        // Calculate movement direction based on input and movement priority
        Vector2 moveVec = _isHorizonMove ? new Vector2(_horizontal, 0) : new Vector2(0, _vertical);

        // Apply velocity to the Rigidbody2D to move the player
        _rigid.velocity = moveVec * _speed;
    }

    // Method to generate a raycast for detecting objects in front of the player
    public void GenerateRay()
    {
        // Cast a ray in the current direction (_rayDirection) to detect objects in the "Customer" layer
        hit = Physics2D.Raycast(transform.position, _rayDirection, _raycastDistance, LayerMask.GetMask("Customer"));

        // Visualize the raycast in the Scene view using a green line
        Debug.DrawLine(transform.position, (Vector2)transform.position + _rayDirection * _raycastDistance, Color.green);
    }

    // Method to update the direction of the ray based on player's movement input
    public void RotateRay()
    {
        // Get the current velocity of the player from the Rigidbody2D
        Vector2 velocity = GetComponent<Rigidbody2D>().velocity;

        // Determine the ray direction based on player's movement input
        if (_horizontal > 0f)
        {
            _rayDirection = Vector2.right; // Ray points to the right
        }
        else if (_horizontal < 0f)
        {
            _rayDirection = Vector2.left; // Ray points to the left
        }
        else if (_vertical > 0f)
        {
            _rayDirection = Vector2.up; // Ray points upwards
        }
        else if (_vertical < 0f)
        {
            _rayDirection = Vector2.down; // Ray points downwards
        }
    }
}