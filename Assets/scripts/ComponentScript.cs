using UnityEngine;
using UnityEngine.EventSystems;

public class ComponentScript : MonoBehaviour, IPointerClickHandler, IPointerDownHandler, IPointerEnterHandler, IPointerUpHandler, IPointerExitHandler
{

	private Vector3[] points;

	private static LineRenderer blackLine;
	private static LineRenderer glowLine;

	public MenuScript menuScript;

	private LineRenderer lineRenderer;
	private Material meshMaterial;

	public Color defaultColor;
	private Color highlightColor;

	//-----------FLAGS------------------//

	public const int FLAG_HIDDEN = 0;
	public const int FLAG_STICKY = 1;
	public const int FLAG_DUMMY  = 2;
	public const int FLAG_STUCK  = 3;
	public const int FLAG_SHADE  = 4;
	public const int FLAG_NOLINE = 5;
	public const int FLAG_CLEAR_LOCK = 6;

	//------------------------------------//

	private bool mouseOn;

	private bool shade;
	public bool disable;
	public bool sticky;
	private bool noLine;
	private bool clearLock;

	public int triggerID;

	//should be called by the menuScript once
	public static void init(GameObject lhb, GameObject lhg)
	{
		blackLine = lhb.GetComponent<LineRenderer>();
		glowLine = lhg.GetComponent<LineRenderer>();
	}

	//call whenever the component needs to be updated graphically
	public void revalidate()
	{
		if (noLine)
		{
			lineRenderer.widthMultiplier = 0f;
		}
		else if (disable)
		{
			modeLock();
		}
		else
		{
			if (mouseOn)
			{
				modeHighlight();
			}
			else
			{
				modeNeutral();
			}
		}
		lineRenderer.SetPositions(points);
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
					lineRenderer.enabled = false;
					GetComponent<MeshRenderer>().enabled = false;
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
					noLine = true;
					break;
				case FLAG_CLEAR_LOCK:
					clearLock = true;
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
		if (disable) {
			disable = false;
			revalidate();
		}
	}

	public void stick()
	{
		disable = true;
		revalidate();
	}

	private void modeHighlight()
	{
		lineRenderer = blackLine;
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
		lineRenderer = blackLine;
		meshMaterial.color = defaultColor;
	}

	private void modeLock()
	{
		lineRenderer = glowLine;
		if (clearLock) {
			meshMaterial.color = Color.clear;
		}
		else
		{
			meshMaterial.color = highlightColor;
		}
	}

	public void OnPointerClick(PointerEventData eventData)
	{
		if (!disable) {
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
		if (!disable) {
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