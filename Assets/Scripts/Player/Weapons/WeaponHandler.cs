using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TowerDefence.Managers;
using TowerDefence.Enemies;

public enum WeaponAim {
    NONE,
    SELF_AIM,
    AIM
}

public enum WeaponFireType {
    SINGLE,
    MULTIPLE
}

public enum WeaponBulletType {
    BULLET,
    BUCKSHOT,
    NONE
}

public class WeaponHandler : MonoBehaviour {

    private Animator anim;

    public WeaponAim weapon_Aim;

    [SerializeField]
    private AudioSource shootSound;
    [SerializeField]
    private AudioSource reloadSound;
    [SerializeField]
    private AudioSource gunSwapSound;

    public WeaponFireType fireType;
    public WeaponBulletType bulletType;
    public GameObject attack_Point;
    public GameObject crosshair;

    public float fireRate = 15f;

    public int currentAmmo;
    public int buckshotCurrentAmmo;
    private int maxAmmo = 6;
    private int buckShotMaxAmmo = 2;
    private float reloadTime = 2f;
    public bool isReloading = false;
    public float damage = 20f;

    Camera mainCam;
    WeaponManager weaponManager;
    private float nextTimeToFire;

    void Awake() 
    {
        weaponManager = GetComponent<WeaponManager>();
        anim = GetComponent<Animator>();
        mainCam = Camera.main;

    }
    void Start()
    {
        if (currentAmmo <= 0)
        {
            currentAmmo = maxAmmo;
        }
        if (buckshotCurrentAmmo <= 0)
        {
            buckshotCurrentAmmo = buckShotMaxAmmo;
        }
    }
    void OnEnable()
    {
        isReloading = false;
    }
    void Update()
    {
        if (isReloading)
        {
            return;
        }
        if (currentAmmo <= 0)
        {
            StartCoroutine(Reload());
            return;
        }
        if (buckshotCurrentAmmo <= 0)
        {
            StartCoroutine(ReloadShotgun());
            return;
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
        if(attack_Point.activeInHierarchy) 
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
    public void RemoveAmmo()
    {
        currentAmmo--;
    }
    public void RemoveBuckshot()
    {
        buckshotCurrentAmmo--;
    }
    IEnumerator Reload()
    {
        isReloading = true;
        Debug.Log("Reloading...");

        ReloadAnimation();

        yield return new WaitForSeconds(reloadTime);

        currentAmmo = maxAmmo;
        isReloading = false;
        Debug.Log("Reload Complete..");
    }
    IEnumerator ReloadShotgun()
    {
        isReloading = true;
        Debug.Log("Reloading...");

        ReloadAnimation();

        yield return new WaitForSeconds(reloadTime);

        buckshotCurrentAmmo = buckShotMaxAmmo;
        isReloading = false;
        Debug.Log("Reload Complete..");
    }
    public void BulletFired()
    {
        RaycastHit hit;
        Debug.Log("Bullet Fired");

        if (Physics.Raycast(mainCam.transform.position, mainCam.transform.forward, out hit))
        {
            if (hit.transform.tag == Tags.ENEMY_TAG)
            {
                hit.transform.GetComponent<Enemy>().Damage(damage);
            }
            else
            {
                Debug.Log("Missed");
            }
        }
    }
}





































