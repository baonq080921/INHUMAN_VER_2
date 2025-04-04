
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
    [Header("Shooting details")]
    public ShootType shootType;
    public int bulletsPerShot; // The number of bullets fired per shot
    public float defaultFireRate; // The default fire rate of the weapon
    public float fireRate = 1f;
    private float lastShootTime;

    [Header("Burst FIre")]

    public bool burstAvailable; // Check if the weapon has burst fire mode
    public bool burstActive; // Check if the weapon is in burst fire mode
    public int burstBulletsPerShot;
    public float burstFireRate;
    public float burstFireDelay = .15f; // The delay between each bullet fired in burst fire mode




    [Header("Magazine details")]
    public int bulletsInMagazine;
    public int magazineCapacity;
    public int totalReserveAmmo;

    [Range(1f, 2f)]
    public float reloadSpeed = 1f; // Make the reload speed on different gun change 
    [Range(1f,2f)]
    public float equipmentSpeed = 1f; // Make the equip speed on different gun change




   
    [Header("Bullet Spread")]
    public float baseBulletSpread = 0.5f; // The base spread of the bullet when shooting
    private float currentBulletSpread = 2f; // The spread of the bullet when shooting
    public float maximumBulletSpread = 5f; // The maximum spread of the bullet when shooting
    public float bulletSpreadIncreaseRate = 0.15f; // The rate at which the bullet spread increases when shooting
    private float lastBulletSpreadTime;
    private float spreadCooldown = 1f; // The time it takes for the bullet spread to reset
    


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
