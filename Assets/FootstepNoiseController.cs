using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FootstepNoiseController : MonoBehaviour
{
    //used by leg animator to create "footstep" noises in sync with animation
    public LayerMask soundLayerMask;
    public float footstepRadius = 1f;
    public float AwarenessIncrement = 3f;

    public void FootStepNoise(){
        Collider2D[] soundColliders; 
        soundColliders = Physics2D.OverlapCircleAll(transform.position, footstepRadius, soundLayerMask);
        foreach(var soundCollider in soundColliders){
            var comp = soundCollider.gameObject.GetComponent<WaypointWander>();
            if(comp)
                comp.AddAwareness(AwarenessIncrement, this.transform);
        }

    }
}
