
public enum WeaponType {
    Pistol,
    Revolver,
    AutoRiffle,
    ShotGun,
    Sniper
}

[System.Serializable] // make a class visible on the inspector

public class Weapon
{
    public WeaponType weaponType;
    public int bulletsInMagazine;
    public int magazineCapacity;
    public int totalReserveAmmo;


    public bool CanShoot()
    {
        return HaveEnoughBullet();
    }

    private bool HaveEnoughBullet()
    {
        if (bulletsInMagazine > 0)
        {
            bulletsInMagazine--;
            return true;
        }
        return false;
    }

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
}
