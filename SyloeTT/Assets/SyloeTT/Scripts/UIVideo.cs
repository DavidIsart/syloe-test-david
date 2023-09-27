using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.Video;

public class UIVideo : MonoBehaviour, IPointerClickHandler
{
	[Header("References :")]
	[SerializeField] private VideoPlayer _videoPlayer;
	[SerializeField] private ToggleImage _toggleImage;
	[SerializeField] private bool _paused = true;

	private int fadeValue => _paused ? 1 : 0;

	private Coroutine _fadeCoroutine;

	void Awake()
	{
		_toggleImage.SetToggle(_paused);
	}

	public void OnPointerClick(PointerEventData eventData)
	{ 
		_paused = !_paused;

		if (_paused)
			_videoPlayer.Pause();
		else
			_videoPlayer.Play();

		FadeToggleImage(fadeValue);
	}

	private void FadeToggleImage(float toValue)
	{
		if (_fadeCoroutine! != null)
			StopCoroutine(_fadeCoroutine);

		StartCoroutine(FadeCoroutine(toValue));
	}

	private IEnumerator FadeCoroutine(float to, float duration = 0.4f)
	{
		if (to > 0)
			_toggleImage.gameObject.SetActive(true);

		float elapsedTime = 0;
		Color color = _toggleImage.color;
		float from = color.a;

		while (elapsedTime < duration)
		{
			elapsedTime += Time.deltaTime;
			color.a = Mathf.SmoothStep(from, to, elapsedTime / duration);
			_toggleImage.color = color;

			yield return null;
		}


		if (to <= 0)
			_toggleImage.gameObject.SetActive(false);

		_fadeCoroutine = null;
	}
}
