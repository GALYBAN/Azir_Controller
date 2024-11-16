using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlayerController : MonoBehaviour
{
    //---------------Components------------------//
    private CharacterController _controller;
    private Transform _camara;
    private Animator _animator;

    //---------------Input-----------------------//
    private float _horizontal;
    private float _vertical;
    private float _turnSmoothVelocity;
    [SerializeField] private float _JumpHeight = 2;
    [SerializeField] private float _pushForce = 10;
    [SerializeField] private float _speed = 5f;
    [SerializeField] private float _turnSmoothTime = 0.05f;

    //---------------Graveded--------------------//
    [SerializeField] private float _gravity = -9.81f;
    [SerializeField] private Vector3 _playerGravity;

    //---------------Grounded-------------------//
    [SerializeField] float _sensorRadius = 0.5f;
    [SerializeField] Transform _sensorPosition;
    [SerializeField] private LayerMask _groundLayer;

    
    private Vector3 moveDirection;
    
    void Awake()
    {
        _controller = GetComponent<CharacterController>();
        _camara = Camera.main.transform;
        _animator = GetComponentInChildren<Animator>(); 
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        _horizontal = Input.GetAxis("Horizontal");
        _vertical = Input.GetAxis("Vertical");
        
     
        if (Input.GetButton("Fire2"))
        {
            AimMovimiento();
        }
        else 
        {
            Movimiento(); 
        }


        if(Input.GetButtonDown("Jump") && IsGrounded())
        {
            Jump();
        }
        else
        {
            _animator.SetBool("IsJumping", false);
        }

        Gravity();

        if(Input.GetKeyDown(KeyCode.R))
        {
            RayTest();
        }
        
        
    }

    void Movimiento()
    {
        Vector3 direction= new Vector3(_horizontal, 0, _vertical);

        _animator.SetFloat("VelZ", direction.magnitude);
        _animator.SetFloat("VelX", 0);

        if(direction != Vector3.zero)
        {
            float targeAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + _camara.eulerAngles.y;
            float smoothAngle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targeAngle, ref _turnSmoothVelocity, _turnSmoothTime);
       
            transform.rotation = Quaternion.Euler(0,smoothAngle, 0); 

             moveDirection = Quaternion.Euler(0, targeAngle, 0) * Vector3.forward;
            
            _controller.Move(moveDirection * _speed * Time.deltaTime);
        }
    }

    
    void AimMovimiento()
    {
        Vector3 direction= new Vector3(_horizontal, 0, _vertical);

        _animator.SetFloat("VelZ",_vertical);
        _animator.SetFloat("VelX",_horizontal);


        float targeAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + _camara.eulerAngles.y;
        float smoothAngle = Mathf.SmoothDampAngle(transform.eulerAngles.y, _camara.eulerAngles.y, ref _turnSmoothVelocity, _turnSmoothTime);
       
        transform.rotation = Quaternion.Euler(0, smoothAngle, 0); 

        if(direction != Vector3.zero)
        {
            moveDirection = Quaternion.Euler(0, targeAngle, 0) * Vector3.forward;
            
            _controller.Move(moveDirection * _speed * Time.deltaTime);
        }
        
        
    }

    void Gravity()
    {
        if(!IsGrounded())
        {
             _playerGravity.y += _gravity * Time.deltaTime;
        }
        else if(IsGrounded() && _playerGravity.y < 0)
        {
            _playerGravity.y = -1;
        }

        _controller.Move(_playerGravity * Time.deltaTime);
    }

    void Jump()
    {
        _playerGravity.y = Mathf.Sqrt(_JumpHeight * -2 * _gravity);
        _animator.SetBool("IsJumping", true);
    }

    bool IsGrounded()
    {
        return Physics.CheckSphere(_sensorPosition.position, _sensorRadius, _groundLayer);

    }

    /*bool IsGrounded()
    {
        RaycastHit hit;
        if(Physics.Raycast(_sensorPosition.position, -transform.up, out hit, 2))
        {
           if(hit.transform.gameObject.layer == 6)
           {
                Debug.DrawRay(_sensorPosition.position, -transform.up * 2, Color.red);
                return true;
           }
           else
           {
                Debug.DrawRay(_sensorPosition.position, -transform.up * 2, Color.green);
                return false;
           }
        }
        else
        {
            return false;
        }
        
    }*/

    void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if (hit.gameObject.tag == "Enemy")
        {
            
        }

        Rigidbody body = hit.collider.attachedRigidbody;

        if (body != null)
        {
            Vector3 pushDirection = new Vector3(moveDirection.x, 0, moveDirection.z);

            body.velocity = pushDirection * _pushForce / body.mass;
        }
    }

    void RayTest()
    {
        RaycastHit hit;
        if(Physics.Raycast(transform.position, transform.forward, out hit, 10))
        {
            Debug.Log(hit.transform.name);
            Debug.Log(hit.transform.position);
            Debug.Log(hit.transform.gameObject.layer);

            if(hit.transform.gameObject.tag == "Enemy")
            {
               Enemy enemyScript = hit.transform.gameObject.GetComponent<Enemy>();
               
               enemyScript.TakeDamage(1);
            }
        }
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(_sensorPosition.position, _sensorRadius);
    }
    
}
