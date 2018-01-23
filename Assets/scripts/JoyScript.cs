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
    private int rawState;
    public int state;


    private void Start()
    {
        bgImg = GetComponent<Image>();
        joystick = transform.GetChild(0).GetComponent<Image>();

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

                float angle = Mathf.Atan((Input.mousePosition.y + 128.5f)/ (Input.mousePosition.x + 303.6f));

                Debug.Log(angle);

                for (int i = 0; i<8; i++)
                {
                    if(angle >= i*45 && angle <= i * 45)
                    {
                        rawState = i;
                        break;
                    }
                }

            }
        }
        else if (Input.GetMouseButtonUp(0))
        {
            joystick.rectTransform.anchoredPosition = new Vector3(0, 0);
            clicked = false;
        }
    }
}
