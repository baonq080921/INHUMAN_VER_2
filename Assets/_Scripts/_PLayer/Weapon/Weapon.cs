
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
    public float fireRate = 1f;
    private float lastShootTime;

    [Header("Magazine details")]
    public int bulletsInMagazine;
    public int magazineCapacity;
    public int totalReserveAmmo;

    [Range(1f, 2f)]
    public float reloadSpeed = 1f; // Make the reload speed on different gun change 
    [Range(1f,2f)]
    public float equipmentSpeed = 1f; // Make the equip speed on different gun change
   
    

    

    public bool CanShoot()
    {
        if(HaveEnoughBullet() && ReadyToFire()){
            bulletsInMagazine--;
            return true;
        }
        return false;
    }



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
