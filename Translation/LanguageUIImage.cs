using UnityEngine;
using System.Collections;
using UnityEngine.UI;


/// <summary>
/// This class is used to allow UI sprites to change based on the language being used.
/// </summary>
public class LanguageUIImage : MonoBehaviour {

	public Sprite spriteEnglish = null, spriteSpanish = null;//the sprites that will be used for each language

	
	private Image image;// the image component attached to this object
	void Awake()
	{
		image = GetComponent<Image>();
		if (image == null)
		{
			Debug.LogWarning("Could not find a sprite renderer");
		}
	}





	void OnEnable()
	{
		if (!image)
		{
			image = GetComponent<Image>();
		}

		//make sure we have access to language manager
		if (LanguageManager._LanguageManager)
		{
			if (image && spriteEnglish && spriteSpanish)
			{
				string lang = LanguageManager._LanguageManager.Language();
				Sprite s = null;

				switch (lang)
				{
					case "English": s = spriteEnglish; break;
					case "Spanish": s = spriteSpanish; break;
				}

				if (s != null)
				{
					image.sprite = s;
				}
			}
		}



	}
}
