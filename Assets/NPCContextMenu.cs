using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCContextMenu : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject UIElements;

    Ray ray;
    RaycastHit2D hit;
    public CircleCollider2D NPCDetector;
    public Transform MenuTarget;
    private bool canOpenMenu = false;

    public void TogglePanel(){
        if(UIElements.activeSelf == true){
            UIElements.SetActive(false);
        }
        else{
            UIElements.SetActive(true);
        }
    }
    // Update is called once per frame
    void Update()
    {
        if(canOpenMenu && Input.GetKeyDown(KeyCode.E)){
            //Code for moving it to game objects position;
            /*Vector2 viewportPoint = Camera.main.WorldToScreenPoint(MenuTarget.position);
            viewportPoint.y -= MenuTarget.lossyScale.y * 100;
            UIElements.transform.position = viewportPoint;*/
            TogglePanel(); 
        }
    }

    void OnTriggerEnter2D(Collider2D other){
        MenuTarget = other.transform;
        if(other.gameObject.tag == "NPC"){
            canOpenMenu = true;
        }
    }
    
    void OnTriggerExit2D(Collider2D other){
        MenuTarget = null;
        canOpenMenu = false;
        UIElements.SetActive(false);
    }
}
