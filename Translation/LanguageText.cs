using UnityEngine;
using System.Collections;
using UnityEngine.UI;

/// <summary>
/// This class is used to allow UI Textfields and their text to change based on the language being used.
/// </summary>
public class LanguageText : MonoBehaviour {

	public string xmlTextSection = " ";
	public int xmlTextID = -9;//the specific id we can find this text at.
	public int xmlTextID2 = -9;//The specific id we can find the second text at. Request Bloodwork button text and Feedback Text will utilize this.

	public bool directTranslation;
	public string translateText;

	private Text textField;
	private int curXmlTextID = -9;

	void Awake()
	{
		textField = GetComponent<Text>();
	}

	void OnEnable()
	{
		UpdateText();
	}

	/// <summary>
	/// Switch the current text to use.
	/// </summary>
	/// <param name="number">1 = Initial, 2 = Secondary</param>
	public void SwitchCurrentText(int number)
	{
		if (xmlTextID >= 0 && xmlTextID2 != xmlTextID && xmlTextID2 >= 0)
		{
			if (number == 1)
			{
				curXmlTextID = xmlTextID;
			}
			else if (number == 2)
			{
				curXmlTextID = xmlTextID2;
			}
			//make sure the text field gets updated
			UpdateText();
		}
		
	}

	/// <summary>
	/// Update the text field.
	/// </summary>
	private void UpdateText()
	{
		//make sure we have access to language manager.
		if (LanguageManager._LanguageManager)
		{

			//verify we have access to the text field.
			if (textField)
			{
				//see if we need to do a direct translation
				if (directTranslation)
				{
					textField.text = LanguageManager._LanguageManager.DirectTranslation(xmlTextSection, translateText);
				}
				else
				{
					//make sure we have a current id to use.
					if (curXmlTextID < 0)
					{
						curXmlTextID = xmlTextID;
					}

					//set our text.
					textField.text = LanguageManager._LanguageManager.TextTranslation(xmlTextSection, curXmlTextID);
				}
				
			}
		
		}
	}
}
