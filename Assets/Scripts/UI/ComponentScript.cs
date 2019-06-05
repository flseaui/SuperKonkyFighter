using UnityEngine;
using UnityEngine.EventSystems;

namespace UI
{
    public class ComponentScript : MonoBehaviour, IPointerClickHandler, IPointerDownHandler, IPointerEnterHandler, IPointerUpHandler, IPointerExitHandler
    {

        private Vector3[] _points;

        private static LineRenderer _blackLine;
        private static LineRenderer _glowLine1, _glowLine2, _glowLine3, _glowLine4;
        private static LineRenderer _noLine;

        public MenuScript MenuScript;

        private LineRenderer _lineRenderer;
        private Material _meshMaterial;

        public Color DefaultColor;
        private Color _highlightColor;

        //-----------FLAGS------------------//

        public const int FlagHidden = 0;
        public const int FlagSticky = 1;
        public const int FlagDummy = 2;
        public const int FlagStuck = 3;
        public const int FlagShade = 4;
        public const int FlagNoline = 5;
        public const int FlagClearLock = 6;
        public const int FlagDecoration = 7;

        //------------------------------------//

        private bool _mouseOn;

        private bool _shade;
        public bool Disable;
        public bool Sticky;
        private bool _noDefault;
        private bool _clearLock;
        private bool _hidden;
        private bool _decoration;

        public int TriggerId;

        public int Glow = 0;

        // 0 - blue, 1 - red, 2 - whitish, 3 - purple
        public void SetGlow(int val)
        {
            Glow = val;
        }

        //should be called by the menuScript once
        public static void Init(GameObject lhb, GameObject lhg, GameObject lhn, GameObject lhg2, GameObject lhg3, GameObject lhg4)
        {
            _blackLine = lhb.GetComponent<LineRenderer>();
            _glowLine1 = lhg.GetComponent<LineRenderer>();
            _glowLine2 = lhg2.GetComponent<LineRenderer>();
            _glowLine3 = lhg3.GetComponent<LineRenderer>();
            _glowLine4 = lhg4.GetComponent<LineRenderer>();
            _noLine = lhn.GetComponent<LineRenderer>();

            /*blackLine.startWidth = 0.3f;           this doesn't work
		blackLine.endWidth = 0.3f;

		glowLine.startWidth = 1.2f;
		glowLine.endWidth = 1.2f;

		noLine.startWidth = 0.3f;
		noLine.endWidth = 0.3f;*/
        }

        //call whenever the component needs to be updated graphically
        public void Revalidate()
        {
            if (_hidden)
            {
                Debug.Log("ended on hidden");
                Hide();
            }
            else
            {
                Show();
                if (Disable && !_decoration)
                {
                    //Debug.Log("("+triggerID+") ended on disable");
                    ModeLock();
                }
                else if (_noDefault)
                {
                    //Debug.Log("(" + triggerID + ") ended on none");
                    ModeNone();
                }
                else if (_mouseOn)
                {
                    //Debug.Log("(" + triggerID + ") ended on highlight");
                    ModeHighlight();
                }
                else
                {
                    //Debug.Log("(" + triggerID + ") ended on neutral");
                    ModeNeutral();
                }
                _lineRenderer.SetPositions(_points);
            }
        }

        public void SetColor(Color c)
        {
            DefaultColor = c;
            _highlightColor = new Color(DefaultColor.r + 0.2f, DefaultColor.g + 0.2f, DefaultColor.b + 0.2f, DefaultColor.a);
            Revalidate();
        }

        public void Setup(int[] flags, Vector3[] p)
        {

            _points = p;

            _mouseOn = false;

            _lineRenderer = GetComponent<LineRenderer>();
            _meshMaterial = GetComponent<MeshRenderer>().material;

            SetColor(DefaultColor);

            //add the clicky unity stuff
            Camera.main.gameObject.AddComponent<Physics2DRaycaster>();
            AddEventSystem();

            foreach (int i in flags)
            {
                switch (i)
                {
                    case FlagHidden:
                        _hidden = true;
                        break;
                    case FlagSticky:
                        Sticky = true;
                        break;
                    case FlagDummy:
                        Disable = true;
                        break;
                    case FlagStuck:
                        Sticky = true;
                        Disable = true;
                        break;
                    case FlagShade:
                        _shade = true;
                        break;
                    case FlagNoline:
                        _noDefault = true;
                        break;
                    case FlagClearLock:
                        _clearLock = true;
                        break;
                    case FlagDecoration:
                        _decoration = true;
                        break;
                }
            }

            Revalidate();
        }

