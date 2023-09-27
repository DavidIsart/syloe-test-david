using TMPro;
using UnityEngine;

public class LecturePage : MonoBehaviour
{
	[Header("References :")]
	[SerializeField] private TextMeshProUGUI _episodeNameText;
	[SerializeField] private TextMeshProUGUI _viewsCountText;
	
	void OnEnable()
	{
		EditorPage.OnInputFieldValuesSaved += SetTexts;
	}

	void OnDisable()
	{
		EditorPage.OnInputFieldValuesSaved -= SetTexts;
	}

	private void SetTexts(string episodeName, string viewsCount)
	{
		_episodeNameText.text = episodeName;
		_viewsCountText.text = viewsCount;
	}
}