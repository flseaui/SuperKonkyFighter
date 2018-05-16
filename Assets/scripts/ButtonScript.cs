using UnityEngine;
using UnityEngine.EventSystems;

public class ButtonScript : MonoBehaviour, IPointerClickHandler,
								  IPointerDownHandler, IPointerEnterHandler,
								  IPointerUpHandler, IPointerExitHandler
{

	public OldMenuScript menuScript;

	private LineRenderer lineRenderer;

	private Material color;

	public Color defaultColor;
	private Color highlightColor;

	//-----------FLAGS------------------//

	public const int FLAG_HIDDEN = 0;
	public const int FLAG_STICKY = 1;
	public const int FLAG_DUMMY  = 2;
	public const int FLAG_STUCK  = 3;
	public const int FLAG_SHADE  = 4;
	public const int FLAG_NOLINE = 5;
	public const int FLAG_LOCK_CLEAR = 6;

	//--------------line types----------//
	public Material defaultMaterial;
	public Material glowMaterial;
	//------------------------------------//

	private bool mouseOn;

	private bool shade;
	public bool disable;
	public bool sticky;
	private bool noLine;
	private bool clearLock;

	public int triggerID;

	public void setColor(Color c)
	{
		defaultColor = c;
		highlightColor = new Color(defaultColor.r + 0.2f, defaultColor.g + 0.2f, defaultColor.b + 0.2f, defaultColor.a);
		if (mouseOn)
		{
			modeHighlight();
		}
		else
		{
			modeNeutral();
		}
	}

	public void startFlags(int[] flags)
	{
		mouseOn = false;
		Camera.main.gameObject.AddComponent<Physics2DRaycaster>();
		addEventSystem();
		lineRenderer = GetComponent<LineRenderer>();
		color = GetComponent<MeshRenderer>().material;
		highlightColor = new Color(defaultColor.r + 0.2f, defaultColor.g + 0.2f, defaultColor.b + 0.2f, defaultColor.a);

		modeNeutral();

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
					modeHighlight();
					break;
				case FLAG_SHADE:
					shade = true;
					break;
				case FLAG_NOLINE:
					noLine = true;
					modeNeutral();
					break;
				case FLAG_LOCK_CLEAR:
					clearLock = true;
					break;
			}
		}
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

	public void enable()
	{
		disable = false;
	}

	public void kek()
	{
		disable = true;
	}

	public void unstick()
	{
		if (disable) {
			disable = false;
			modeNeutral();
		}
	}

	public void stick()
	{
		disable = true;
		modeLock();
	}

	private void modeHighlight()
	{
		if (!noLine)
		{
			lineRenderer.widthMultiplier = 0.3f;
			lineRenderer.material = defaultMaterial;
			lineRenderer.numCornerVertices = 0;
			lineRenderer.sortingOrder = 2;
			GradientColorKey gck = new GradientColorKey();
			gck.color = Color.white;
			GradientAlphaKey gak = new GradientAlphaKey();
			gak.alpha = 0.75f;
			Gradient g = new Gradient();
			g.SetKeys(new GradientColorKey[] { gck }, new GradientAlphaKey[] { gak });
			lineRenderer.colorGradient = g;
			if (shade)
			{
				color.color = new Color(defaultColor.r, defaultColor.g, defaultColor.b, 0.2f);
			}
			else
			{
				color.color = highlightColor;
			}
		}
		else
		{
			lineRenderer.widthMultiplier = 0f;
		}
	}

	private void modeNeutral()
	{
		if (!noLine) {
			lineRenderer.widthMultiplier = 0.3f;
			lineRenderer.material = defaultMaterial;
			lineRenderer.numCornerVertices = 0;
			lineRenderer.sortingOrder = 2;
			GradientColorKey gck = new GradientColorKey();
			gck.color = Color.white;
			GradientAlphaKey gak = new GradientAlphaKey();
			gak.alpha = 0.75f;
			Gradient g = new Gradient();
			g.SetKeys(new GradientColorKey[] { gck }, new GradientAlphaKey[] { gak });
			lineRenderer.colorGradient = g;
			color.color = defaultColor;
		}
		else
		{
			lineRenderer.widthMultiplier = 0f;
		}
	}

	private void modeLock()
	{
		lineRenderer.widthMultiplier = 0.8f;
		lineRenderer.material = glowMaterial;
		lineRenderer.numCornerVertices = 8;
		lineRenderer.sortingOrder = 3;

		GradientColorKey gck0 = new GradientColorKey();
		gck0.color = new Color(0.4f, 0.6f, 1f);
		gck0.time = 0;

		GradientColorKey gck1 = new GradientColorKey();
		gck1.color = Color.black;
		gck1.time = 0.66f;

		GradientColorKey gck2 = new GradientColorKey();
		gck2.color = new Color(0.4f, 0.6f, 1f);
		gck2.time = 1;

		GradientAlphaKey gak = new GradientAlphaKey();
		gak.alpha = 1f;

		Gradient g = new Gradient();
		g.SetKeys(new GradientColorKey[] { gck0, gck1, gck2 }, new GradientAlphaKey[] { gak });
		lineRenderer.colorGradient = g;

		if (clearLock) {
			color.color = Color.clear;
		}
		else
		{
			color.color = highlightColor;
		}
	}

	public void OnPointerClick(PointerEventData eventData)
	{
		if (!disable) {
			menuScript.triggerEvent(triggerID);
			if (sticky)
			{
				modeLock();
				disable = true;
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
			modeHighlight();
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
			modeNeutral();
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