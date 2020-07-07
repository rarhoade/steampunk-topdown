using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class BaseSkillSlot: UISlot, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
{ 
    [SerializeField] protected Image image;
    protected bool isDragging;
    [SerializeField] protected Color dragColor = new Color(1,1,1, 0.5f);
    public event Action<BaseSkillSlot> OnPointerEnterEvent;
    public event Action<BaseSkillSlot> OnPointerExitEvent;
    public event Action<BaseSkillSlot> OnRightClickEvent;
    protected Skill _skill;
    public Skill Skill {
        get { return _skill;}
        set {
            _skill = value;

            if(_skill == null) {
                image.sprite = null;
                image.color = disabledColor;
            } else {
                image.sprite = _skill.Icon;
                image.color = normalColor;
            }

            if (isPointerOver){
                OnPointerExit(null);
                OnPointerEnter(null);
            }
        }
    } 


    protected virtual void OnValidate() 
    {
        if(image == null)
            image = GetComponent<Image>();
        
        Skill = _skill;

    }

    protected virtual void OnDisable()
    {
        if (isPointerOver) 
            OnPointerExit(null);

    }
    public void OnPointerClick(PointerEventData eventData)
	{
		if (eventData != null && eventData.button == PointerEventData.InputButton.Right)
		{
			if (OnRightClickEvent != null)
				OnRightClickEvent(this);
		}
	}

	public void OnPointerEnter(PointerEventData eventData)
	{
		isPointerOver = true;

		if (OnPointerEnterEvent != null)
			OnPointerEnterEvent(this);
	}

	public void OnPointerExit(PointerEventData eventData)
	{
		isPointerOver = false;

		if (OnPointerExitEvent != null)
			OnPointerExitEvent(this);
	}
}