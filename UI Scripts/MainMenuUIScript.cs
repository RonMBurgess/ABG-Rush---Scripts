using UnityEngine;
using System.Collections;

public class MainMenuUIScript : MonoBehaviour {

	public bool devCodes;
	private bool ccA, ccB, ccC, ccD, ccE;

	// Use this for initialization
	void Start () {
		DevCodesReset();
	}
	
	// Update is called once per frame
	void Update () {
		if (Application.isEditor)
		{
			if (devCodes)
			{
				bool code = false;

				if (Input.GetKeyUp(KeyCode.A))
				{
					ccA = true;
					code = true;
				}
				else if (Input.GetKeyUp(KeyCode.B) && ccA)
				{
					ccB = true;
					code = true;
				}
				else if (Input.GetKeyUp(KeyCode.C) && ccB)
				{
					ccC = true;
					code = true;
				}
				else if (Input.GetKeyUp(KeyCode.D) && ccC)
				{
					ccD = true;
					code = true;
				}
				else if (Input.GetKeyUp(KeyCode.E) && ccD)
				{
					ccE = true;
					code = true;
				}

				if (code)
				{
					if (ccA && ccB && ccC && ccD && ccE)
					{
						Application.LoadLevel("UI Tests");
					}
				}
				else
				{
					
				}
			}
		}
	}

    public void Practice()
    {
        Application.LoadLevel("Scn_Practice");

    }

    public void Help()
    {
        Application.LoadLevel("Scn_Help");
    }

    public void Play()
    {
        Application.LoadLevel("Scn_Play");
    }

	public void QuitOut()
	{
		Application.Quit();
	}

	public void ToggleLanguage(string language)
	{
		if (LanguageManager._LanguageManager)
		{
			LanguageManager._LanguageManager.SetLanguage(language);
			Application.LoadLevel(1);
		}
	}

	private void DevCodesReset()
	{
		ccA = false; ccB = false; ccC = false; ccD = false; ccE = false;
	}
}
