using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class ToggleImage : Image
{
	[Header("Toggle References :")]
	[SerializeField] private Sprite _toggleOnSprite;
	[SerializeField] private Sprite _toggleOffSprite;
	[SerializeField] private bool _isOn = true;
	
	public UnityEvent onValueChanged = new UnityEvent();

	protected override void Awake()
	{
		base.Awake();
		sprite = _isOn ? _toggleOnSprite : _toggleOffSprite;
	}

	protected override void OnValidate()
	{
		base.OnValidate();
		sprite = _isOn ? _toggleOnSprite : _toggleOffSprite;
	}

	public void SetToggle(bool isOn)
	{
		if (_isOn != isOn)
			onValueChanged?.Invoke();

		_isOn = isOn;
		sprite = _isOn ? _toggleOnSprite : _toggleOffSprite;
	}

	public void Toggle()
	{
		onValueChanged.Invoke();
		_isOn = !_isOn;
		sprite = _isOn ? _toggleOnSprite : _toggleOffSprite;
	}

}
