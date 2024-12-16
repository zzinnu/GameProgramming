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
    public GameObject cam;
    public Rigidbody Rb { get; set; }
    public bool IsRunning { get; set; }

    private Vector2 m_moveDir;
    private Vector2 m_rotateDir;
   
    private float m_moveSpeed;
   
    private int m_gunIndex;
    
    private bool m_isGrounded;
    private Player m_player;

    public void OnMove(InputAction.CallbackContext context)
    {
        if (GameManager.Instance.isPaused)
            return;

        // 눌렀을 때만 이동할 수 있도록 canceled는 Vector2.zero로 초기화
        if (context.performed)
            m_moveDir = context.ReadValue<Vector2>();
        else if (context.canceled)
            m_moveDir = Vector2.zero;
    }

    public void OnRotate(InputAction.CallbackContext context)
    {
        if (GameManager.Instance.isPaused)
            return;

        if (context.performed)
        {
            m_rotateDir = context.ReadValue<Vector2>();

            // 카메라 회전 제한
            // 대충 cam.transform.localRotation.x가 0.6f이면 90도 정도인듯
            if (cam.transform.localRotation.x > 0.6f)
            {
                if ((cam.transform.localRotation.w < 0.0f && m_rotateDir.y > 0) || (cam.transform.localRotation.w > 0.0f && m_rotateDir.y < 0))
                    m_rotateDir.y = 0;
            }
        }
            
        else if (context.canceled)
            m_rotateDir = Vector2.zero;
    }

    public void OnRun(InputAction.CallbackContext context)
    {
        if (GameManager.Instance.isPaused)
            return;

        if (context.performed)
        {
            if(m_player.currentStamina > 10.0f)
            {
                IsRunning = true;
                m_player.isUseStamina = true;
                m_player.gun.SetAnimParamDefault();
            }
        }
        else if (context.canceled)
        {
            IsRunning = false;
            m_player.isUseStamina = false;
        }
    }

    public void OnShoot(InputAction.CallbackContext context)
    {
        if (GameManager.Instance.isPaused)
            return;

        // 꾹 누르면 자동으로 발사, 땠을 때 발사 중지
        // 달리고 있을땐 안됨
        if (context.performed && !IsRunning)
        {
            StartCoroutine(m_player.gun.FireCoroutine());
        }
        else if(context.canceled)
        {
            StartCoroutine(m_player.gun.StopFireCoroutine());
            m_player.gun.anim.SetBool("Fire", false);
        }
    }

    public void OnBulletChange(InputValue input)
    {
        m_gunIndex++;
        // m_playerGun.gunType = ((GunType)(m_gunIndex % 2));
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        if (GameManager.Instance.isPaused)
            return;

        // 점프는 스태미너 10 소모
        // 땅에 다시 착지할 때까지 점프 불가능
        if (context.performed && m_isGrounded && m_player.currentStamina > 5.0f)
        {
            m_player.isUseStamina = true;
            m_player.currentStamina -= 5.0f;
            Rb.AddForce(transform.up * jumpForce, ForceMode.Impulse);
            m_isGrounded = false;

            m_player.audioSources[1].PlayOneShot(m_player.jumpSound);

            Debug.Log("Jump");
        }
    }

    // 점프 이후 착지할 때 collsion을 통해 판단
    private void OnCollisionEnter(Collision collision)
    {
        LayerMask collisionLayer = 1 << collision.gameObject.layer;
        if ((collisionLayer & groundLayer) == collisionLayer)
        {
            m_isGrounded = true;
            m_player.isUseStamina = false;
        }   
    }

    public void OnReload(InputAction.CallbackContext context)
    {
        if(context.performed)
        {
            m_player.gun.Reload();
        }
    }

    private void Init()
    {
        // Cursor
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;

        // Camera
        // cam = Camera.main;
        // cam.transform.localRotation = Quaternion.identity;

        // Rigidbody
        Rb = GetComponent<Rigidbody>();
        m_isGrounded = true;

        // Player
        m_player = GetComponent<Player>();

    }

    void Awake()
    {
        Init();
    }

    void FixedUpdate()
    {
        if(GameManager.Instance.isPaused)
            return;

        Rb.drag = m_isGrounded ? m_moveDir.magnitude > 0 ? 5 : 20 : 2; // 땅에 있을 때는 이동 중이면 5, 아니면 20, 점프중일땐 2

        // Running 판단
        // Runnig일 경우 스태미나 감소, 0이하일 경우 Running 해제
        m_moveSpeed = IsRunning ? runSpeed : walkSpeed;
        if(IsRunning)
        {
            if(m_player.currentStamina <= 0.0f)
            {
                IsRunning = false;
                m_player.isUseStamina = false;
            }

            m_player.currentStamina -= 0.1f;
        }

        // 상대적인 이동 방향을 구하기 위해 TransformDirection 사용
        Vector3 relativeMovement = transform.TransformDirection(new Vector3(m_moveDir.x, 0, m_moveDir.y));
        Rb.AddForce(relativeMovement * m_moveSpeed, ForceMode.Impulse);

        //m_rb.velocity = relativeMovement * m_moveSpeed;

        // 회전
        // 몸체 회전이후 카메라 회전
        // 카메라 y축 회전의 경우 부모 오브젝트가 이미 수행하므로, 카메라는 x축 회전만 진행
        transform.localRotation *= Quaternion.Euler(0, m_rotateDir.x * rotateSpeed * Time.fixedDeltaTime, 0);
        cam.transform.localRotation = Quaternion.Euler(cam.transform.localRotation.eulerAngles.x - m_rotateDir.y * rotateSpeed * Time.fixedDeltaTime, 0, 0);
        // Debug.Log(cam.transform.localRotation);
    }
}
