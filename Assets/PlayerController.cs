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
    public GameObject shootPoint;
    public GameObject[] bullets;

    private Vector2 m_moveDir;
    private Vector2 m_rotateDir;
    private bool isRunning;
    private float moveSpeed;
    private int useBulletIndex;

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
        isRunning = input.isPressed;
        // Debug.Log("IsRunning: " + isRunning);
    }

    public void OnShoot(InputValue input)
    {
        if(input.isPressed)
        {
            GameObject bullet = Instantiate(bullets[useBulletIndex]);
            bullet.transform.position = shootPoint.transform.position;
            bullet.transform.rotation = shootPoint.transform.rotation;
        }
    }

    public void OnBulletChange(InputValue input)
    {
        useBulletIndex = (useBulletIndex + 1) % bullets.Length;
    }

    void Awake()
    {
        // Cursor
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;

        // Default bullet
        useBulletIndex = 0;
        if (bullets.Length == 0)
            Debug.LogError("Player의 Bullets 필요함");

    }

    void FixedUpdate()
    {
        moveSpeed = isRunning ? runSpeed : walkSpeed;
        moveSpeed *= Time.deltaTime;

        transform.Translate(m_moveDir.x * moveSpeed, 0, m_moveDir.y * moveSpeed);
        transform.Rotate(0, m_rotateDir.x * rotateSpeed * Time.deltaTime, 0);
    }
}
