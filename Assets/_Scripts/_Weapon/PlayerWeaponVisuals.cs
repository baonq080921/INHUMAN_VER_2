using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Animations.Rigging;

public class PlayerWeaponVisuals : MonoBehaviour
{
    private Animator anim;
    private Player player;


    #region Gun transforms region

    [SerializeField] private WeaponModel[] weaponModels;
    [SerializeField] private BackupWeaponModel[] backupWeaponModels;


    [SerializeField] private Transform[] gunTransforms;

    [SerializeField] private Transform pistol;
    [SerializeField] private Transform revolver;
    [SerializeField] private Transform autoRifle;
    [SerializeField] private Transform shotgun;
    [SerializeField] private Transform rifle;

    private Transform currentGun;
    #endregion

    [Header("Rig ")]
    [SerializeField] private float rigWeightIncreaseRate;
    private bool shouldIncrease_RigWeight;
    private Rig rig;

    [Header("Left hand IK")]
    [SerializeField] private float leftHandIkWeightIncreaseRate;
    [SerializeField] private TwoBoneIKConstraint leftHandIK;
    [SerializeField] private Transform leftHandIK_Target;
    private bool shouldIncrease_LeftHandIKWieght;



    private void Start()
    {
        anim = GetComponentInChildren<Animator>();
        rig = GetComponentInChildren<Rig>();
        player = GetComponent<Player>();
        weaponModels = GetComponentsInChildren<WeaponModel>(true);
        backupWeaponModels = GetComponentsInChildren<BackupWeaponModel>(true);

    }

    private void Update()
    {

        UpdateRigWigth();
        UpdateLeftHandIKWeight();
    }



    public void PlayFireAnimation() => anim.SetTrigger("Fire");
    public void PlayReloadAnimation()
    {

        float reloadSpeed = player.weapon.CurrentWeapon().reloadSpeed;
        anim.SetTrigger("Reload");
        anim.SetFloat("ReloadSpeed",reloadSpeed);
        ReduceRigWeight();
    }


    public WeaponModel CurrentWeaponModel(){
        WeaponModel weaponModel = null;
        WeaponType weaponType = player.weapon.CurrentWeapon().weaponType;
        for(int i = 0 ; i < weaponModels.Length; i++){
            if(weaponModels[i].weaponType == weaponType){
                weaponModel = weaponModels[i];
            }
        }
        return weaponModel;
    }


    
    public void PlayWeaponEquipAnimation()
    {

        EquipType equipType = CurrentWeaponModel().equipAnimationType;
        float equipmentSpeed = player.weapon.CurrentWeapon().equipmentSpeed;
        leftHandIK.weight = 0;
        ReduceRigWeight();
        anim.SetFloat("EquipType", ((float)equipType));
        anim.SetTrigger("EquipWeapon");
        anim.SetFloat("EquipSpeed",equipmentSpeed);

    }
   



    //Switch on/off current weapon model player is currently holding
    public void SwitchOnCurrentWeaponModel()
    {
        int animationIndex = ((int)CurrentWeaponModel().holdType);
        SwitchOffBackupWeaponModels();

        if(!player.weapon.HasOnlyOneWeapon()){
            SwitchOnBackupWeaponModels();
        }

        SwitchOffWeaponModels();

        SwitchAnimationLayer(animationIndex);
        CurrentWeaponModel().gameObject.SetActive(true);
        AttachLeftHand();
    }
    public void SwitchOffWeaponModels()
    {
        for (int i = 0; i < weaponModels.Length; i++)
        {
            weaponModels[i].gameObject.SetActive(false);
        }
    }


    // Switch on and off backup weapon models
    public void SwitchOnBackupWeaponModels(){
        WeaponType weaponType = player.weapon.BackUpWeapon().weaponType;
        foreach(BackupWeaponModel backupWeaponModel in backupWeaponModels){
            if(backupWeaponModel.weaponType == weaponType){
                backupWeaponModel.gameObject.SetActive(true);
            }
        }
    }
    private void SwitchOffBackupWeaponModels(){
        foreach(BackupWeaponModel backupWeaponModel in backupWeaponModels){
            backupWeaponModel.gameObject.SetActive(false);
        }
    }
    private void SwitchAnimationLayer(int layerIndex)
    {
        for (int i = 1; i < anim.layerCount; i++)
        {
            anim.SetLayerWeight(i, 0);
        }

        anim.SetLayerWeight(layerIndex, 1);
    }



#region Animation Region


private void AttachLeftHand()
    {
        Transform targetTransform = CurrentWeaponModel().holdPoint;

        leftHandIK_Target.localPosition = targetTransform.localPosition;
        leftHandIK_Target.localRotation = targetTransform.localRotation;
    }
private void UpdateLeftHandIKWeight()
    {
        if (shouldIncrease_LeftHandIKWieght)
        {
            leftHandIK.weight += leftHandIkWeightIncreaseRate * Time.deltaTime;

            if (leftHandIK.weight >= 1)
                shouldIncrease_LeftHandIKWieght = false;
        }
    }
    private void UpdateRigWigth()
    {
        if (shouldIncrease_RigWeight)
        {
            rig.weight += rigWeightIncreaseRate * Time.deltaTime;

            if (rig.weight >= 1)
                shouldIncrease_RigWeight = false;
        }
    }
    private void ReduceRigWeight()
    {
        rig.weight = .15f;
    }



    public void MaximizeRigWeight() => shouldIncrease_RigWeight = true;
    public void MaximizeLeftHandWeight() => shouldIncrease_LeftHandIKWieght = true;

#endregion

   
}

