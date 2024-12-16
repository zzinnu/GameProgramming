using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerUI : MonoBehaviour
{
    public Player player;

    [Header("UI Component")]
    [Header("Ammo")]
    public TMP_Text maxAmmo;
    public TMP_Text currentAmmo;

    private void Init()
    {
        maxAmmo.text = player.gun.maxAmmo.ToString();
    }

    private void Start()
    {
        if (player == null)
            player = GameManager.Instance.Player;

        Init();
    }

    private void Update()
    {
        currentAmmo.text = player.gun.CurrentAmmo.ToString();
    }
}
