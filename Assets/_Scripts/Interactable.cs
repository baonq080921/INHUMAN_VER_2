using Unity.VisualScripting;
using UnityEngine;

public class Interactable : MonoBehaviour
{

    protected PlayerWeaponController weaponController;

    protected MeshRenderer mesh;
    [SerializeField] private Material highlightMaterial;
    protected Material defaultMaterial;

    void Start()
    {
        if(mesh == null){
            mesh = GetComponentInChildren<MeshRenderer>();
        }

        defaultMaterial = mesh.material;
    }

    void Update()
    {

    }

    public void HighlightActive(bool active){
        if(active){
            mesh.material = highlightMaterial;
        }
        else{
            mesh.material = defaultMaterial;
        }
    }

    protected void UpdateMeshAndMaterial(MeshRenderer newMesh){
        mesh = newMesh;
        defaultMaterial = newMesh.material;
    }

    public virtual void  Interaction(){
        Debug.Log("Interacted with "+  gameObject.name);
    }
     protected virtual void OnTriggerEnter(Collider other)
    {

        if(weaponController == null){
            weaponController = other.GetComponent<PlayerWeaponController>();
        }
        PlayerInteraction playerInteraction = other.GetComponent<PlayerInteraction>();

        if(playerInteraction == null){
            return;
        }

        playerInteraction.GetInteractables().Add(this);
        playerInteraction.UpdateClosetInteractable();
    }

    protected virtual void OnTriggerExit(Collider other)
    {
        PlayerInteraction playerInteraction = other.GetComponent<PlayerInteraction>();

        if(playerInteraction == null){
            return;
        }
        playerInteraction.GetInteractables().Remove(this);
        playerInteraction.UpdateClosetInteractable();

    }
}
