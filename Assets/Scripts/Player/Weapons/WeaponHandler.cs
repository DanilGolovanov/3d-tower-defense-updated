using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//script that handles the weapon types and reload system

//aiming for weapon type (none = melee)
public enum WeaponAim {
    NONE,
    SELF_AIM,
    AIM
}

//fire rate for projectiles
public enum WeaponFireType {
    SINGLE,
    MULTIPLE
}

//ammo type for projectiles (none = melee)
public enum WeaponBulletType {
    BULLET,
    BUCKSHOT,
    NONE
}

public class WeaponHandler : MonoBehaviour {

    private Animator anim;
    public WeaponAim weapon_Aim;
    [Header ("Weapon Audio")]
    [SerializeField]
    private AudioSource shootSound;
    [SerializeField]
    private AudioSource reloadSound;
    [SerializeField]
    private AudioSource gunSwapSound;
    [Header("Weapon Calibration")]
    public WeaponFireType fireType;
    public WeaponBulletType bulletType;
    public GameObject attack_Point;
    public GameObject crosshair;
    public float fireRate = 15f;

    [Header("Ammo Calibration")]
    //ammo UI text
    public Text ammoText;
    public Text BuckshotAmmoText;
    //ammo variables
    public int currentAmmo;
    public int buckshotCurrentAmmo;
    private int pistolClip = 6;
    private int buckshotClip = 2;
    public static int maxAmmo = 12;
    public static int maxBuckshot = 4;
    public bool outOfAmmo = false;
    public bool outOfBuckshot = false;
    private float reloadTime = 2f;
    public bool isReloading = false;

    //references
    WeaponManager weaponManager;

    void Awake() 
    {
        weaponManager = GetComponent<WeaponManager>();
        anim = GetComponent<Animator>();
    }
    void Start()
    {
        if (currentAmmo <= 0)
        {
            currentAmmo = pistolClip;
        }
        if (buckshotCurrentAmmo <= 0)
        {
            buckshotCurrentAmmo = buckshotClip;
        }
    }
    //when script is enabled make sure reloading is toggled off
    void OnEnable()
    {
        isReloading = false;
    }
    //ammo UI
    //Ammo checking
    void Update()
    {
        ammoText.text = currentAmmo.ToString() + "/" + maxAmmo.ToString();
        BuckshotAmmoText.text = buckshotCurrentAmmo.ToString() + "/" + maxBuckshot.ToString();

        if (isReloading)
        {
            return;
        }

        if (currentAmmo <= 0)
        {
            AmmoCheck();
            if (!outOfAmmo)
            {
                StartCoroutine(Reload());
                return;
            }

        }

        if (buckshotCurrentAmmo <= 0)
        {
            AmmoCheck();
            if (!outOfBuckshot)
            {
                StartCoroutine(ReloadShotgun());
                return;
            }
        }
    }
    public void RemoveAmmo()
    {
        currentAmmo--;
    }
    public void RemoveBuckshot()
    {
        buckshotCurrentAmmo--;
    }
    //corotuines to function alongside reload animations
    IEnumerator Reload()
    {
        isReloading = true;
        Debug.Log("Reloading...");

        ReloadAnimation();

        yield return new WaitForSeconds(reloadTime + 1f);

        currentAmmo = pistolClip;
        maxAmmo -= pistolClip;
        isReloading = false;
        Debug.Log("Reload Complete..");
    }
    IEnumerator ReloadShotgun()
    {
        isReloading = true;
        Debug.Log("Reloading...");

        ReloadAnimation();

        yield return new WaitForSeconds(reloadTime + 1f);

        buckshotCurrentAmmo = buckshotClip;
        maxBuckshot -= buckshotClip;
        isReloading = false;
        Debug.Log("Reload Complete..");
    }
    //check ammo function to check current max ammo value
    //trigger control bool
    private void AmmoCheck()
    {
        if (maxAmmo <= 0)
        {
            maxAmmo = 0;
            outOfAmmo = true;
        }
        else
        {
            outOfAmmo = false;
        }

        if (maxBuckshot <= 0)
        {
            maxBuckshot = 0;
            outOfBuckshot = true;
        }
        else
        {
            outOfBuckshot = false;
        }
    }
    //Visual events to be called during animations
    public void ShootAnimation()
    {
        anim.SetTrigger(AnimationTags.SHOOT_TRIGGER);
    }
    public void ReloadAnimation()
    {
        anim.SetTrigger(AnimationTags.RELOAD_TRIGGER);
    }
    //Sound events to be called during animations
    void Play_ShootSound()
    {
        shootSound.Play();
    }

    void Play_ReloadSound()
    {
        reloadSound.Play();
    }

    void Play_GunSwapSound()
    {
        gunSwapSound.Play();
    }
    //melee attacks
    void Turn_On_AttackPoint()
    {
        attack_Point.SetActive(true);
    }

    void Turn_Off_AttackPoint()
    {
        if (attack_Point.activeInHierarchy)
        {
            attack_Point.SetActive(false);
        }
    }
    void Turn_On_CrossHair()
    {
        crosshair.SetActive(true);
    }
    void Turn_Off_CrossHair()
    {
        if (crosshair.activeInHierarchy)
        {
            crosshair.SetActive(false);
        }

    }
}





































