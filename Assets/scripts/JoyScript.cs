using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class JoyScript : MonoBehaviour, IDragHandler, IPointerUpHandler, IPointerDownHandler {

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
            pos.y = (pos.y / bgImg.rectTransform.sizeDelta.y);

            input = new Vector3(pos.x * 2 + 1, 0, pos.y * 2 - 1);
            input = (input.magnitude > 1.0f) ? input.normalized : input;

            joystick.rectTransform.anchoredPosition = new Vector3(input.x * (bgImg.rectTransform.sizeDelta.x / 2) , (input.z *( bgImg.rectTransform.sizeDelta.y / 2)));


        }
    }

    public virtual void onPointerDown(PointerEventData ped)
    {
        onDrag(ped);
    }

    public virtual void onPointerUp(PointerEventData ped)
    {

    }

    public void OnDrag(PointerEventData eventData)
    {
        throw new System.NotImplementedException();
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        throw new System.NotImplementedException();
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        throw new System.NotImplementedException();
    }
}
