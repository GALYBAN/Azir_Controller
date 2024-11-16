using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;


public class TPSController : MonoBehaviour
{

    private CharacterController _controller;
    private Transform _camera;
    private Transform _lookAtPlayer;
    private Animator _animator;

    //---------------Camaras---------------------//

    [SerializeField] private GameObject _normalCamera;
    [SerializeField] private GameObject _aimCamera;

    //---------------Input-----------------------//
    private float _horizontal;
    private float _vertical;
    private float _turnSmoothVelocity;
    [SerializeField] private float _JumpHeight = 2;
    [SerializeField] private float _movementSpeed = 5f;

    //---------------Graveded--------------------//
    [SerializeField] private float _gravity = -9.81f;
    [SerializeField] private Vector3 _playerGravity;

    //---------------Grounded-------------------//
    [SerializeField] float _sensorRadius = 0.5f;
    [SerializeField] Transform _sensorPosition;
    [SerializeField] private LayerMask _groundLayer;

    [SerializeField] private AxisState xAxis;
    [SerializeField] private AxisState yAxis;

    private Vector3 moveDirection;


    void Awake()
    {
        _controller = GetComponent<CharacterController>();
        _camera = Camera.main.transform;
        _lookAtPlayer = GameObject.Find("LookAtPlayer").transform;
        _animator = GetComponentInChildren<Animator>();
    }

    void Update()
    {
        _horizontal = Input.GetAxis("Horizontal");
        _vertical = Input.GetAxis("Vertical");

        if(Input.GetButtonDown("Fire2"))
        {
            _normalCamera.SetActive(false);
            _aimCamera.SetActive(true);
        }
        else if (Input.GetButtonUp("Fire2"))
        {
            _normalCamera.SetActive(true);
            _aimCamera.SetActive(false);
        }
        
        Movimiento(); 


        if(Input.GetButtonDown("Jump") && IsGrounded())
        {
            _animator.SetBool("IsJumping", true);
            Jump();
        }

        Gravity();
    }

    void Movimiento()
    {
        Vector3 move= new Vector3(_horizontal, 0, _vertical);

        _animator.SetFloat("VelZ", _vertical);
        _animator.SetFloat("VelX", _horizontal);

        yAxis.Update(Time.deltaTime);
        xAxis.Update(Time.deltaTime);

        transform.rotation = Quaternion.Euler(0, xAxis.Value, 0);
        _lookAtPlayer.rotation = Quaternion.Euler(yAxis.Value, xAxis.Value, 0);

        if(move != Vector3.zero)
        {
            float targetAngle = Mathf.Atan2(move.x, move.z) * Mathf.Rad2Deg + _camera.eulerAngles.y;
            Vector3 moveDirection = Quaternion.Euler(0, targetAngle, 0) * Vector3.forward;

            _controller.Move(moveDirection * _movementSpeed * Time.deltaTime);
        }

    }

    bool IsGrounded()
    {
        return Physics.CheckSphere(_sensorPosition.position, _sensorRadius, _groundLayer);
    }

    void Jump()
    {
        _playerGravity.y = Mathf.Sqrt(_JumpHeight * -2 * _gravity);
    }

    void Gravity()
    {
        if(!IsGrounded())
        {
             _playerGravity.y += _gravity * Time.deltaTime;
        }
        else if(IsGrounded() && _playerGravity.y < 0)
        {
            _animator.SetBool("IsJumping", false);
            _playerGravity.y = -1;
        }

        _controller.Move(_playerGravity * Time.deltaTime);
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(_sensorPosition.position, _sensorRadius);
    }
}
