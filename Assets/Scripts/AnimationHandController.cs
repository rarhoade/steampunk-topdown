using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationHandController : MonoBehaviour
{
    private WeaponController weaponController;
    [SerializeField] SpriteRenderer[] EquipSlotHandSprites;
    // Start is called before the first frame update
    void Start()
    {
        weaponController = gameObject.transform.parent.GetComponentInParent<WeaponController>();
        weaponController.RecieveHandRenderers(EquipSlotHandSprites);
    }
}
