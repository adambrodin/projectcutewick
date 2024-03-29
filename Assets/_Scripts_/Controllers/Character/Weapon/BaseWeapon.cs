﻿#pragma warning disable CS0649
using TMPro;
using UnityEngine;

public abstract class BaseWeapon : MonoBehaviour
{
    #region BaseVariables

    public Rigidbody2D bullet;
    public float fireForce = 20f;
    public float range_Spread;

    public TextMeshProUGUI ammo_Text;

    [SerializeField] public static int ammo;
    [SerializeField] private int internal_Ammo;

    [SerializeField] private float lastfailed;
    [SerializeField] private float failRate;

    [SerializeField] private bool accuracy;
    [SerializeField] private bool can_Reload; //under utveckling
    [SerializeField] public static bool is_Firing;

    #endregion

    #region Properties
    public bool semi_Auto = false;
    public bool full_Auto = false;
    public bool shotgun = false;
    public bool range = false;
    public float spawnChance;
    #endregion

    private PlayerMovement playerCtrl;

    void Awake()
    {
        playerCtrl = GameObject.Find("Player").GetComponent<PlayerMovement>();
        ammo_Text = GameObject.Find("Ammo Text").GetComponent<TextMeshProUGUI>();
    }

    void FixedUpdate()
    {
        internal_Ammo = Mathf.Clamp(internal_Ammo, 0, 60);
        ammo_Text.text = internal_Ammo.ToString();
    }

    public void Fire(string sound)
    {
        is_Firing = true;

        internal_Ammo -= 1;

        if (accuracy == true)
        {
            range_Spread = Random.Range(1, -1);
        }

        if (internal_Ammo > 0)
        {
            AudioManager.Instance.SetState(sound, true);

            if (playerCtrl.facingRight == 1)
            {

                Rigidbody2D bulletInstance = Instantiate(bullet, transform.position, Quaternion.Euler(new Vector3(0, 0, 0))) as Rigidbody2D;
                bulletInstance.velocity = new Vector2(fireForce, range_Spread);
            }

            else
            {
                Rigidbody2D bulletInstance = Instantiate(bullet, transform.position, Quaternion.Euler(new Vector3(0, 0, 180))) as Rigidbody2D;
                bulletInstance.velocity = new Vector2(-fireForce, range_Spread);
            }
        }

        else
        {
            if (Time.time - lastfailed > 1 / failRate)
            {
                AudioManager.Instance.SetState("Weapon_Fail", true);
            }
        }
    }
}