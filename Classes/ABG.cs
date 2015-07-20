using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Xml;
//using System.Xml.XPath;
using System.Text;

public class ABG{

    ///*
    ///The purpose of this class is to:
    /// - Create Diagnoses at startup based on data read from file
    /// - Generate values for each diagnosis. 
    /// - Store all of the different Diagnoses
    /// - Return Random Diagnosis
    //*/

	//variables for xml
	private string fileLocation;//, fileName;


    private List<string> test_Stories_L, test_Stories_S, test_Medications_1, test_Medications_2, test_Medications_3, test_Medications_4, test_Symptoms_1, test_Symptoms_2, test_Symptoms_3, test_Symptoms_4, test_Conditions_1, test_Conditions_2, test_Conditions_3, test_Conditions_4;

    private List<Diagnosis> diagnoses, diagnosesInUse;
    private float val_PH_Lowest = 7.24f, val_PH_Neutral_Low = 7.35f, val_PH_Neutral_High = 7.45f, val_PH_Highest = 7.58f;//PH Values
    private float val_CO2_Lowest = 20f, val_CO2_Neutral_Low = 35f, val_CO2_Neutral_High = 45f, val_CO2_Highest = 64f;//CO2 Values
    private float val_HCO3_Lowest = 14f, val_HCO3_Neutral_Low = 22f, val_HCO3_Neutral_High = 26f, val_HCO3_Highest = 42f;// HCO3 Values

	/// <summary>
	/// Create a new ABG Class. 
	/// </summary>
	/// <param name="locationOfFile">Location of the xml file to be read from.</param>
	/// <param name="nameOfFile">Name of the xml file to be read from.</param>
    public ABG(string locationOfFile = "", string nameOfFile = "")
    {
        diagnoses = new List<Diagnosis>();
		diagnosesInUse = new List<Diagnosis>();

		if (locationOfFile != "" /*&& nameOfFile != ""*/)
		{
			fileLocation = locationOfFile;
			//fileName = nameOfFile;
			LoadDiagnosesFromXML();
		}
		else
		{
			CreateDiagnoses();
		}
        
    }
	
	


#region Diagnosis Generation and Preparation

