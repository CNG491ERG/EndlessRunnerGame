using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour, IDamageable{
    [Header("Player's Input Handler")]
    public InputHandler inputHandler;

    [Header("Faction Data of the Player")]
    [SerializeField] private FactionSO faction;
    [SerializeField] private GameObject basicAbilityObject;
    [SerializeField] private GameObject activeAbility1Object;
    [SerializeField] private GameObject activeAbility2Object;
    [SerializeField] private GameObject passiveAbilityObject;
    [SerializeField] private GameObject jumpObject;
    private IAbility basicAbility;
    private IAbility activeAbility1;
    private IAbility activeAbility2;
    private IAbility passiveAbility;
    private IAbility jump;

    [Header("IDamageable Values")]
    [SerializeField] private float maxHealth;
    [SerializeField] private float health;
    [SerializeField] private float defense;

    public float Health{ get => health; set => health = value;}
    public float Defense { get => defense; set => defense = value;}
    public float MaxHealth { get => maxHealth; set => maxHealth = value; }

    [Header("Other")]
    [SerializeField] private Boss boss;
    public event EventHandler OnDamageableDeath;
    public event EventHandler OnDamageableHurt;
    public event EventHandler OnDamageableHurtBasic;

    void Start(){
        if(faction.BasicAbility != null){
            basicAbilityObject = Instantiate(faction.BasicAbility, this.transform);
            basicAbility = basicAbilityObject.GetComponent<IAbility>();
        }
        if(faction.ActiveAbility1 != null){
            activeAbility1Object = Instantiate(faction.ActiveAbility1, this.transform);
            activeAbility1 = activeAbility1Object.GetComponent<IAbility>();
        }
        if(faction.ActiveAbility2 != null){
            activeAbility2Object = Instantiate(faction.ActiveAbility2, this.transform);
            activeAbility2 = activeAbility2Object.GetComponent<IAbility>();
        }
        if(faction.PassiveAbility != null){
            passiveAbilityObject = Instantiate(faction.PassiveAbility, this.transform);
            passiveAbility = passiveAbilityObject.GetComponent<IAbility>();
        }
        if(faction.Jump != null){
            jumpObject = Instantiate(faction.Jump, this.transform);
            jump = jumpObject.GetComponent<IAbility>();
        }

        maxHealth = health = faction.MaxHealth;
        defense = faction.Defense;
    }

    private void FixedUpdate() {
        basicAbility?.UseAbility(inputHandler.BasicAbilityInput);
        activeAbility1?.UseAbility(inputHandler.ActiveAbility1Input);
        activeAbility2?.UseAbility(inputHandler.ActiveAbility2Input);
        jump?.UseAbility(inputHandler.JumpInput);
    }

    public void TakeDamage(float damageToTake){
        float totalDamage = damageToTake * (1 - Defense);
        Health = Health - totalDamage <= 0 ? 0 : Health - totalDamage;
        OnDamageableHurt?.Invoke(this, EventArgs.Empty);

        if(Health == 0){
            OnDamageableDeath?.Invoke(this, EventArgs.Empty);
        }
    }

    public void ResetAllCooldowns(){
        //playerFaction.ActiveAbility1.ResetCooldown();
        //playerFaction.ActiveAbility2.ResetCooldown();
        //playerFaction.BasicAttack.ResetCooldown();
    }
}