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
            Vector3 direction = new Vector3 (
                mousePosition.x - transform.position.x,
                mousePosition.y - transform.position.y,
                0
            );

            transform.up = direction;
        }
    }

    public void SetFrozen(bool frozenValue){
        canRotate = frozenValue;
    }
}
