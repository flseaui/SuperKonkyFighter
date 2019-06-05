using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class JoyScript : MonoBehaviour
    {

        private Image _bgImg;
        private Image _joystick;
        private Vector3 _input;
        public Camera Cam;
        public Animator Anim;

        private bool _clicked;

        public float Ijoyx;
        public float Ijoyy;

        public float Joyx;
        public float Joyy;

        private float _inCamx;

        private float _fullr;

        public bool Up;
        public bool Right;
        public bool Down;
        public bool Left;


        private void Start()
        {
            _bgImg = GetComponent<Image>();
            _bgImg.enabled = false;
            _joystick = transform.GetChild(0).GetComponent<Image>();
            _joystick.enabled = false;

            RectTransform rt = (RectTransform)_bgImg.transform;

            _fullr = rt.rect.height/30;

            Ijoyx = _joystick.transform.position.x;
            Ijoyy = _joystick.transform.position.y;

            _inCamx = Cam.transform.position.x;
        }

        private void Update()
        {
            Joyx = Ijoyx + (_inCamx + Cam.transform.position.x);
            Joyy = Ijoyy;

            if (Input.GetMouseButtonDown(0) && Input.mousePosition.x < 350 && Input.mousePosition.y < 300)
            {
                _clicked = true;
            }
            if (Input.GetMouseButton(0) && _clicked)
            {
                Vector2 pos;
                if (RectTransformUtility.ScreenPointToLocalPointInRectangle(_bgImg.rectTransform, Input.mousePosition, Cam, out pos))
                {
                    pos.x = (pos.x / _bgImg.rectTransform.sizeDelta.x);
                    pos.y = (pos.y / _bgImg.rectTransform.sizeDelta.y);

                    _input = new Vector3(pos.x * 2, 0, pos.y * 2);
                    _input = (_input.magnitude > 1.0f) ? _input.normalized : _input;

                    _joystick.rectTransform.anchoredPosition = new Vector3(_input.x * (_bgImg.rectTransform.sizeDelta.x / 2), (_input.z * (_bgImg.rectTransform.sizeDelta.y / 2)));

                    float y = (_joystick.transform.position.y - Joyy);
                    float x = (_joystick.transform.position.x - Joyx);

                    float angle = (Mathf.Atan2(y, x)/Mathf.PI)*180;
                    float radius = Mathf.Sqrt((y * y) + (x * x));

                    Dir(angle,radius);

                }
            }
            else if (Input.GetMouseButtonUp(0))
            {
                _joystick.rectTransform.anchoredPosition = new Vector3(0, 0);
                _clicked = false;
                Left = false;
                Anim.SetBool("Left", false);
                Right = false;
                Anim.SetBool("Right", false);
                Up = false;
                Anim.SetBool("Up", false);
                Down = false;
                Anim.SetBool("Down", false);
            }
        }

        public void Dir(float a, float r)
        {
            if (r >= _fullr / 3)
            {
                if (a > 112.5 || a < -112.5)
                {
                    Left = true;
                    Anim.SetBool("Left", true);
                }
                else
                {
                    Left = false;
                    Anim.SetBool("Left", false);
                }

                if (a < 67.5 && a > -67.5)
                {
                    Right = true;
                    Anim.SetBool("Right", true);
                }
                else
                {
                    Right = false;
                    Anim.SetBool("Right", false);
                }

                if (a < 157.5 && a > 22.5)
                {
                    Up = true;
                    Anim.SetBool("Up", true);
                }
                else
                {
                    Up = false;
                    Anim.SetBool("Up", false);
                }

                if (a > -157.5 && a < -22.5)
                {
                    Down = true;
                    Anim.SetBool("Down", true);
                }
                else
                {
                    Down = false;
                    Anim.SetBool("Down", false);
                }
            }
            else
            {
                Left = false;
                Anim.SetBool("Left", false);
                Right = false;
                Anim.SetBool("Right", false);
                Up = false;
                Anim.SetBool("Up", false);
                Down = false;
                Anim.SetBool("Down", false);
            }
        }
    }
}
