using System.Collections.Generic;
using UnityEditor.Rendering.LookDev;
using UnityEngine;

public class PlayerInteraction : MonoBehaviour
{
    private List<Interactable> interactables = new List<Interactable>();
    private Interactable closetInteractable;


    void Start()
    {
        Player player = GetComponent<Player>();
        player.controls.Character.Interaction.performed += Context => InteractWithCloset();
    }



    private void InteractWithCloset(){
        closetInteractable?.Interaction();
        interactables.Remove(closetInteractable);

        UpdateClosetInteractable();
    }
    public void UpdateClosetInteractable(){
        closetInteractable?.HighlightActive(false);


        closetInteractable = null;

        float closetDistance = float.MaxValue;

        foreach(Interactable interactable in interactables){
            float distance = Vector3.Distance(transform.position, interactable.transform.position);

            if(distance < closetDistance){
                closetDistance = distance;
                closetInteractable = interactable;
            }
        }

        closetInteractable?.HighlightActive(true);
    }

    public List<Interactable> GetInteractables() => interactables;
}
