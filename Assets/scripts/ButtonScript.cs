using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections;

public class ButtonScript : MonoBehaviour, IPointerClickHandler,
								  IPointerDownHandler, IPointerEnterHandler,
								  IPointerUpHandler, IPointerExitHandler
{

	public MenuScript menuScript;

	private LineRenderer lineRenderer;

	private bool glow;

	public Material defaultMaterial;
	public Material glowMaterial;

	void Start()
	{
		Camera.main.gameObject.AddComponent<Physics2DRaycaster>();

		addEventSystem();

		lineRenderer = GetComponent<LineRenderer>();

		glow = false;
	}

	void Update()
	{
		if (glow)
		{
			lineRenderer.widthMultiplier = 1.9f;
			lineRenderer.material = glowMaterial;
			lineRenderer.numCornerVertices = 9;
			lineRenderer.sortingOrder = 99;
			GradientColorKey gck = new GradientColorKey();
			gck.color = Color.white;
			GradientAlphaKey gak = new GradientAlphaKey();
			gak.alpha = 1f;
			Gradient g = new Gradient();
			g.SetKeys(new GradientColorKey[] { gck }, new GradientAlphaKey[] { gak });
			lineRenderer.colorGradient = g;
		}
		else
		{
			lineRenderer.widthMultiplier = 0.3f;
			lineRenderer.material = defaultMaterial;
			lineRenderer.numCornerVertices = 0;
			lineRenderer.sortingOrder = 1;
			GradientColorKey gck = new GradientColorKey();
			gck.color = Color.black;
			GradientAlphaKey gak = new GradientAlphaKey();
			gak.alpha = 1f;
			Gradient g = new Gradient();
			g.SetKeys(new GradientColorKey[] { gck}, new GradientAlphaKey[] { gak });
			lineRenderer.colorGradient = g;
		}
	}

	public void OnPointerClick(PointerEventData eventData)
	{
		Debug.Log("kek");
		menuScript.startScreen(1);
	}

	public void OnPointerDown(PointerEventData eventData)
	{
		//animator.SetInteger("state", 2);
	}

	public void OnPointerEnter(PointerEventData eventData)
	{
		glow = true;
	}

	public void OnPointerUp(PointerEventData eventData)
	{
		//animator.SetInteger("state", 1);
	}

	public void OnPointerExit(PointerEventData eventData)
	{
		glow = false;
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