using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponCotnroller : MonoBehaviour
{
    public Animator anim;
    public PlayerController playerController;

    private void Awake()
    {
        if(anim == null)
            anim = GetComponent<Animator>();

        if(playerController == null)
            playerController = GetComponentInParent<PlayerController>();
    }

    private void Update()
    {
        anim.SetFloat("Velocity", playerController.Rb.velocity.magnitude);
        anim.SetBool("IsRunning", playerController.IsRunning);
    }
}
