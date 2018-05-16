using UnityEngine;
using UnityEngine.EventSystems;

public class ComponentScript : MonoBehaviour, IPointerClickHandler, IPointerDownHandler, IPointerEnterHandler, IPointerUpHandler, IPointerExitHandler
{

    private Vector3[] points;

    private static LineRenderer blackLine;
    private static LineRenderer glowLine;
    private static LineRenderer noLine;

    public MenuScript menuScript;

    private LineRenderer lineRenderer;
    private Material meshMaterial;

    public Color defaultColor;
    private Color highlightColor;

    //-----------FLAGS------------------//

    public const int FLAG_HIDDEN = 0;
    public const int FLAG_STICKY = 1;
    public const int FLAG_DUMMY = 2;
    public const int FLAG_STUCK = 3;
    public const int FLAG_SHADE = 4;
    public const int FLAG_NOLINE = 5;
    public const int FLAG_CLEAR_LOCK = 6;
    public const int FLAG_DECORATION = 7;

    //------------------------------------//

    private bool mouseOn;

    private bool shade;
    public bool disable;
    public bool sticky;
    private bool noDefault;
    private bool clearLock;
    private bool hidden;
    private bool decoration;

    public int triggerID;

    //should be called by the menuScript once
    public static void init(GameObject lhb, GameObject lhg, GameObject lhn)
    {
        blackLine = lhb.GetComponent<LineRenderer>();
        glowLine = lhg.GetComponent<LineRenderer>();
        noLine = lhn.GetComponent<LineRenderer>();

        /*blackLine.startWidth = 0.3f;           this doesn't work
		blackLine.endWidth = 0.3f;

		glowLine.startWidth = 1.2f;
		glowLine.endWidth = 1.2f;

		noLine.startWidth = 0.3f;
		noLine.endWidth = 0.3f;*/
    }

    //call whenever the component needs to be updated graphically
    public void revalidate()
    {
        if (hidden)
        {
            Debug.Log("ended on hidden");
            hide();
        }
        else
        {
            show();
            if (disable && !decoration)
            {
                //Debug.Log("("+triggerID+") ended on disable");
                modeLock();
            }
            else if (noDefault)
            {
                //Debug.Log("(" + triggerID + ") ended on none");
                modeNone();
            }
            else if (mouseOn)
            {
                //Debug.Log("(" + triggerID + ") ended on highlight");
                modeHighlight();
            }
            else
            {
                //Debug.Log("(" + triggerID + ") ended on neutral");
                modeNeutral();
            }
            lineRenderer.SetPositions(points);
        }
    }

    public void setColor(Color c)
    {
        defaultColor = c;
        highlightColor = new Color(defaultColor.r + 0.2f, defaultColor.g + 0.2f, defaultColor.b + 0.2f, defaultColor.a);
        revalidate();
    }

    public void setup(int[] flags, Vector3[] p)
    {

        points = p;

        mouseOn = false;

        lineRenderer = GetComponent<LineRenderer>();
        meshMaterial = GetComponent<MeshRenderer>().material;

        setColor(defaultColor);

        //add the clicky unity stuff
        Camera.main.gameObject.AddComponent<Physics2DRaycaster>();
        addEventSystem();

        foreach (int i in flags)
        {
            switch (i)
            {
                case FLAG_HIDDEN:
                    hidden = true;
                    break;
                case FLAG_STICKY:
                    sticky = true;
                    break;
                case FLAG_DUMMY:
                    disable = true;
                    break;
                case FLAG_STUCK:
                    sticky = true;
                    disable = true;
                    break;
                case FLAG_SHADE:
                    shade = true;
                    break;
                case FLAG_NOLINE:
                    noDefault = true;
                    break;
                case FLAG_CLEAR_LOCK:
                    clearLock = true;
                    break;
                case FLAG_DECORATION:
                    decoration = true;
                    break;
            }
        }

        revalidate();
    }

    public void hide()
    {
        lineRenderer.enabled = false;
        GetComponent<MeshRenderer>().enabled = false;
    }

    public void show()
    {
        lineRenderer.enabled = true;
        GetComponent<MeshRenderer>().enabled = true;
    }

    public void unstick()
    {
        if (disable)
        {
            disable = false;
            revalidate();
        }
    }

    public void stick()
    {
        disable = true;
        revalidate();
    }

    private void modeNone()
    {
        lineRenderer = CopyComponent<LineRenderer>(noLine, gameObject);
        lineRenderer.startWidth = 0.3f;
        lineRenderer.endWidth = 0.3f;
    }

    private void modeHighlight()
    {
        lineRenderer = CopyComponent<LineRenderer>(blackLine, gameObject);
        lineRenderer.startWidth = 0.3f;
        lineRenderer.endWidth = 0.3f;
        if (shade)
        {
            meshMaterial.color = new Color(defaultColor.r, defaultColor.g, defaultColor.b, 0.2f);
        }
        else
        {
            meshMaterial.color = highlightColor;
        }
    }

    private void modeNeutral()
    {
        lineRenderer = CopyComponent<LineRenderer>(blackLine, gameObject);
        lineRenderer.startWidth = 0.3f;
        lineRenderer.endWidth = 0.3f;
        meshMaterial.color = defaultColor;
    }

    private void modeLock()
    {
        lineRenderer = CopyComponent<LineRenderer>(glowLine, gameObject);
        lineRenderer.startWidth = 1.2f;
        lineRenderer.endWidth = 1.2f;
        if (clearLock)
        {
            meshMaterial.color = Color.clear;
        }
        else
        {
            meshMaterial.color = highlightColor;
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
        if (!disable)
        {
            menuScript.triggerEvent(triggerID);
            if (sticky)
            {
                disable = true;
                revalidate();
            }
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {

    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (!disable)
        {
            mouseOn = true;
            revalidate();
        }
    }

    public void OnPointerUp(PointerEventData eventData)
    {

    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (!disable)
        {
            mouseOn = false;
            revalidate();
        }
    }

    void addEventSystem()
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