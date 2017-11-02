using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
public class ClickToMove : MonoBehaviour
{
    public HeroController heroController;
    public void OnPointClick(BaseEventData eventData)
    {
        PointerEventData pData = (PointerEventData)eventData;
       // Debug.Log(pData.pointerCurrentRaycast.worldPosition);
        heroController.Move(pData.pointerCurrentRaycast.worldPosition);
    }
}
