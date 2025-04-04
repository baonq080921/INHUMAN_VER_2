using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Player : MonoBehaviour
{
    public PlayerControls controls { get; private set; }
    public PlayerAim aim { get; private set; } 
    public PlayerMovement movement { get; private set; }
    public PlayerWeaponController weapon { get; private set; }
    public PlayerWeaponVisuals weaponVisuals{get; private set;}

    private void Awake()
    {
        controls = new PlayerControls();
        aim = GetComponent<PlayerAim>();
        movement = GetComponent<PlayerMovement>();
        weapon = GetComponent<PlayerWeaponController>();
        weaponVisuals = GetComponent<PlayerWeaponVisuals>();
        StartCoroutine(TestMethod());
    }

    IEnumerator TestMethod(){
        for(int i = 0; i < 10; i++){
            Debug.Log(i);
            yield return new WaitForSeconds(1f);
        }
    }
    private void OnEnable()
    {
        controls.Enable();
    }
    private void OnDisable()
    {
        controls.Disable();
    }


}
