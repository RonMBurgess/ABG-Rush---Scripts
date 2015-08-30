using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class MainMenuUIScript : MonoBehaviour {

	public bool devCodes;
	private bool ccA, ccB, ccC, ccD, ccE;

	// Use this for initialization
	void Start () {
		//Debug.Log("Random of 5,10: " + Random.Range(5, 10));
		//Debug.Log("Random of 20,15: " + Random.Range(20, 15));
		DevCodesReset();
		#region ABG Values Test
		//Test, Make sure to Delete after.
		//ABG abg = new ABG("NursingInterventions");
		//string r = "Respiratory", m = "Metabolic", aci = "Acidosis", alk = "Alkalosis", uc = "Uncompensated", pc =  "Partial Compensation", c = "Compensated";

		//string diagnosisData = m + " " + alk + " " + c;
		//for (int i = 0; i < 5; i++)
		//{
		//	Diagnosis d = new Diagnosis(m, alk, c);
		//	abg.DiagnosisAnswerValues(d);
		//	diagnosisData += "\nPH: " + d.PH.ToString("F2") + " CO2: " + d.CO2.ToString("F2") + " HCO3: " + d.HCO3.ToString("F2");
			
		//}
		//Debug.Log(diagnosisData);
		#endregion

		
		
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

	public void SwitchLanguage(string language)
	{
		if (LanguageManager._LanguageManager)
		{
			LanguageManager._LanguageManager.SetLanguage(language);
			Application.LoadLevel(1);
		}
	}

	public void ToggleLanguage()
	{
		LanguageManager lm = LanguageManager._LanguageManager;
		if (lm)
		{
			if (lm.Language() == "English")
			{
				lm.SetLanguage("Spanish");
			}
			else if (lm.Language() == "Spanish")
			{
				lm.SetLanguage("English");
			}

			Application.LoadLevel(1);
		}
	}


	private void DevCodesReset()
	{
		ccA = false; ccB = false; ccC = false; ccD = false; ccE = false;
	}

	public void SoundClick()
	{
		//Play a sound
		if (SoundManager._SoundManager)
		{
			SoundManager._SoundManager.PlaySound("Click");
		}
	}
}
