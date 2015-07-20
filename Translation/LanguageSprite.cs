using UnityEngine;
using System.Collections;


/// <summary>
/// This class is used to allow gameobject sprites to change based on the language being used.
/// </summary>
public class LanguageSprite : MonoBehaviour {

	public Sprite spriteEnglish = null, spriteSpanish = null;//the sprites that will be used for each language

	private SpriteRenderer sr; // the sprite renderer attached to this object.

	void Awake()
	{
		sr = GetComponent<SpriteRenderer>();
		if (sr == null)
		{
			Debug.LogWarning("Could not find a sprite renderer");
		}
	}

	
	


	void OnEnable()
	{
		if (!sr)
		{
			sr = GetComponent<SpriteRenderer>();
		}

		//make sure we have access to language manager
		if (LanguageManager._LanguageManager)
		{
			if (sr && spriteEnglish && spriteSpanish)
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
					sr.sprite = s;
				}
			}
		}
		
		
		
	}
}
