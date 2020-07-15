using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class UsableSkillSlot : BaseSkillSlot, IBeginDragHandler, IEndDragHandler, IDragHandler, IDropHandler 
{
    public event Action<BaseSkillSlot> OnBeginDragEvent;
	public event Action<BaseSkillSlot> OnEndDragEvent;
	public event Action<BaseSkillSlot> OnDragEvent;
	public event Action<BaseSkillSlot> OnDropEvent;

	protected override void OnDisable()
	{
		base.OnDisable();
		if(isDragging){
			OnEndDrag(null);
		}
	}
	public void OnBeginDrag(PointerEventData eventData)
	{
		isDragging = true;

		if (Skill != null)
			image.color = dragColor;

		if (OnBeginDragEvent != null)
			OnBeginDragEvent(this);
	}

	public void OnEndDrag(PointerEventData eventData)
	{
		isDragging = false;

		if (Skill != null)
			image.color = normalColor;

		if (OnEndDragEvent != null)
			OnEndDragEvent(this);
	}

	public void OnDrag(PointerEventData eventData)
	{
		if (OnDragEvent != null)
			OnDragEvent(this);
	}

	public void OnDrop(PointerEventData eventData)
	{
		if (OnDropEvent != null)
			OnDropEvent(this);
	}
}
