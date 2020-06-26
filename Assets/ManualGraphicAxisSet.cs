using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManualGraphicAxisSet : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        GetComponent<Camera>().transparencySortMode = TransparencySortMode.CustomAxis;
        GetComponent<Camera>().transparencySortAxis = new Vector3(0, 1, 0);
    }

}
