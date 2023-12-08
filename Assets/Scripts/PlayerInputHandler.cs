using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInputHandler : MonoBehaviour{
    [SerializeField] private Faction faction;
    [SerializeField] private EventViewer eventViewer;

    private void Start() {
        eventViewer = GetComponent<EventViewer>();
        faction = transform.parent.GetComponentInChildren<Faction>();
    }

    private void FixedUpdate() {
        bool basicAbilityInput = Input.GetMouseButton(0);
        bool jumpInput = Input.GetKey(KeyCode.Space);
        bool activeAbility1Input = Input.GetKey(KeyCode.Q);
        bool activeAbility2Input = Input.GetKey(KeyCode.E);

        if(basicAbilityInput){
            eventViewer.eventsBeingPerformed.Add(faction.basicAttack.abilityName);
        }
        if(jumpInput){
            eventViewer.eventsBeingPerformed.Add(faction.jumpAbility.abilityName);
        }
        if(activeAbility1Input){
            eventViewer.eventsBeingPerformed.Add(faction.activeAbility1.abilityName);
        }
        if(activeAbility2Input){
            eventViewer.eventsBeingPerformed.Add(faction.activeAbility2.abilityName);
        }
        
        faction.basicAttack.UseAbility(basicAbilityInput);
        faction.jumpAbility.UseAbility(jumpInput);
        faction.activeAbility1.UseAbility(activeAbility1Input);
        faction.activeAbility2.UseAbility(activeAbility2Input);
        
    }
}
