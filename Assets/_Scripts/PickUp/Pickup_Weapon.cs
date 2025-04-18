using UnityEngine;

public class Pickup_Weapon : Interactable
{
    
    [SerializeField] private Weapon_Data weapon_Data;
    [SerializeField] private BackupWeaponModel[] models;
    //Create a variables of the weapon we gonna drop
    [SerializeField] private Weapon weapon;
    private bool oldWeapon;

    void Start()
    {
        SetupGameObject();
        if(oldWeapon == false)
            weapon = new Weapon(weapon_Data);
    }

    public void SetupPickupWeapon(Weapon weapon, Transform transform){
        oldWeapon = true;
        this.weapon = weapon;
        weapon_Data = weapon.weaponData;
        this.transform.position = transform.position;
    }


    [ContextMenu("Update Item Model")]

    public void SetupGameObject(){
        gameObject.name = "Pickup_weapon -" + weapon_Data.weaponType.ToString();
        SetUpWeaponModel();
    }

    private void SetUpWeaponModel(){
        foreach(BackupWeaponModel model in models){
            model.gameObject.SetActive(false);

            if(model.weaponType == weapon_Data.weaponType){
                model.gameObject.SetActive(true);
                UpdateMeshAndMaterial(model.GetComponent<MeshRenderer>());
            }
        }
    }

    public override void Interaction()
    {
        weaponController.PickupWeapon(weapon);
        ObjectPool.instance.ReturnToObject(gameObject);
    }
}
