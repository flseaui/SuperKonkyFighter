using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace UI
{
	public class ButtonScript : MonoBehaviour, IPointerClickHandler,
		IPointerDownHandler, IPointerEnterHandler,
		IPointerUpHandler, IPointerExitHandler
	{

		public OldMenuScript MenuScript;

		private LineRenderer _lineRenderer;

		private Material _color;

		public Color DefaultColor;
		private Color _highlightColor;

		//-----------FLAGS------------------//

		public const int FlagHidden = 0;
		public const int FlagSticky = 1;
		public const int FlagDummy  = 2;
		public const int FlagStuck  = 3;
		public const int FlagShade  = 4;
		public const int FlagNoline = 5;
		public const int FlagLockClear = 6;

		//--------------line types----------//
		public Material DefaultMaterial;
		public Material GlowMaterial;
		//------------------------------------//

		private bool _mouseOn;

		private bool _shade;
		public bool Disable;
		public bool Sticky;
		private bool _noLine;
		private bool _clearLock;

		public int TriggerId;

		public void SetColor(Color c)
		{
			DefaultColor = c;
			_highlightColor = new Color(DefaultColor.r + 0.2f, DefaultColor.g + 0.2f, DefaultColor.b + 0.2f, DefaultColor.a);
			if (_mouseOn)
			{
				ModeHighlight();
			}
			else
			{
				ModeNeutral();
			}
		}

		public void StartFlags(IEnumerable<int> flags)
		{
			_mouseOn = false;
			Camera.main.gameObject.AddComponent<Physics2DRaycaster>();
			AddEventSystem();
			_lineRenderer = GetComponent<LineRenderer>();
			_color = GetComponent<MeshRenderer>().material;
			_highlightColor = new Color(DefaultColor.r + 0.2f, DefaultColor.g + 0.2f, DefaultColor.b + 0.2f, DefaultColor.a);

			ModeNeutral();

			foreach (var i in flags)
			{
				switch (i)
				{
					case FlagHidden:

						_lineRenderer.enabled = false;
						GetComponent<MeshRenderer>().enabled = false;
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
						ModeHighlight();
						break;
					case FlagShade:
						_shade = true;
						break;
					case FlagNoline:
						_noLine = true;
						ModeNeutral();
						break;
					case FlagLockClear:
						_clearLock = true;
						break;
				}
			}
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

		public void Enable()
		{
			Disable = false;
		}

		public void Kek()
		{
			Disable = true;
		}

		public void Unstick()
		{
			if (Disable) {
				Disable = false;
				ModeNeutral();
			}
		}

		public void Stick()
		{
			Disable = true;
			ModeLock();
		}

		private void ModeHighlight()
		{
			if (!_noLine)
			{
				_lineRenderer.widthMultiplier = 0.3f;
				_lineRenderer.material = DefaultMaterial;
				_lineRenderer.numCornerVertices = 0;
				_lineRenderer.sortingOrder = 2;
				var gck = new GradientColorKey {color = Color.white};
				var gak = new GradientAlphaKey {alpha = 0.75f};
				var g = new Gradient();
				g.SetKeys(new[] { gck }, new[] { gak });
				_lineRenderer.colorGradient = g;
				_color.color = _shade ? new Color(DefaultColor.r, DefaultColor.g, DefaultColor.b, 0.2f) : _highlightColor;
			}
			else
			{
				_lineRenderer.widthMultiplier = 0f;
			}
		}

		private void ModeNeutral()
		{
			if (!_noLine) {
				_lineRenderer.widthMultiplier = 0.3f;
				_lineRenderer.material = DefaultMaterial;
				_lineRenderer.numCornerVertices = 0;
				_lineRenderer.sortingOrder = 2;
				var gck = new GradientColorKey {color = Color.white};
				var gak = new GradientAlphaKey {alpha = 0.75f};
				var g = new Gradient();
				g.SetKeys(new[] { gck }, new[] { gak });
				_lineRenderer.colorGradient = g;
				_color.color = DefaultColor;
			}
			else
			{
				_lineRenderer.widthMultiplier = 0f;
			}
		}

		private void ModeLock()
		{
			_lineRenderer.widthMultiplier = 0.8f;
			_lineRenderer.material = GlowMaterial;
			_lineRenderer.numCornerVertices = 8;
			_lineRenderer.sortingOrder = 3;

			var gck0 = new GradientColorKey {color = new Color(0.4f, 0.6f, 1f), time = 0};
			var gck1 = new GradientColorKey {color = Color.black, time = 0.66f};
			var gck2 = new GradientColorKey {color = new Color(0.4f, 0.6f, 1f), time = 1};
			var gak  = new GradientAlphaKey {alpha = 1f};

			var g = new Gradient();
			g.SetKeys(new[] { gck0, gck1, gck2 }, new[] { gak });
			_lineRenderer.colorGradient = g;

			_color.color = _clearLock ? Color.clear : _highlightColor;
		}

		public void OnPointerClick(PointerEventData eventData)
		{
			if (!Disable) {
				MenuScript.TriggerEvent(TriggerId);
				if (Sticky)
				{
					ModeLock();
					Disable = true;
				}
			}
		}

		public void OnPointerDown(PointerEventData eventData)
		{
	
		}

		public void OnPointerEnter(PointerEventData eventData)
		{
			if (!Disable) {
				_mouseOn = true;
				ModeHighlight();
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
				ModeNeutral();
			}
		}

		private void AddEventSystem()
		{
			var tempObj = GameObject.Find("EventSystem");
			if (tempObj == null)
			{
				var eventSystem = new GameObject("EventSystem");
				eventSystem.AddComponent<EventSystem>();
				eventSystem.AddComponent<StandaloneInputModule>();
			}
			else
			{
				if (tempObj.GetComponent<EventSystem>() == null)
				{
					tempObj.AddComponent<EventSystem>();
				}

				if (tempObj.GetComponent<StandaloneInputModule>() == null)
				{
					tempObj.AddComponent<StandaloneInputModule>();
				}
			}
		}

	}
}