        public void Hide()
        {
            _lineRenderer.enabled = false;
            GetComponent<MeshRenderer>().enabled = false;
        }

        public void Show()
        {
            _lineRenderer.enabled = true;
            GetComponent<MeshRenderer>().enabled = true;
        }

        public void Unstick()
        {
            if (Disable)
            {
                Disable = false;
                Revalidate();
            }
        }

        public void Stick()
        {
            Disable = true;
            Revalidate();
        }

        private void ModeNone()
        {
            _lineRenderer = CopyComponent<LineRenderer>(_noLine, gameObject);
            _lineRenderer.startWidth = 0.3f;
            _lineRenderer.endWidth = 0.3f;
        }

        private void ModeHighlight()
        {
            _lineRenderer = CopyComponent<LineRenderer>(_blackLine, gameObject);
            _lineRenderer.startWidth = 0.3f;
            _lineRenderer.endWidth = 0.3f;
            if (_shade)
            {
                _meshMaterial.color = new Color(DefaultColor.r, DefaultColor.g, DefaultColor.b, 0.2f);
            }
            else
            {
                _meshMaterial.color = _highlightColor;
            }
        }

        private void ModeNeutral()
        {
            _lineRenderer = CopyComponent<LineRenderer>(_blackLine, gameObject);
            _lineRenderer.startWidth = 0.3f;
            _lineRenderer.endWidth = 0.3f;
            _meshMaterial.color = DefaultColor;
        }

        private void ModeLock()
        {
            switch (Glow)
            {
                case 0:
                    _lineRenderer = CopyComponent<LineRenderer>(_glowLine1, gameObject);
                    break;
                case 1:
                    _lineRenderer = CopyComponent<LineRenderer>(_glowLine2, gameObject);
                    break;
                case 2:
                    _lineRenderer = CopyComponent<LineRenderer>(_glowLine3, gameObject);
                    break;
                case 3:
                    _lineRenderer = CopyComponent<LineRenderer>(_glowLine4, gameObject);
                    break;
                default:
                    _lineRenderer = CopyComponent<LineRenderer>(_glowLine1, gameObject);
                    break;
            }
            _lineRenderer.startWidth = 1.2f;
            _lineRenderer.endWidth = 1.2f;
            if (_clearLock)
            {
                _meshMaterial.color = Color.clear;
            }
            else
            {
                _meshMaterial.color = _highlightColor;
            }
        }

        T CopyComponent<T>(T original, GameObject destination) where T : Component
        {
            System.Type type = original.GetType();
            var dst = destination.GetComponent(type) as T;
            if (!dst) dst = destination.AddComponent(type) as T;
            var fields = type.GetFields();
            foreach (var field in fields)
            {
                if (field.IsStatic) continue;
                field.SetValue(dst, field.GetValue(original));
            }
            var props = type.GetProperties();
            foreach (var prop in props)
            {
                if (!prop.CanWrite || !prop.CanWrite || prop.Name == "name") continue;
                prop.SetValue(dst, prop.GetValue(original, null), null);
            }
            return dst as T;
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            /*
        if (!disable)
        {
            menuScript.triggerEvent(triggerID);
            if (sticky)
            {
                disable = true;
                revalidate();
            }
        }*/
        }

        public void Click()
        {
            if (!Disable)
            {
                //menuScript.triggerEvent(triggerID);
                if (Sticky)
                {
                    Disable = true;
                    Revalidate();
                }
            }
        }

        public void OnPointerDown(PointerEventData eventData)
        {

        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            if (!Disable)
            {
                _mouseOn = true;
                Revalidate();
            }
        }

        public void OnPointerUp(PointerEventData eventData)
        {

        }

        public void OnPointerExit(PointerEventData eventData)
        {
            if (!Disable)
            {
                _mouseOn = false;
                Revalidate();
            }
        }

        void AddEventSystem()
        {
            GameObject eventSystem = null;
            GameObject tempObj = GameObject.Find("EventSystem");
            if (tempObj == null)
            {
                eventSystem = new GameObject("EventSystem");
                eventSystem.AddComponent<EventSystem>();
                eventSystem.AddComponent<StandaloneInputModule>();
            }
            else
            {
                if ((tempObj.GetComponent<EventSystem>()) == null)
                {
                    tempObj.AddComponent<EventSystem>();
                }

                if ((tempObj.GetComponent<StandaloneInputModule>()) == null)
                {
                    tempObj.AddComponent<StandaloneInputModule>();
                }
            }
        }

    }
}