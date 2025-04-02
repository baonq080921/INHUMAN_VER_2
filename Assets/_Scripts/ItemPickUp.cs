using UnityEngine;

public class ItemPickUp : MonoBehaviour
{

    [SerializeField] private Weapon weapon;
    private void OnTriggerEnter(Collider other)
    {
        other.GetComponent<PlayerWeaponController>()?.PickupWeapon(weapon);   
    }
}
