using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class JoystickScript : MonoBehaviour, IDragHandler, IPointerUpHandler, IPointerDownHandler {

    private Image bgImg;
    private Image joystick;
    private Vector3 input;

    private void start()
    {
        bgImg = GetComponent<Image>();
        joystick = transform.GetChild(0).GetComponent<Image>();

    }

    public virtual void onDrag(PointerEventData ped)
    {
        Vector2 pos;
        if(RectTransformUtility.ScreenPointToLocalPointInRectangle(bgImg.rectTransform, ped.position, ped.pressEventCamera, out pos))
        {
            pos.x = (pos.x / bgImg.rectTransform.sizeDelta.x);
        }
    }

    public virtual void onPointerDown(PointerEventData ped)
    {
        onDrag(ped);
    }

    public virtual void onPointerUp(PointerEventData ped)
    {

    }

}