    /// <summary>
    /// Set the answer values for a diagnosis
    /// </summary>
    /// <param name="d">The Diagnosis</param>
    /// <returns>The same Diagnosis with answers and values</returns>
    public Diagnosis DiagnosisAnswerValues(Diagnosis d)
    {
        string rm = d.Answer_Respiratory_Metabolic, aa = d.Answer_Acidosis_Alkalosis, comp = d.Answer_Compensation;
        
        
        //if the diagnosis does not have set answers, give it some.
        if (aa == " ")
        {
            int r = Random.Range(0, 12);

            switch (r)
            {
                case (0): d.Answer_Respiratory_Metabolic = "Respiratory"; d.Answer_Acidosis_Alkalosis = "Acidosis"; d.Answer_Compensation = "Uncompensated"; break;
                case (1): d.Answer_Respiratory_Metabolic = "Respiratory"; d.Answer_Acidosis_Alkalosis = "Acidosis"; d.Answer_Compensation = "Partial Compensation"; break;
                case (2): d.Answer_Respiratory_Metabolic = "Respiratory"; d.Answer_Acidosis_Alkalosis = "Acidosis"; d.Answer_Compensation = "Compensated"; break;
                case (3): d.Answer_Respiratory_Metabolic = "Respiratory"; d.Answer_Acidosis_Alkalosis = "Alkalosis"; d.Answer_Compensation = "Uncompensated"; break;
                case (4): d.Answer_Respiratory_Metabolic = "Respiratory"; d.Answer_Acidosis_Alkalosis = "Alkalosis"; d.Answer_Compensation = "Partial Compensation"; break;
                case (5): d.Answer_Respiratory_Metabolic = "Respiratory"; d.Answer_Acidosis_Alkalosis = "Alkalosis"; d.Answer_Compensation = "Compensated"; break;
                case (6): d.Answer_Respiratory_Metabolic = "Metabolic"; d.Answer_Acidosis_Alkalosis = "Acidosis"; d.Answer_Compensation = "Uncompensated"; break;
                case (7): d.Answer_Respiratory_Metabolic = "Metabolic"; d.Answer_Acidosis_Alkalosis = "Acidosis"; d.Answer_Compensation = "Partial Compensation"; break;
                case (8): d.Answer_Respiratory_Metabolic = "Metabolic"; d.Answer_Acidosis_Alkalosis = "Acidosis"; d.Answer_Compensation = "Compensated"; break;
                case (9): d.Answer_Respiratory_Metabolic = "Metabolic"; d.Answer_Acidosis_Alkalosis = "Alkalosis"; d.Answer_Compensation = "Uncompensated"; break;
                case (10): d.Answer_Respiratory_Metabolic = "Metabolic"; d.Answer_Acidosis_Alkalosis = "Alkalosis"; d.Answer_Compensation = "Partial Compensation"; break;
                case (11): d.Answer_Respiratory_Metabolic = "Metabolic"; d.Answer_Acidosis_Alkalosis = "Alkalosis"; d.Answer_Compensation = "Compensated"; break;
            }
            //go through the gauntlet again but this time, pick up the number values.
            return DiagnosisAnswerValues(d);
        }
        else
        {
            //since we have set answers, set the number values
            if (rm == "Respiratory" && aa == "Acidosis" && comp == "Uncompensated")
            {
                d.PH = GenerateDiagnosisValues("PH", 0); d.CO2 = GenerateDiagnosisValues("CO2", 2); d.HCO3 = GenerateDiagnosisValues("HCO3", 1);
            }
            else if (rm == "Respiratory" && aa == "Acidosis" && comp == "Partial Compensation")
            {
                d.PH = GenerateDiagnosisValues("PH", 0); d.CO2 = GenerateDiagnosisValues("CO2", 2); d.HCO3 = GenerateDiagnosisValues("HCO3", 2);
            }
            else if (rm == "Respiratory" && aa == "Acidosis" && comp == "Compensated")
            {
                d.PH = GenerateDiagnosisValues("PH", 5); d.CO2 = GenerateDiagnosisValues("CO2", 2); d.HCO3 = GenerateDiagnosisValues("HCO3", 2);
            }
            else if (rm == "Respiratory" && aa == "Alkalosis" && comp == "Uncompensated")
            {
                d.PH = GenerateDiagnosisValues("PH", 2); d.CO2 = GenerateDiagnosisValues("CO2", 0); d.HCO3 = GenerateDiagnosisValues("HCO3", 1);
            }
            else if (rm == "Respiratory" && aa == "Alkalosis" && comp == "Partial Compensation")
            {
                d.PH = GenerateDiagnosisValues("PH", 2); d.CO2 = GenerateDiagnosisValues("CO2", 0); d.HCO3 = GenerateDiagnosisValues("HCO3", 0);
            }
            else if (rm == "Respiratory" && aa == "Alkalosis" && comp == "Compensated")
            {
                d.PH = GenerateDiagnosisValues("PH", 6); d.CO2 = GenerateDiagnosisValues("CO2", 0); d.HCO3 = GenerateDiagnosisValues("HCO3", 0);
            }
            else if (rm == "Metabolic" && aa == "Acidosis" && comp == "Uncompensated")
            {
                d.PH = GenerateDiagnosisValues("PH", 0); d.CO2 = GenerateDiagnosisValues("CO2", 1); d.HCO3 = GenerateDiagnosisValues("HCO3", 0);
            }
            else if (rm == "Metabolic" && aa == "Acidosis" && comp == "Partial Compensation")
            {
                d.PH = GenerateDiagnosisValues("PH", 0); d.CO2 = GenerateDiagnosisValues("CO2", 0); d.HCO3 = GenerateDiagnosisValues("HCO3", 0);
            }
            else if (rm == "Metabolic" && aa == "Acidosis" && comp == "Compensated")
            {
                d.PH = GenerateDiagnosisValues("PH", 5); d.CO2 = GenerateDiagnosisValues("CO2", 0); d.HCO3 = GenerateDiagnosisValues("HCO3", 0);
            }
            else if (rm == "Metabolic" && aa == "Alkalosis" && comp == "Uncompensated")
            {
                d.PH = GenerateDiagnosisValues("PH", 2); d.CO2 = GenerateDiagnosisValues("CO2", 1); d.HCO3 = GenerateDiagnosisValues("HCO3", 2);
            }
            else if (rm == "Metabolic" && aa == "Alkalosis" && comp == "Partial Compensation")
            {
                d.PH = GenerateDiagnosisValues("PH", 2); d.CO2 = GenerateDiagnosisValues("CO2", 2); d.HCO3 = GenerateDiagnosisValues("HCO3", 2);
            }
            else if (rm == "Metabolic" && aa == "Alkalosis" && comp == "Compensated")
            {
                d.PH = GenerateDiagnosisValues("PH", 6); d.CO2 = GenerateDiagnosisValues("CO2", 2); d.HCO3 = GenerateDiagnosisValues("HCO3", 2);
            }

            return d;
        }

        

        //return d;
    }



