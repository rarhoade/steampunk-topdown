using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class UISlot : MonoBehaviour{

    protected bool isPointerOver;
    [SerializeField] protected Color normalColor = Color.white;
	[SerializeField] protected Color disabledColor = new Color(1, 1, 1, 0);

}