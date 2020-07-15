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
    public SpriteRenderer LeftHand;
    public SpriteRenderer RightHand;
    public SpriteRenderer IdleRightHand;
    public SpriteRenderer IdleLeftHand;
    

    // Update is called once per frame
    public void EquipItem(EquippableItem item){
        switch((int)item.EquipmentType){
            case 0:
                //helmet
                break;
            case 1:
                //chest
                UpdateUpperBodySprite(item.SpriteSet[0]);
                break;
            case 2:
                //bottom
                UpdateLowerBodySprite(item.SpriteSet[0]);
                UpdateLegSprites(item.SpriteSet[1], item.SpriteSet[2]);
                break;
            case 3:
                //gloves
                UpdateHandSprite(item.SpriteSet[0], item.SpriteSet[1]);
                break;
            case 4:
                //boots
                break;
            default:
                break;
        }
    }

    public void UnequipItem(EquippableItem item){
        switch((int)item.EquipmentType){
            case 0:
                //helmet
            case 1:
                //chest
                UpdateUpperBodySprite();
                break;
            case 2:
                //bottoms
                UpdateLowerBodySprite();
                UpdateLegSprites();
                break;
            case 3:
                //gloves
                UpdateHandSprite();
                break;
            case 4:
                //boots
            default:
                break;
        }
    }

    public void UpdateHandSprite(Sprite lh=null, Sprite rh=null){
        LeftHand.sprite = lh;
        IdleLeftHand.sprite = lh;
        RightHand.sprite = rh;
        IdleRightHand.sprite = rh;
    }

    public void UpdateUpperBodySprite(Sprite sprite=null){
        UpperBody.sprite = sprite;
    }

    public void UpdateLowerBodySprite(Sprite sprite=null){
        LowerBody.sprite = sprite;
    }

    public void UpdateLegSprites(Sprite rightLeg=null, Sprite leftLeg=null){
        LeftLeg.sprite = rightLeg;
        RightLeg.sprite = leftLeg;
    }
    
    public void SetHandSprites(SpriteRenderer[] sprites){
        RightHand = sprites[0];
        LeftHand = sprites[1];
        RightHand.sprite = IdleRightHand.sprite;
        LeftHand.sprite = IdleLeftHand.sprite;
    }
}
