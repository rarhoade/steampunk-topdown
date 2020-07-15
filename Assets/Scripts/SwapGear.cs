using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwapGear : MonoBehaviour
{
    // Start is called before the first frame update
    private PlayerCombat combatControl;
    public GameObject WeaponSwap;
    public GameObject WeaponContainer;
    void Awake()
    {
        combatControl = GetComponent<PlayerCombat>();
    }

    // Update is called once per frame
    void Update()
    {
       if(Input.GetKeyDown(KeyCode.Space)){
            foreach(Transform weapon in WeaponContainer.transform){
                Destroy(weapon.gameObject);
            }
            GameObject NewWeapon = Instantiate(WeaponSwap);
            NewWeapon.transform.position = WeaponContainer.transform.position;
            NewWeapon.transform.parent = WeaponContainer.transform;
            combatControl.SetAnimator(NewWeapon.GetComponent<Animator>());
            NewWeapon.GetComponent<AnimationEventHandler>().InstantiateComponents();
       } 
    }
}