    /// <summary>
    /// Generate number values for each of the variables.
    /// </summary>
    /// <param name="value">name of the value ie. PH, CO2, HCO3</param>
    /// <param name="LowMedHigh">0 for acid, 1 for neutral, 2 for basic, 5 == Lower Neutral, 6 = Higher Neutral</param>
    /// <returns></returns>
    private float GenerateDiagnosisValues(string value, int LowMedHigh)
    {
        float lowest = 0f, low = 0f, lowMed = 0f, medHigh = 0f, high = 0f, highest = 0f;
        if (value == "PH")
        {
			lowest = val_PH_Lowest; low = val_PH_Neutral_Low; high = val_PH_Neutral_High; highest = val_PH_Highest; lowMed = 39.5f; medHigh = 40.5f;
        }
        else if (value == "CO2")
        {
            lowest = val_CO2_Lowest; low = val_CO2_Neutral_Low; high = val_CO2_Neutral_High; highest = val_CO2_Highest;
        }
        else if (value == "HCO3")
        {
            lowest = val_HCO3_Lowest; low = val_HCO3_Neutral_Low; high = val_HCO3_Neutral_High; highest = val_HCO3_Highest;
        }


        if (LowMedHigh == 0)
        {
            return Random.Range(lowest, low);
        }
        else if (LowMedHigh == 1)
        {
            return Random.Range(low, high);
        }
        else if (LowMedHigh == 2)
        {
            return Random.Range(high, highest);
        }
		else if (LowMedHigh == 5)
		{
			return Random.Range(low, lowMed);
		}
		else if (LowMedHigh == 6)
		{
			return Random.Range(medHigh, high);
		}

        Debug.LogWarning("ABG: GenerateDiagnosisValues Did not receive a valid Param");
        return -50f;
    }


    /// <summary>
    /// Read in data from file to create a list of diagnoses.
    /// </summary>
    private void CreateDiagnoses()
    {
		Debug.Log("ABG is creating Diagnoses");
        //Come back and change this to read in from a file later.


        //Initialize Test Sample lists.

		test_Stories_L = new List<string>(); test_Stories_S = new List<string>();
		test_Symptoms_1 = new List<string>(); test_Symptoms_2 = new List<string>(); test_Symptoms_3 = new List<string>(); test_Symptoms_4 = new List<string>();
		test_Conditions_1 = new List<string>(); test_Conditions_2 = new List<string>(); test_Conditions_3 = new List<string>(); test_Conditions_4 = new List<string>();
		test_Medications_1 = new List<string>(); test_Medications_2 = new List<string>(); test_Medications_3 = new List<string>(); test_Medications_4 = new List<string>();

		//populate the test sample lists.
		//long stories
		test_Stories_L.Add("I fell off a ladder yesterday afternoon while trimming my hedges and have been taking a narcotic pain reliever since shortly after the accident."); test_Stories_L.Add("I woke up in the middle of the night with stomach cramps and feeling nauseous after eating at a delicatessen yesterday. I vomited lots of green fluid 3 times and I feel weak and sick to my stomach."); test_Stories_L.Add("I was on my way to an important meeting for work when I realized I forgot several essential documents. I became anxious and started hyperventilating. Then I became dizzy and felt tingling in my fingertips."); test_Stories_L.Add("My child has been very difficult to feed and has had very frequent runny poops for the past 3 days. My child also seems to be breathing fast.");


		//short stories
		test_Stories_S.Add("I fell off a ladder yesterday afternoon while trimming my hedges."); test_Stories_S.Add("I woke up in the middle of the night with stomach cramps"); test_Stories_S.Add("I became anxious on the way to work and started hyperventilating!!"); test_Stories_S.Add("My child has been difficult to feed and has had runny poops for days.");

		//symptoms
        test_Symptoms_1.Add("Severe Pain"); test_Symptoms_1.Add("Cyanotic Fingers");
        test_Symptoms_2.Add("Stomach Cramps"); test_Symptoms_2.Add("Weak & Fainty");
		test_Symptoms_3.Add("Hyperventilation"); test_Symptoms_3.Add("Dizziness"); test_Symptoms_3.Add("Palpitations"); test_Symptoms_3.Add("Tingling in arms");
		test_Symptoms_4.Add("Tachypnea"); test_Symptoms_4.Add("Depressed anterior fontanel");

		//conditions
        test_Conditions_1.Add("Asthma"); test_Conditions_1.Add("Lung Disease");
        test_Conditions_2.Add("Stomach Polyps"); test_Conditions_2.Add("Peptic Ulcer Disease");

		//medications
        test_Medications_1.Add("Alleve"); test_Medications_1.Add("Zyrtec");
        test_Medications_2.Add("Pepto-Bismol"); test_Medications_2.Add("Tums");

		//generate the diagnoses
        Diagnosis d = new Diagnosis("Respiratory", "Acidosis", "Uncompensated", test_Stories_L[0], test_Stories_S[0], test_Medications_1, test_Symptoms_1, test_Conditions_1);
        Diagnosis e = new Diagnosis("Metabolic", "Alkalosis", "Compensated", test_Stories_L[1], test_Stories_S[1], test_Medications_2, test_Symptoms_2, test_Conditions_2);
        
		Diagnosis f = new Diagnosis("Respiratory", "Alkalosis", "Uncompensated", test_Stories_L[2], test_Stories_S[2], test_Medications_3, test_Symptoms_3, test_Medications_3);
		Diagnosis g = new Diagnosis("Metabolic", "Acidosis", "Compensated", test_Stories_L[3], test_Stories_S[3], test_Medications_4, test_Symptoms_4, test_Conditions_4);

		//add the new diagnoses to the list.
		diagnoses.Add(d); diagnoses.Add(e); diagnoses.Add(f); diagnoses.Add(g);

        //get the values for the diagnoses
        for (int i = 0; i < diagnoses.Count; i++ )
        {
            diagnoses[i] = DiagnosisAnswerValues(diagnoses[i]);
        }
    }

