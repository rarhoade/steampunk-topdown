using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class RotateToCursor : MonoBehaviour
{
    // Start is called before the first frame update
    public bool canRotate;
    void Awake()
    {
       canRotate = true; 
    }

    // Update is called once per frame
    void Update()
    {
        if(canRotate){
            Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            //Debug.Log(mousePosition);
            Vector2 direction = new Vector2 (
                mousePosition.x - transform.position.x,
                mousePosition.y - transform.position.y
            );

            transform.up = direction;
            /*if(direction.x > 0){
                transform.eulerAngles = new Vector3(
                    0f,
                    180f,
                    Quaternion.Inverse(transform.rotation).eulerAngles.z
                );
            }
            else{
                transform.eulerAngles = new Vector3(
                    0f,
                    0f,
                    transform.rotation.eulerAngles.z
                );
            }*/
        }
    }

    public void SetFrozen(bool frozenValue){
        canRotate = frozenValue;
    }
}
