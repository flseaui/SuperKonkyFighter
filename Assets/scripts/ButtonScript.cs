using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections;

public class ButtonScript : MonoBehaviour, IPointerClickHandler,
								  IPointerDownHandler, IPointerEnterHandler,
								  IPointerUpHandler, IPointerExitHandler
{

	public MenuScript menuScript;

	private LineRenderer lineRenderer;

	public Material defaultMaterial;
	public Material glowMaterial;

	private bool invis;

	void Start()
	{
		Camera.main.gameObject.AddComponent<Physics2DRaycaster>();

		addEventSystem();

		lineRenderer = GetComponent<LineRenderer>();

		glowSwitch(false);
	}

	public void hide()
	{
		invis = true;
	}

	private void glowSwitch(bool glow)
	{
		if (!invis) {
			if (glow)
			{
				lineRenderer.widthMultiplier = 0.8f;
				lineRenderer.material = glowMaterial;
				lineRenderer.numCornerVertices = 8;
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
				lineRenderer.sortingOrder = 2;
				GradientColorKey gck = new GradientColorKey();
				gck.color = Color.white;
				GradientAlphaKey gak = new GradientAlphaKey();
				gak.alpha = 0.75f;
				Gradient g = new Gradient();
				g.SetKeys(new GradientColorKey[] { gck }, new GradientAlphaKey[] { gak });
				lineRenderer.colorGradient = g;
			}
		}
		else
		{
			lineRenderer.enabled = false;
			GetComponent<MeshRenderer>().enabled = false;
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
		glowSwitch(true);
	}

	public void OnPointerUp(PointerEventData eventData)
	{
		//animator.SetInteger("state", 1);
	}

	public void OnPointerExit(PointerEventData eventData)
	{
		glowSwitch(false);
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