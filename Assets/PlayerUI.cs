using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerUI : MonoBehaviour
{
    public Player player;

    [Header("UI Component")]
    [Header("Ammo")]
    public Text maxAmmo;
    public Text currentAmmo;

    private void Init()
    {
        maxAmmo.text = player.gun.maxAmmo.ToString();
    }

    private void Start()
    {
        if (player == null)
            player = GameManager.Instance.player;

        Init();
    }

    private void Update()
    {
        currentAmmo.text = player.gun.CurrentAmmo.ToString();
    }
}
