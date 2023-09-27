using UnityEngine;
using UnityEngine.EventSystems;

public class ScreenSwiper : MonoBehaviour, IDragHandler, IEndDragHandler
{
	[Tooltip("Screens that you would be able to swipe from and to, order is important as first will be the leftmost and last the rightmost")]
	[SerializeField] private RectTransform[] _screens;
	[SerializeField] private bool _autoOrganize = false;

	private Vector3 _panelLocation;

	void Start()
	{
		if (_autoOrganize)
			OrganizePagesHorizontally();

		_panelLocation = transform.position;
	}

	private void OrganizePagesHorizontally()
	{
		RectTransform rectTransform;
		int screenWidth = Screen.width;
		float xStartOffset = screenWidth * 0.5f;
		float yStartOffset = Screen.height * 0.5f;

		for (int i = 0; i < _screens.Length; i++)
		{
			rectTransform = _screens[i];
			rectTransform.position = new Vector3(screenWidth * i + xStartOffset, yStartOffset, rectTransform.position.z);
		}
	}

	public void OnDrag(PointerEventData eventData)
	{
		float dragDifference = eventData.pressPosition.x - eventData.position.x;
		transform.position = _panelLocation - new Vector3(dragDifference, 0, 0);
	}

	public void OnEndDrag(PointerEventData eventData)
	{
		_panelLocation = transform.position;
	}
}
