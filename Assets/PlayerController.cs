using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public float walkSpeed;
    public float runSpeed;
    public float rotateSpeed;
    public float jumpForce;
    public LayerMask groundLayer;
    public Camera cam;

    private Vector2 m_moveDir;
    private Vector2 m_rotateDir;
    private bool m_isRunning;
    private float m_moveSpeed;
    
    private float m_rotationX;
    private Gun m_playerGun;
    private int m_gunIndex;
    private Rigidbody m_rb;
    private bool m_isGrounded;

    public void OnMove(InputValue input)
    {
        m_moveDir = input.Get<Vector2>().normalized;
        // Debug.Log("moveDir: " + m_moveDir);
    }

    public void OnRotate(InputValue input)
    {
        m_rotateDir = input.Get<Vector2>();
        // Debug.Log("rotateDir: " + m_rotateDir);
    }

    public void OnRun(InputValue input)
    {
        if(GameManager.Instance.player.currentStamina > 10.0f)
        {
            GameManager.Instance.player.isUseStamina = input.isPressed;
            m_isRunning = input.isPressed;
        }
        // Debug.Log("IsRunning: " + isRunning);
    }

    public void OnShoot(InputValue input)
    {
        if(input.isPressed)
        {
            m_playerGun.Fire();
        }
    }

    public void OnBulletChange(InputValue input)
    {
        m_gunIndex++;
        m_playerGun.gunType = ((GunType)(m_gunIndex % 2));
    }

    public void OnJump(InputValue input)
    {
        if(input.isPressed && m_isGrounded && GameManager.Instance.player.currentStamina > 10.0f)
        {
            GameManager.Instance.player.isUseStamina = true;
            GameManager.Instance.player.currentStamina -= 10.0f;
            m_rb.AddRelativeForce(Vector3.up * jumpForce, ForceMode.Impulse);
            m_isGrounded = false;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        LayerMask collisionLayer = 1 << collision.gameObject.layer;
        if ((collisionLayer & groundLayer) == collisionLayer)
        {
            m_isGrounded = true;
            GameManager.Instance.player.isUseStamina = false;
        }
            
    }

    private void Init()
    {
        // Cursor
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;

        // Camera
        cam = Camera.main;
        cam.transform.localRotation = Quaternion.identity;

        // Gun type
        m_playerGun = GetComponent<Gun>();

        // Rigidbody
        m_rb = GetComponent<Rigidbody>();
        m_isGrounded = true;
        
    }

    void Awake()
    {
        Init();
    }

    void FixedUpdate()
    {
        m_moveSpeed = m_isRunning ? runSpeed : walkSpeed;
        if(m_isRunning)
        {
            if(GameManager.Instance.player.currentStamina < 0.0f)
            {
                m_isRunning = false;
                GameManager.Instance.player.isUseStamina = false;
            }

            GameManager.Instance.player.currentStamina -= 0.1f;
        }

        transform.Translate(m_moveDir.x * m_moveSpeed * Time.deltaTime, 0, m_moveDir.y * m_moveSpeed * Time.deltaTime);

        transform.Rotate(0, m_rotateDir.x * rotateSpeed * Time.deltaTime, 0); // y축 회전(플레이어 자체)
        m_rotationX += m_rotateDir.y * rotateSpeed * Time.deltaTime; // x축 회전(카메라만)
        m_rotationX = Mathf.Clamp(m_rotationX, -90.0f, 90.0f);
        cam.transform.localRotation = Quaternion.Euler(-m_rotationX, 0, 0);
    }
}
