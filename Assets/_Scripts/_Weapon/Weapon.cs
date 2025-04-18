
using System;

//using NUnit.Framework;
using UnityEngine;

public enum WeaponType {
    Pistol,
    Revolver,
    AutoRiffle,
    ShotGun,
    Sniper
}

public enum ShootType{

    Single,
    Auto
}

[System.Serializable] // make a class visible on the inspector

public class Weapon
{

    public WeaponType weaponType;
    public ShootType shootType;


    #region Regular Shot mode Variables:
        
    
    public int bulletsPerShot {get;  private set;} // The number of bullets fired per shot
    private float defaultFireRate; // The default fire rate of the weapon
    public float fireRate = 1f;
    private float lastShootTime;

    #endregion

    #region Burst Shot Mode Variables 
    private bool burstAvailable; // Check if the weapon has burst fire mode
    private bool burstActive; // Check if the weapon is in burst fire mode
    private int burstBulletsPerShot;
    private float burstFireRate;
    public float burstFireDelay {get;  private set;} // The delay between each bullet fired in burst fire mode
    #endregion


    #region Magazine Variables
        
    public int bulletsInMagazine;
    public int magazineCapacity;
    public int totalReserveAmmo;

    #endregion


    #region Weapon Generic Variables 
    public float reloadSpeed {get;  private set;} // Make the reload speed on different gun change 
    public float equipmentSpeed {get;  private set;} // Make the equip speed on different gun change

    public float gunDistance{get;   private set;} // The distance from the player to the gun when shooting

    public float cameraDistance {get;  private set;} // The distance from the camera to the gun when shooting

    public Weapon_Data weaponData {get; private set;} // servers as default weapon_data
    #endregion


   
    #region Weapon Spread Variables
    private float baseBulletSpread = 0.5f; // The base spread of the bullet when shooting
    private float currentBulletSpread = 2f; // The spread of the bullet when shooting
    private float maximumBulletSpread = 5f; // The maximum spread of the bullet when shooting
    private float bulletSpreadIncreaseRate = 0.15f; // The rate at which the bullet spread increases when shooting
    private float lastBulletSpreadTime;
    private float spreadCooldown = 1f; // The time it takes for the bullet spread to reset
    
    #endregion

    public Weapon(Weapon_Data weaponData){
        fireRate = weaponData.fireRate;
        defaultFireRate = fireRate;
        weaponType = weaponData.weaponType;
        shootType = weaponData.shootType;
        this.baseBulletSpread = weaponData.baseBulletSpread;
        this.maximumBulletSpread = weaponData.maximumBulletSpread;
        this.bulletSpreadIncreaseRate = weaponData.bulletSpreadIncreaseRate;
   
        this.reloadSpeed = weaponData.reloadSpeed;
        this.equipmentSpeed = weaponData.equipmentSpeed;
        this.gunDistance = weaponData.gunDistance;
        this.cameraDistance = weaponData.cameraDistance;

        this.burstActive = weaponData.burstActive;
        this.burstAvailable = weaponData.burstAvailable;
        this.burstFireRate = weaponData.burstFireRate;
        this.burstFireDelay = weaponData.burstFireDelay;

        this.bulletsPerShot = weaponData.bulletsPerShot;
        this.defaultFireRate = weaponData.defaultFireRate;

        this.bulletsInMagazine = weaponData.bulletsInMagazine;
        this.magazineCapacity = weaponData.magazineCapacity;
        this.totalReserveAmmo = weaponData.totalReserveAmmo;
        this.weaponData = weaponData;
    }

    #region Spread Methods:
         public Vector3 ApplyBulletSpread(Vector3 originalDirection){

        UpdateSpread();

        float randomizeValue = UnityEngine.Random.Range(-currentBulletSpread, currentBulletSpread);
        Quaternion spreadRotation = Quaternion.Euler(randomizeValue, randomizeValue, randomizeValue);
        return spreadRotation * originalDirection;
    }


    private void IncreaseSpread(){
        currentBulletSpread = Mathf.Clamp(currentBulletSpread + bulletSpreadIncreaseRate,baseBulletSpread, maximumBulletSpread);

    }

    private void UpdateSpread(){

        if(Time.time > lastBulletSpreadTime + spreadCooldown)
            currentBulletSpread = baseBulletSpread;
        else
            IncreaseSpread();

        
        lastBulletSpreadTime = Time.time;
    }
    #endregion
   




   #region  BurstMethod
   public bool BurstActive(){
    // Hard code for shotgun type:
    if(weaponType == WeaponType.ShotGun){
        burstFireDelay = 0f;
        baseBulletSpread = 15f;
        maximumBulletSpread = 15f;
        fireRate = 2f;
        bulletsPerShot = 6;
        return true;
    }
    return burstActive;
   }

   public void ToggleBurstMode(){
    if(burstAvailable == false)
        return;
    burstActive = !burstActive;
    
    if(burstActive){
        fireRate = burstFireRate;
        bulletsPerShot = burstBulletsPerShot;
    }else{
        fireRate = defaultFireRate;
        bulletsPerShot = 1;
    }
   }
   #endregion
    public bool CanShoot() => HaveEnoughBullet() && ReadyToFire();

    private bool ReadyToFire(){
        if(Time.time > lastShootTime + 1/fireRate){
            lastShootTime = Time.time;
            return true;
        }
        return false;
    }


#region  Reload method:

 private bool HaveEnoughBullet() => bulletsInMagazine > 0f;

    public bool CanReload(){

        if(bulletsInMagazine == magazineCapacity){
            return false;
        }
        if(totalReserveAmmo > 0){
            return true;
        }
        return false;
    }

    public void RefillBulles(){

        totalReserveAmmo += bulletsInMagazine;
        int bulletsToReload = magazineCapacity;
        if(bulletsToReload > totalReserveAmmo){
            bulletsToReload = totalReserveAmmo;
        }
        totalReserveAmmo -= bulletsToReload;
        bulletsInMagazine = bulletsToReload;

        if(totalReserveAmmo < 0){
            totalReserveAmmo = 0;
        }
    }
    
#endregion
   
}
