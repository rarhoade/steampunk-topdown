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
            UpdateLegSprites(item.SpriteSet[1], item.SpriteSet[2]);
		}
		else if((int)item.EquipmentType == 3){
            //gloves
            UpdateLegSprites(item.SpriteSet[0], item.SpriteSet[0]);
		}
    }

    public void UnequipItem(EquippableItem item){
        switch((int)item.EquipmentType){
            case 1:
                //chest
                UpdateUpperBodySprite(null);
                break;
            case 2:
                UpdateLowerBodySprite();
                UpdateLegSprites();
                break;
            case 3:
                //gloves
                UpdateUpperBodySprite(null);
                break;
            default:
                break;
        }
    }
    public void UpdateUpperBodySprite(Sprite sprite){
        UpperBody.sprite = sprite;
    }

    public void UpdateLowerBodySprite(Sprite sprite=null){
        LowerBody.sprite = sprite;
    }

    public void UpdateLegSprites(Sprite rightLeg=null, Sprite leftLeg=null){
        LeftLeg.sprite = rightLeg;
        RightLeg.sprite = leftLeg;
    }
}
