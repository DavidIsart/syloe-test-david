using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;

public class PageSwiper : MonoBehaviour, IDragHandler, IEndDragHandler
{
	[Header("Page Refereces :")]
	[Tooltip("Screens that you would be able to swipe from and to, order is important as first will be the leftmost and last the rightmost")]
	[SerializeField] private RectTransform[] _pages;
	[SerializeField] private bool _autoOrganize = false;

	[Header("Drag Behaviour References :")]
	[SerializeField, Range(0, 1)] private float _nextPagePercentageThreshold = 0.55f;
	[SerializeField, Range(0, 1)] private float _easeDuration = 0.5f;
	[SerializeField] private AnimationCurve _easeCurve = AnimationCurve.EaseInOut(0, 0, 1, 1);
	[SerializeField] private bool _clamp;

	private Vector3[] _swiperLocations;
	private Vector3 _currentSwiperLocation;
	private Vector2 _xBounds;
	private int _totalPages;
	private int _currentPage;

	void Start()
	{
		_totalPages = _pages.Length;

		if (_autoOrganize)
			OrganizePagesHorizontally();

		SetSwiperLocations();
	}

	// To use in button On Pressed event
	public void GoToNextPage()
	{
		SetCurrentPageIndex(1);
		_currentSwiperLocation = _swiperLocations[_currentPage];

		EaseToLocation(transform.position, _currentSwiperLocation, _easeDuration);
	}

	// To use in button On Pressed event
	public void GoToPreviousPage()
	{
		SetCurrentPageIndex(-1);
		_currentSwiperLocation = _swiperLocations[_currentPage];

		EaseToLocation(transform.position, _currentSwiperLocation, _easeDuration);
	}

	public void OnDrag(PointerEventData eventData)
	{
		float dragDifference = eventData.pressPosition.x - eventData.position.x;
		transform.position = _currentSwiperLocation - new Vector3(dragDifference, 0, 0);

		if (_clamp)
			ClampPosition();
	}

	public void OnEndDrag(PointerEventData eventData)
	{
		float screenSwipePercentage = (eventData.pressPosition.x - eventData.position.x) / Screen.width; // How much % of the screen the user has scrolled

		if (Mathf.Abs(screenSwipePercentage) >= _nextPagePercentageThreshold)
			SetCurrentPageIndex(screenSwipePercentage);

		_currentSwiperLocation = _swiperLocations[_currentPage];

		EaseToLocation(transform.position, _currentSwiperLocation, _easeDuration);
	}

	private void ClampPosition()
	{
		Vector3 position = transform.position;
		position.x = Mathf.Clamp(position.x, _xBounds.y, _xBounds.x);
		transform.position = position;
	}

	/// <summary>
	/// Only to use once at initialization, will set swiper bounds
	/// </summary>
	private void SetBounds() => _xBounds = new Vector2(_swiperLocations[0].x, _swiperLocations[_swiperLocations.Length - 1].x);

	private void SetCurrentPageIndex(float sign)
	{
		if (sign > 0 && _currentPage < _totalPages - 1)
			_currentPage++;
		else if (sign < 0 && _currentPage > 0)
			_currentPage--;
	}

	/// <summary>
	/// Sets the possible locations the swiper will need to go, to avoid extra calculations in runtime
	/// </summary>
	private void SetSwiperLocations()
	{
		int totalPages = _pages.Length;
		_swiperLocations = new Vector3[totalPages];

		for (int i = 0; i < totalPages; i++)
			_swiperLocations[i] = transform.position - new Vector3(Screen.width * i, 0, 0);

		_currentSwiperLocation = _swiperLocations[0];

		SetBounds();
	}

	/// <summary>
	/// Only to use once at initialization, will organize pages horizontally, array order is important
	/// </summary>
	private void OrganizePagesHorizontally()
	{
		RectTransform page;
		int totalPages = _pages.Length;

		for (int i = 0; i < totalPages; i++)
		{
			page = _pages[i];
			page.position = new Vector3(Screen.width * i + Screen.width * 0.5f, Screen.height * 0.5f, page.position.z);
		}
	}

	private void EaseToLocation(Vector3 origin, Vector3 end, float duration = 0.3f)
	{
		if (origin == end)
			return;

		StartCoroutine(EaseToLocationCoroutine(origin, end, duration));
	}

	private IEnumerator EaseToLocationCoroutine(Vector3 origin, Vector3 end, float duration = 0.3f)
	{
		float elapsedTime = 0;

		while (elapsedTime < duration)
		{
			elapsedTime += Time.deltaTime;
			transform.position = Vector3.Lerp(origin, end, _easeCurve.Evaluate(elapsedTime / duration));
			yield return null;
		}
	}
}
