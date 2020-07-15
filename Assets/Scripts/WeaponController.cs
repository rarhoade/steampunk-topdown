using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponController : MonoBehaviour
{
    [SerializeField] GameObject DefaultPrimaryWeapon;
    [SerializeField] GameObject DefaultSecondaryWeapon;
    [SerializeField] EquipmentSpriteController equipmentSpriteController;
    public GameObject PrimaryWeapon;
    public GameObject SecondaryWeapon;
    public PlayerCombat combatControl;

    void Awake(){
        if(!PrimaryWeapon){
            EquipPrimaryWeapon(DefaultPrimaryWeapon);
        }
    }
    public void EquipPrimaryWeapon(GameObject wep){
        if(PrimaryWeapon)
            Destroy(PrimaryWeapon);
        PrimaryWeapon = Instantiate(wep, transform);
        PrimaryWeapon.transform.position = transform.position;
        combatControl.SetAnimator(PrimaryWeapon.GetComponent<Animator>());
        PrimaryWeapon.GetComponent<AnimationEventHandler>().InstantiateComponents();
    }

    

    public void EquipSecondaryWeapon(GameObject wep){
        if(SecondaryWeapon)
            Destroy(SecondaryWeapon);
        SecondaryWeapon = Instantiate(wep);
        SecondaryWeapon.transform.position = transform.position;
        SecondaryWeapon.transform.parent = transform;
        /* need to change for adding secondary weapon support
        combatControl.SetAnimator(SecondaryWeapon.GetComponent<Animator>());
        SecondaryWeapon.GetComponent<AnimationEventHandler>().InstantiateComponents(); */
    }

    public void UnequipPrimaryWeapon(){
        if(PrimaryWeapon)
            Destroy(PrimaryWeapon);
        EquipPrimaryWeapon(DefaultPrimaryWeapon);
    }

    public void UnequipSecondaryWeapon(){
        if(SecondaryWeapon)
            Destroy(SecondaryWeapon);
        EquipSecondaryWeapon(DefaultSecondaryWeapon);
    }

    public void RecieveHandRenderers(SpriteRenderer[] sprites){
        equipmentSpriteController.SetHandSprites(sprites);
    }
}
