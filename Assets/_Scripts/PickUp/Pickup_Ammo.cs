using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
    public struct AmotData{

        public WeaponType weaponType;
        [Range(10,50)]
        public int minAmmout;
        [Range(10,100)]
        public int maxAmmount;
    }
public enum AmmoBoxType {smallBox, bigBox};
public class Pickup_Ammo : Interactable
{
    [SerializeField] private AmmoBoxType boxType;

    
    
    [SerializeField] List<AmotData> smallBoxAmmo;
    [SerializeField] List<AmotData> bigBoxAmmo;

    [SerializeField] private GameObject[] boxModel;

    void Start()
    {
        SetupBoxModel();
    }

    private void SetupBoxModel()
    {
        for (int i = 0; i < boxModel.Length; i++)
        {
            boxModel[i].SetActive(false);
            if (i == ((int)boxType))
            {
                boxModel[i].SetActive(true);
                UpdateMeshAndMaterial(boxModel[i].GetComponent<MeshRenderer>());
            }
        }
    }

    private void AddBulletsToWeapon(Weapon weapon, int ammout){
        if(weapon == null) 
            return;
        weapon.totalReserveAmmo += ammout;
    }

    private int GetBulletAmount(AmotData amotData){

        float min = Mathf.Min(amotData.minAmmout,amotData.maxAmmount);
        float max = Mathf.Max(amotData.minAmmout, amotData.maxAmmount);
        float randomAmmoAmount = Random.Range(min, max);
        return Mathf.RoundToInt(randomAmmoAmount);
    }

    public override void Interaction(){
        List<AmotData> currentAmmoList = smallBoxAmmo;

        if(boxType == AmmoBoxType.bigBox){
            currentAmmoList = bigBoxAmmo;
        }
        foreach(AmotData ammo in currentAmmoList){
            Weapon weapon = weaponController.WeaponInSlots(ammo.weaponType);
            AddBulletsToWeapon(weapon, GetBulletAmount(ammo));
        }

        ObjectPool.instance.ReturnToObject(gameObject);
    }

}