	private void LoadDiagnosesFromXML()
	{
		Debug.Log("LoadDiagnosesFromXML");
		if (fileLocation != "")
		{
			XmlDocument xmlDoc = new XmlDocument();
			xmlDoc.Load(fileLocation);

			//parse through all the different interventions.
			XmlNodeList interventions = xmlDoc.GetElementsByTagName("Intervention");

			foreach (XmlNode intervention in interventions)
			{
				
				
				//diagnosis RM, AA, C
				string rm = "" , aa = "", c = "";
				List<string> historyEnglish = new List<string>();
				List<string> historySpanish = new List<string>();
				List<string> signsSymptomsEnglish = new List<string>();
				List<string> signsSymptomsSpanish = new List<string>();

				foreach (XmlNode child in intervention)
				{
					string n = child.Name;
					//diagnosis
					if (n == "Diagnosis")
					{
						rm = child.FirstChild.InnerText;
						aa = child.ChildNodes[1].InnerText;
						c = child.LastChild.InnerText;
					}

					//English
					else if (n == "English" || n == "Spanish")
					{
						//each language only has 2 sections of information
						XmlNode signsSymptoms = child.FirstChild;
						XmlNode history = child.LastChild;

						//create a list for each history
						List<string> allHistory = new List<string>();
						//create a list for all signs and symptoms
						List<string> allSignsSymptoms = new List<string>();

						//Read in all of the history information.
						foreach (XmlNode hist in history)
						{
							if (hist.InnerText != null && hist.InnerText != " " && hist.NodeType != XmlNodeType.Comment)
							{
								allHistory.Add(hist.InnerText);
							}
							
						}
						//Read in all of the signs and symptoms information.
						foreach (XmlNode ss in signsSymptoms)
						{
							if (ss.InnerText != null && ss.InnerText != " " && ss.NodeType != XmlNodeType.Comment)
							{
								allSignsSymptoms.Add(ss.InnerText);
							}
							
						}

						//Determine which language we are looking at, and set the list accordingly.
						if (n == "English") { historyEnglish = allHistory; signsSymptomsEnglish = allSignsSymptoms; }
						else if (n == "Spanish") { historySpanish = allHistory; signsSymptomsSpanish = allSignsSymptoms; }
					}

					
				}

				//create the diagnosis
				Diagnosis d = new Diagnosis(rm, aa, c, " ", " ", historyEnglish, historySpanish, signsSymptomsEnglish, signsSymptomsSpanish);

				//add this diagnosis to the list
				diagnoses.Add(d);
			}
		
		}
		//XmlNodeList interventions
	}


#endregion


    /// <summary>
    /// Return a Diagnosis with Random Values for Practice mode.
    /// </summary>
    /// <returns>Diagnosis with Random Answer Values.</returns>
    public Diagnosis RandomDiagnosis()
    {
        //create new diagnosis
        Diagnosis d = new Diagnosis();
        //set values for answers
        DiagnosisAnswerValues(d);
        return d;
    }

	/// <summary>
	/// Return a random diagnosis for game mode.
	/// </summary>
	/// <returns></returns>
	public Diagnosis PatientDiagnosis()
	{
		//return a random diagnosis for a patient.
		if (diagnoses.Count > 0)
		{
			Diagnosis d = diagnoses[Random.Range(0, diagnoses.Count)];
			diagnoses.Remove(d);
			diagnosesInUse.Add(d);
			//randomize the values
			return DiagnosisAnswerValues(d);
		}
		else
		{
			return RandomDiagnosis();
		}
	}

	/// <summary>
	/// Inform ABG that this Diagnosis is finished.
	/// </summary>
	/// <param name="d"></param>
	public void PatientDiagnosisComplete(Diagnosis d)
	{
		//remove the diagnosis because it's no longer in use by a patient.
		diagnosesInUse.Remove(d);
		//add the diagnosis back to the available diagnosis list.
		diagnoses.Add(d);
	}

}
