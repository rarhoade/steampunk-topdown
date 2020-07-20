using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingWindowView : MonoBehaviour
{
    public GameObject gameLight;

    private void OnTriggerEnter2D(Collider2D other){
        gameLight.SetActive(true);
    }

    private void OnTriggerStay2D(Collider2D other){
        float lightAngle = Vector2.SignedAngle(
            new Vector2(transform.position.x, transform.position.y),
            new Vector2(other.transform.position.x, other.transform.position.y) 
        );
        lightAngle = (lightAngle * Mathf.Rad2Deg);
        if(lightAngle > -60){
            lightAngle = -60;
        } else if (lightAngle < -120){
            lightAngle = -120;
        }
        gameLight.transform.rotation = Quaternion.Euler(0, 0, lightAngle);
    }

    private void OnTriggerExit2D(Collider2D other){
        gameLight.SetActive(false);
    }
}
