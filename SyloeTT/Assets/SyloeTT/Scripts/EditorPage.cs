using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class EditorPage : MonoBehaviour
{
    [Header("References :")]
    [SerializeField] private TMP_InputField _episodeNameInputField;
    [SerializeField] private TMP_InputField _viewCountInputField;
    [SerializeField] private Button _saveButton;

    public delegate void SendInputFieldValuesEvent(string episodeName, string viewsCount);
    public static SendInputFieldValuesEvent OnInputFieldValuesSaved;

	void OnEnable()
	{
		_saveButton.onClick.AddListener(OnButtonSave);
	}

	void OnDisable()
	{
		_saveButton.onClick.RemoveListener(OnButtonSave);
	}

	private void OnButtonSave() => OnInputFieldValuesSaved?.Invoke(_episodeNameInputField.text, _viewCountInputField.text);
}
