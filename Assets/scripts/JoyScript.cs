using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class JoyScript : MonoBehaviour
{

    private Image bgImg;
    private Image joystick;
    private Vector3 input;
    public Camera Cam;

    private bool clicked;

    private float joyx;
    private float joyy;

    private float fullr;

    public bool Up;
    public bool Right;
    public bool Down;
    public bool Left;


    private void Start()
    {
        bgImg = GetComponent<Image>();
        joystick = transform.GetChild(0).GetComponent<Image>();

        RectTransform rt = (RectTransform)bgImg.transform;

        fullr = rt.rect.height/30;

        joyx = joystick.transform.position.x;
        joyy = joystick.transform.position.y;
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0) && Input.mousePosition.x < 350 && Input.mousePosition.y < 300)
        {
            clicked = true;
        }
        if (Input.GetMouseButton(0) && clicked)
        {
            Vector2 pos;
            if (RectTransformUtility.ScreenPointToLocalPointInRectangle(bgImg.rectTransform, Input.mousePosition, Cam, out pos))
            {
                pos.x = (pos.x / bgImg.rectTransform.sizeDelta.x);
                pos.y = (pos.y / bgImg.rectTransform.sizeDelta.y);

                input = new Vector3(pos.x * 2, 0, pos.y * 2);
                input = (input.magnitude > 1.0f) ? input.normalized : input;

                joystick.rectTransform.anchoredPosition = new Vector3(input.x * (bgImg.rectTransform.sizeDelta.x / 2), (input.z * (bgImg.rectTransform.sizeDelta.y / 2)));

                float y = (joystick.transform.position.y - joyy);
                float x = (joystick.transform.position.x - joyx);

                float angle = (Mathf.Atan2(y, x)/Mathf.PI)*180;
                float radius = Mathf.Sqrt((y * y) + (x * x));

                Dir(angle,radius);

            }
        }
        else if (Input.GetMouseButtonUp(0))
        {
            joystick.rectTransform.anchoredPosition = new Vector3(0, 0);
            clicked = false;
            Left = false;
            Right = false;
            Up = false;
            Down = false;
        }
    }

    public void Dir(float a, float r)
    {
        if (r >= fullr / 3)
        {
            if (a > 112.5 || a < -112.5)
            {
                Left = true;
            }
            else
            {
                Left = false;
            }

            if (a < 67.5 && a > -67.5)
            {
                Right = true;
            }
            else
            {
                Right = false;
            }

            if (a < 157.5 && a > 22.5)
            {
                Up = true;
            }
            else
            {
                Up = false;
            }

            if (a > -157.5 && a < -22.5)
            {
                Down = true;
            }
            else
            {
                Down = false;
            }
        }
        else
        {
            Left = false;
            Right = false;
            Up = false;
            Down = false;
        }
    }
}
