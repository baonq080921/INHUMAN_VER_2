using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerWeaponController : MonoBehaviour
{
    private const float REFERENCE_BULLET_SPEED = 20;
    //This is the default speed from whcih our mass formula is derived.

    private Player player;


    [SerializeField] private Weapon currentWeapon;
    private bool weaponReady;
    private bool isShooting;

    [Header("Bullet details")]
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private float bulletSpeed;

    

    [SerializeField] private Transform weaponHolder;

    [Header("Inventory")]
    [SerializeField] private List<Weapon> weaponSlots;
    [SerializeField] private int maxSlots = 2;  

    private void Start()
    {
        player = GetComponent<Player>();
        currentWeapon.bulletsInMagazine = currentWeapon.magazineCapacity;
        Invoke("EquipWeaponStart",2f);
        AssginInputEvents();
    }

    void Update()
    {
        if(isShooting) Shoot();

        if(Input.GetKeyDown(KeyCode.T)){
            currentWeapon.ToggleBurstMode();
        }
    }

    #region Slots - Managment - Equip/PickUp/Drop/Ready


    private void EquipWeaponStart() => EquipWeapon(0);
    private void EquipWeapon(int i){
        SetWeaponReady(false);
        currentWeapon = weaponSlots[i];
        player.weaponVisuals.PlayWeaponEquipAnimation();

        CameraManager.instance.ChangeCameraDistance(currentWeapon.cameraDistance);
    }

    public void PickupWeapon(Weapon newWeapon){
        if(weaponSlots.Count >= maxSlots){
            return;
        }
        weaponSlots.Add(newWeapon);
        player.weaponVisuals.SwitchOnBackupWeaponModels();
    }

    private void DropWeapon(){
        if(HasOnlyOneWeapon()){
            return;
        }
        weaponSlots.Remove(currentWeapon);
        EquipWeapon(0);
    }

    public void SetWeaponReady(bool ready) => weaponReady = ready;

    public bool WeaponReady() => weaponReady;

    public bool HasOnlyOneWeapon() => weaponSlots.Count <= 1;


     #endregion

     IEnumerator BurstFire(){
        SetWeaponReady(false);
        for(int i = 1 ; i <= currentWeapon.bulletsPerShot; i++){
            FireSingleBullet();
            yield return new WaitForSeconds(currentWeapon.burstFireDelay);
            if(i >= currentWeapon.bulletsPerShot){
                SetWeaponReady(true);
            }
            
        }
     }
    private void Shoot()
    {

        if (!WeaponReady()) return;

        if (!currentWeapon.CanShoot())
        {
            Debug.Log("OUt of bullet");
            return;
        }

        player.weaponVisuals.PlayFireAnimation();

        if (currentWeapon.shootType == ShootType.Single) isShooting = false;
        if(currentWeapon.BurstActive()){
            StartCoroutine("BurstFire");
            return;
        }
        FireSingleBullet();

    }

    private void FireSingleBullet()
    {

        currentWeapon.bulletsInMagazine --;
        // ObjectPool.instance.SaySomething();
        GameObject newBullet = ObjectPool.instance.GetBullet();
        //Instantiate(bulletPrefab, gunPoint.position, Quaternion.LookRotation(gunPoint.forward));

        newBullet.transform.position = GunPoint().position;
        newBullet.transform.rotation = Quaternion.LookRotation(GunPoint().forward);

        Rigidbody rbNewBullet = newBullet.GetComponent<Rigidbody>();
        Bullet bulletScript = newBullet.GetComponent<Bullet>();
        bulletScript.BulletSetUp(currentWeapon.gunDistance);


        Vector3 bulletsDirection = currentWeapon.ApplyBulletSpread(BulletDirection());
        rbNewBullet.mass = REFERENCE_BULLET_SPEED / bulletSpeed;
        rbNewBullet.linearVelocity = bulletsDirection * bulletSpeed;
    }

    private void Reload()
    {
        SetWeaponReady(false);
        player.weaponVisuals.PlayReloadAnimation();
    }


    // This method is used to get the direction of the bullet based on the aim position and the gun point position.
    // It calculates the direction vector from the gun point to the aim position and normalizes it.
    public Vector3 BulletDirection()
    {
        Transform aim = player.aim.Aim();

        Vector3 direction = (aim.position - GunPoint().position).normalized;

        if(player.aim.CanAimPrecisly() == false && player.aim.Target() == null)
            direction.y = 0;

        return direction;
    }

    public Transform GunPoint() => player.weaponVisuals.CurrentWeaponModel().gunPoint;

    // Return the backup weapon in the weapon slots
    public Weapon BackUpWeapon(){
        foreach(Weapon weapon in weaponSlots){
            if(weapon != currentWeapon){
                Debug.Log(weapon.weaponType);
                return weapon; 
            }
        }
        return null;
    }
    public Weapon CurrentWeapon() => currentWeapon;

    #region INPUT REGION
    private void AssginInputEvents(){
        PlayerControls controls = player.controls;
        controls.Character.EquipSlot1.performed += context => EquipWeapon(0);
        controls.Character.EquipSlot2.performed += context => EquipWeapon(1);
        controls.Character.Drop.performed += context => DropWeapon();
        controls.Character.Fire.performed += context => isShooting = true;
        controls.Character.Fire.canceled += context => isShooting = false;
        controls.Character.Reload.performed += context =>{
        if(currentWeapon.CanReload() && WeaponReady())
            {
                Reload();

            }
        };
    #endregion
    }

    
}
