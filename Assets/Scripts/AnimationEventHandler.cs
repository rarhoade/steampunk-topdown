using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationEventHandler : MonoBehaviour
{
    // Start is called before the first frame update
    public RotateToCursor weaponController;
    public PlayerMovement rootObject;
    public PlayerCombat combatController;
    
    void Awake(){
        InstantiateComponents();
    }

    public void InstantiateComponents(){
        weaponController = GetComponentInParent<RotateToCursor>();
        rootObject = GetComponentInParent<PlayerMovement>();
        combatController = GetComponentInParent<PlayerCombat>();

    }


    public void AttackBeginning(){
        FreezeMovement();
        FreezeWeaponRotation();
        StopAttack();
        HideIdleHands();
    }

    public void AttackEnd(){
        ResumeAttack();
        ResumeWeaponRotation();
        ContinueMovement();
        ShowIdleHands();
    }

    public void StopAttack(){
        combatController.canAttack = false;
    }

    public void HideIdleHands(){
        combatController.IdleHands.SetActive(false);
    }

    public void ShowIdleHands(){
        combatController.IdleHands.SetActive(true);
    }

    public void ResumeAttack(){
        combatController.canAttack = true;
    }

    public void FreezeWeaponRotation(){
        if(weaponController != null){
            weaponController.SetFrozen(false);
        }
    }

    public void ResumeWeaponRotation(){
        if(weaponController != null){
            weaponController.SetFrozen(true);
        }
    }

    public void FreezeMovement(){
        rootObject.canMove = false;
    }

    public void ContinueMovement(){
        rootObject.canMove = true;
    }
}
