using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Diagnosis {

	private string greetingEnglish, greetingSpanish, diagnosisRM, diagnosisAA, diagnosisC; //, assesmentRM, assesmentAA;
    //private List<string> medications, symptoms, conditions;
	private List<string> historyEnglish, historySpanish, signsandsymptomsEnglish, signsandsymptomsSpanish;
	private float val_PH, val_CO2, val_HCO3;

    public Diagnosis(string diag_RM = "", string diag_AA = "", string diag_C = "", string greetE = "", string greetS = "", List<string> histEnglish = null, List<string> histSpanish = null, List<string>signssymptsEnglish = null, List<string>signssymptsSpanish = null)
    {
        //initialize the lists
        //medications = new List<string>();
        //symptoms = new List<string>();
        //conditions = new List<string>();

        //set the diagnosis answers
        diagnosisRM = diag_RM; diagnosisAA = diag_AA; diagnosisC = diag_C;
        
		//set the greeting
		greetingEnglish = greetE;
		greetingSpanish = greetS;

		//set the history
		historyEnglish = histEnglish;
		historySpanish = histSpanish;

		//set the signs and symptoms
		signsandsymptomsEnglish = signssymptsEnglish;
		signsandsymptomsSpanish = signssymptsSpanish;

		//set the story
		//Debug.Log(story_L);
		//Debug.Log(story_S);
        //story_Long = story_L; story_Short = story_S;
        //set the extra information
        //medications = meds; symptoms = sympts; conditions = conds;
    }


    #region Public Fields

    public float HCO3
    {
        get { return val_HCO3;}
        set { val_HCO3 = value;}
    }

    public float CO2
    {
        get { return val_CO2; }
        set { val_CO2 = value; }
    }

    public float PH
    {
        get { return val_PH; }
        set { val_PH = value; }
    }

	
    public string Answer_Respiratory_Metabolic
    {
		get
		{
			if (diagnosisRM != "")
			{
				if (LanguageManager._LanguageManager)
				{
					return LanguageManager._LanguageManager.DirectTranslation("ABG", diagnosisRM);
				}
				else
				{
					return diagnosisRM;
				}
			}
			else
			{
				return diagnosisRM;
			}
			
		}
        set { diagnosisRM = value; }
    }

    public string Answer_Acidosis_Alkalosis
    {
		get
		{
			if (diagnosisAA != "")
			{
				if (LanguageManager._LanguageManager)
				{
					return LanguageManager._LanguageManager.DirectTranslation("ABG", diagnosisAA);
				}
				else
				{
					return diagnosisAA;
				}
			}
			else
			{
				return diagnosisAA;
			}
			
		}
        set { diagnosisAA = value; }
    }

    public string Answer_Compensation
    {
		get
		{
			if (diagnosisC != "")
			{
				if (LanguageManager._LanguageManager)
				{
					//use the replace method for strings because white space will need to be removed in order for partial compensation to work.
					return LanguageManager._LanguageManager.DirectTranslation("ABG", diagnosisC.Replace(" ", ""));
				}
				else
				{
					return diagnosisC;
				}
			}
			else
			{
				return diagnosisC;
			}
			
		}
        set { diagnosisC = value; }
    }
	
    #endregion

	/*
    #region Medication, Symptoms, Conditions
    /// <summary>
    /// Return a string of medications
    /// </summary>
    /// <returns>String of medications</returns>
    public string Medications()
    {
        string m = "None.";

        if (medications.Count > 1)
        {

            for(int i = 0; i < medications.Count; i++)
            {
                if (i == 0)
                {
                    m = medications[i];
                }
                else if(i == medications.Count - 1)
                {
					m += ", " + medications[i] + ".";
                }
                else
                {
                    m += ", " + medications[i];
                }
                
            }
        }
		else if (medications.Count == 1)
		{
			m = medications[0] + "."; ;
		}
        return m;
        
    }

    /// <summary>
    /// Return a string of Symptoms
    /// </summary>
    /// <returns>String of symptoms</returns>
    public string Symptoms()
    {
        string s = "None.";

        if (symptoms.Count > 1)
        {

            for (int i = 0; i < symptoms.Count; i++)
            {
                if (i == 0)
                {
                    s = symptoms[i];
                }
                else if (i == symptoms.Count - 1)
                {
                    s += ", " + symptoms[i] + ".";
                }
                else
                {
                    s += ", " + symptoms[i];
                }

            }
        }
		else if (symptoms.Count == 1)
		{
			s = symptoms[0] + "."; ;
		}
        return s;

    }

    /// <summary>
    /// Return a string of Conditions
    /// </summary>
    /// <returns>String of Conditions</returns>
    public string Conditions()
    {
        string c = "None.";

        if (conditions.Count > 1)
        {

            for (int i = 0; i < conditions.Count; i++)
            {
                if (i == 0)
                {
                    c = conditions[i];
                }
                else if (i == conditions.Count - 1)
                {
                    c += ", " + conditions[i] + ".";
                }
                else
                {
                    c += ", " + conditions[i];
                }

            }
        }
		else if (conditions.Count == 1)
		{
			c = conditions[0] + ".";
		}
        return c;

    }

	

	/// <summary>
	/// Return the story
	/// </summary>
	/// <param name="w">L = Long, S = Short</param>
	/// <returns></returns>
	public string Story(string w)
	{
		if (w == "L")
		{
			return story_Long;
		}
		else
		{
			return story_Short;
		}
	}
    #endregion

	*/

	//public void InitialDiagnosisSet(string RM, string AA)
	//{
	//	//if values were provided, then we must set them.
	//	if (RM != "" && AA != "")
	//	{
	//		assesmentAA = AA;
	//		assesmentRM = RM;
	//	}
		
	//}

	public bool InitialDiagnosisCorrect(string RM, string AA)
	{
		return (RM == Answer_Respiratory_Metabolic && AA == Answer_Acidosis_Alkalosis);
	}

	/// <summary>
	/// Information that should help give an initial assesment of the diagnosis.
	/// </summary>
	/// <returns>List of Strings / Bullet Points</returns>
	public List<string> History()
	{
		//Do a check for language, return the history for that language.
		if (LanguageManager._LanguageManager)
		{
			switch (LanguageManager._LanguageManager.Language()){
				case "English": return historyEnglish;// break;
				case "Spanish": return historySpanish;// break;
			}
		}
		
		//since there is no language manager, we are most likely testing, so return english.
		return historyEnglish;
	}


	/// <summary>
	/// Information that should help give an initial assesment of the diagnosis.
	/// </summary>
	/// <returns>List of Strings/Bullet Points</returns>
	public List<string> SignsAndSymptoms()
	{
		//Do a check for language, return the signs and symptoms for that language.
		if (LanguageManager._LanguageManager)
		{
			switch (LanguageManager._LanguageManager.Language())
			{
				case "English": return signsandsymptomsEnglish;// break;
				case "Spanish": return signsandsymptomsSpanish;// break;
			}
		}

		//since there is no language manager, we are most likely testing, so return english.
		return signsandsymptomsEnglish;
	}

	public string Greeting()
	{
		if (LanguageManager._LanguageManager)
		{
			switch (LanguageManager._LanguageManager.Language())
			{
				case "English": return greetingEnglish;
				case "Spanish": return greetingSpanish;
			}
		}
		//since there is no language manager, we are most likely testing, so return english.
		return greetingEnglish;
	}
}
