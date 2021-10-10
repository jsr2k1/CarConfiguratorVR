using UnityEngine;
using UnityEngine.UI;

public class ChangeToggleStyle : MonoBehaviour
{
	public Image uiImage;
	public Text text;
	public Image genderImage;

	public Sprite[] onOffSprites;
	public Color[] onOffTextColors;
	public Color[] onOffGenderImageColor;

	public string key = "gender";	//"scenario"
	public int value;	//gender -> male 0, female 1 //scenario-> 1,2,3...

	private Toggle thisToggle;

	private void Awake()
	{
		thisToggle = GetComponent<Toggle>();
	}

	private void Start()
	{
		CheckValueChange();
	}

	public void CheckValueChange()
	{
		if(onOffSprites.Length > 0 && onOffSprites[0])
			uiImage.sprite = onOffSprites[(thisToggle.isOn) ? 0 : 1];
		if(text)
			text.color = onOffTextColors[(thisToggle.isOn) ? 0 : 1];
		if(genderImage)
			genderImage.color = onOffGenderImageColor[(thisToggle.isOn) ? 0 : 1];
		//extra
		uiImage.color = onOffGenderImageColor[(thisToggle.isOn) ? 0 : 1];

		if (thisToggle.isOn)
		{
			PlayerPrefs.SetInt(key, value);
		}
	}
}
