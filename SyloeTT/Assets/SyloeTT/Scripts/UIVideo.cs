using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.Video;

public class UIVideo : MonoBehaviour, IPointerClickHandler
{
	[Header("References :")]
	[SerializeField] private VideoPlayer _videoPlayer;
	[SerializeField] private ToggleImage _toggleImage;
	[SerializeField] private Slider _slider;
	[SerializeField] private TextMeshProUGUI _timeText;
	[SerializeField] private bool _paused = true;

	private float _videoLength;
	private Coroutine _fadeCoroutine;

	private int fadeValue => _paused ? 1 : 0;
	private float clockTime => (float)_videoPlayer.clockTime;
	private float watchPercentage => Mathf.Clamp01(clockTime / _videoLength);

	void Awake()
	{
		_toggleImage.SetToggle(_paused);
		_videoLength = (float)_videoPlayer.length;

		UpdateSliderAndText();
	}

	void Update()
	{
		if (!_paused)
			UpdateSliderAndText();
	}

	private void UpdateSliderAndText()
	{
		_slider.normalizedValue = watchPercentage;
		_timeText.text = TimeSpan.FromSeconds(_videoPlayer.clockTime).ToString(@"mm\:ss");

		print(TimeSpan.FromSeconds(_videoPlayer.clockTime).ToString(@"mm\:ss"));
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
