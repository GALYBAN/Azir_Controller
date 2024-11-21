using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThirdPersonController : MonoBehaviour
{

    //Componentes
    private CharacterController _controller;
    private Transform _camera;
    private Animator _animator;

    //Inputs
    private float _horizontal;
    private float _vertical;
    private float _turnSmoothVelocity;

    [SerializeField] private float _jumpHeight = 2f;
    [SerializeField] private float _speed = 5f;
    [SerializeField] private float _turnSmoothTime = 0.1f;

    //Gravedad
    [SerializeField] private float _gravity = -9.81f;
    [SerializeField] private Vector3 _playerGravity;

    //Grounded
    [SerializeField] float _sensorRadius = 0.5f;
    [SerializeField] private Transform _groundCheck;
    [SerializeField] private LayerMask _groundMask;

    //Moviemiento
    private Vector3 moveDirection;

    void Awake()
    {
        _controller = GetComponent<CharacterController>();
        _camera = Camera.main.transform;
        _animator = GetComponentInChildren<Animator>();
    }

    void Update()
    {
        _horizontal = Input.GetAxis("Horizontal");
        _vertical = Input.GetAxis("Vertical");
        
        
        Movimiento(); 

        if(Input.GetButtonDown("Jump") && IsGrounded())
        {
            Jump();
        }
        else
        {
           _animator.SetBool("IsJumping", false);
        }

        Gravity();      
    }

    void Movimiento()
    {
        Vector3 direction = new Vector3(_horizontal, 0, _vertical);

        _animator.SetFloat("VelZ", direction.magnitude);
        _animator.SetFloat("VelX", 0);

        if (direction != Vector3.zero)
        {
            float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + _camera.eulerAngles.y;
            float smoothAngle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref _turnSmoothVelocity, _turnSmoothTime);

            transform.rotation = Quaternion.Euler(0, smoothAngle, 0);

            moveDirection = Quaternion.Euler(0, targetAngle, 0) * Vector3.forward;

            _controller.Move(moveDirection * _speed * Time.deltaTime);
        }

    }

    void Gravity()
    {
        if (!IsGrounded())
        {
            _playerGravity.y += _gravity * Time.deltaTime;
        }
        else if (IsGrounded() && _playerGravity.y < 0)
        {
            _playerGravity.y = -1;
        }

        _controller.Move(_playerGravity * Time.deltaTime);
    }

    void Jump()
    {
        _playerGravity.y = Mathf.Sqrt(_jumpHeight * -2 * _gravity);
        _animator.SetBool("IsJumping", true);
    }

    bool IsGrounded()
    {
        return Physics.CheckSphere(_groundCheck.position, _sensorRadius, _groundMask);
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(_groundCheck.position, _sensorRadius);
    }
}
