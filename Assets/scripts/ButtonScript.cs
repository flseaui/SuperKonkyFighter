using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections;

public class ButtonScript : MonoBehaviour, IPointerClickHandler,
								  IPointerDownHandler, IPointerEnterHandler,
								  IPointerUpHandler, IPointerExitHandler
{

	private float lastPixelHeight = -1;
	private TextMesh textMesh;
	private Animator animator;

	void Start()
	{
		Camera.main.gameObject.AddComponent<Physics2DRaycaster>();

		addEventSystem();

		textMesh = GetComponentInChildren<TextMesh>();

		animator = GetComponent<Animator>();
		animator.SetInteger("state", 0);
	}

	void Update()
	{

	}

	public void OnPointerClick(PointerEventData eventData)
	{
		Debug.Log("kek");
	}

	public void OnPointerDown(PointerEventData eventData)
	{
		animator.SetInteger("state", 2);
	}

	public void OnPointerEnter(PointerEventData eventData)
	{
		animator.SetInteger("state", 1);
	}

	public void OnPointerUp(PointerEventData eventData)
	{
		animator.SetInteger("state", 1);
	}

	public void OnPointerExit(PointerEventData eventData)
	{
		animator.SetInteger("state",0);
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