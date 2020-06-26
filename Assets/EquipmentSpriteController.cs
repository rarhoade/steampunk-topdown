using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EquipmentSpriteController : MonoBehaviour
{
    // Start is called before the first frame update
    public SpriteRenderer UpperBody;
    public SpriteRenderer LowerBody;
    public SpriteRenderer RightLeg;
    public SpriteRenderer LeftLeg;
    

    // Update is called once per frame
    public void EquipItem(EquippableItem item){
        if((int)item.EquipmentType == 0){
            //equip helmet
		}
		else if((int)item.EquipmentType == 1){
            //equip chest
            UpdateUpperBodySprite(item.SpriteSet[0]);
		}
		else if((int)item.EquipmentType == 2){
            //lower
            UpdateLowerBodySprite(item.SpriteSet[0]);
		}
		else if((int)item.EquipmentType == 3){
            //boots/legs
            UpdateLegSprites(item.SpriteSet[0]);
		}
    }

    public void UnequipItem(EquippableItem item){
        switch((int)item.EquipmentType){
            case 1:
                UpdateUpperBodySprite(null);
                break;
            case 3:
                UpdateUpperBodySprite(null);
                break;
            default:
                break;
        }
    }
    public void UpdateUpperBodySprite(Sprite sprite){
        UpperBody.sprite = sprite;
    }

    public void UpdateLowerBodySprite(Sprite sprite){
        LowerBody.sprite = sprite;
    }

    public void UpdateLegSprites(Sprite Leg){
        LeftLeg.sprite = Leg;
        RightLeg.sprite = Leg;
    }
}
