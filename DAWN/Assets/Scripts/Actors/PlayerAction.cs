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
}
