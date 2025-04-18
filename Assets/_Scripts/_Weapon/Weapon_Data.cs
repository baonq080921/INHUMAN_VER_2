using UnityEngine;
using UnityEngine.Rendering.Universal;



[CreateAssetMenu(fileName ="New Weapon Data", menuName ="Weapon System/Weapon Data")]
public class Weapon_Data : ScriptableObject
{
    public string weaponName;
    public ShootType shootType;
    [Header("Regular Shot")]
    public float fireRate;
    public int bulletsPerShot = 1;
    public float defaultFireRate;


    [Header("Burst Shot")]
    public bool burstActive;
    public bool burstAvailable;
    public int burstBulletsPerShot;
    public float burstFireRate;
    public float burstFireDelay = .15f;

    [Header("Magazine details")]
    public int bulletsInMagazine;
    public int magazineCapacity;
    public int totalReserveAmmo;



    [Header("Weapon Spread")]
    public float baseBulletSpread;
    public float maximumBulletSpread;
    public float bulletSpreadIncreaseRate =.15f ;

    [Header("Weapon Generic")]
    public WeaponType weaponType;

    [Range(1f, 3f)]
    public float reloadSpeed = 1f ;
    [Range(1f,3f)]
    public float equipmentSpeed = 1f;
    [Range(4f,8f)]
    public float gunDistance =4f;
    [Range(4f,8f)]
    public float cameraDistance = 6f;



}
