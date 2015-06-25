using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ABG{

    ///*
    ///The purpose of this class is to:
    /// - Create Diagnoses at startup based on data read from file
    /// - Generate values for each diagnosis. 
    /// - Store all of the different Diagnoses
    /// - Return Random Diagnosis
    //*/

    private List<string> test_Stories_L = new List<string>(), test_Stories_S = new List<string>(), test_Medications_1 = new List<string>(), test_Medications_2 = new List<string>(), test_Symptoms_1 = new List<string>(), test_Symptoms_2 = new List<string>(), test_Conditions_1 = new List<string>(), test_Conditions_2 = new List<string>();

    private List<Diagnosis> diagnoses;
    private float val_PH_Lowest = 7.24f, val_PH_Neutral_Low = 7.35f, val_PH_Neutral_High = 7.45f, val_PH_Highest = 7.58f;//PH Values
    private float val_CO2_Lowest = 20f, val_CO2_Neutral_Low = 35f, val_CO2_Neutral_High = 45f, val_CO2_Highest = 64f;//CO2 Values
    private float val_HCO3_Lowest = 14f, val_HCO3_Neutral_Low = 22f, val_HCO3_Neutral_High = 26f, val_HCO3_Highest = 42f;// HCO3 Values


    public ABG()
    {
        diagnoses = new List<Diagnosis>();
        CreateDiagnoses();
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
        if (d.Answer_Acidosis_Alkalosis == " ")
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
                d.PH = GenerateDiagnosisValues("PH", 1); d.CO2 = GenerateDiagnosisValues("CO2", 2); d.HCO3 = GenerateDiagnosisValues("HCO3", 2);
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
                d.PH = GenerateDiagnosisValues("PH", 1); d.CO2 = GenerateDiagnosisValues("CO2", 0); d.HCO3 = GenerateDiagnosisValues("HCO3", 0);
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
                d.PH = GenerateDiagnosisValues("PH", 1); d.CO2 = GenerateDiagnosisValues("CO2", 0); d.HCO3 = GenerateDiagnosisValues("HCO3", 0);
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
                d.PH = GenerateDiagnosisValues("PH", 1); d.CO2 = GenerateDiagnosisValues("CO2", 2); d.HCO3 = GenerateDiagnosisValues("HCO3", 2);
            }

            return d;
        }

        

        //return d;
    }



    /// <summary>
    /// Generate number values for each of the variables.
    /// </summary>
    /// <param name="value">name of the value ie. PH, CO2, HCO3</param>
    /// <param name="LowMedHigh">0 for acid, 1 for neutral, 2 for basic</param>
    /// <returns></returns>
    private float GenerateDiagnosisValues(string value, int LowMedHigh)
    {
        float lowest = 0f, low = 0f, high = 0f, highest = 0f;
        if (value == "PH")
        {
            lowest = val_PH_Lowest; low = val_PH_Neutral_Low; high = val_PH_Neutral_High; highest = val_PH_Highest;
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

        Debug.LogWarning("ABG: GenerateDiagnosisValues Did not receive a valid Param");
        return -50f;
    }


    /// <summary>
    /// Read in data from file to create a list of diagnoses.
    /// </summary>
    private void CreateDiagnoses()
    {
        //Come back and change this to read in from a file later.


        //Test Samples
        test_Stories_L.Add("I fell off a ladder yesterday afternoon while trimming my hedges and have been taking a narcotic pain reliever since shortly after the accident. "); test_Stories_L.Add("I woke up in the middle of the night with stomach cramps and feeling nauseous after eating at a delicatessen yesterday. I vomited lots of green fluid 3 times and I feel weak and sick to my stomach.");
        test_Stories_S.Add("I fell off a ladder yesterday afternoon while trimming my hedges."); test_Stories_S.Add("I woke up int he middle of the night wit stomach cramps");
        test_Symptoms_1.Add("Severe Pain"); test_Symptoms_1.Add("Cyanotic Fingers");
        test_Symptoms_2.Add("Stomach Cramps"); test_Symptoms_2.Add("Weak & Fainty");
        test_Conditions_1.Add("Asthma"); test_Conditions_1.Add("Lung Disease");
        test_Conditions_2.Add("Stomach Polyps"); test_Conditions_2.Add("Peptic Ulcer Disease");
        test_Medications_1.Add("Alleve"); test_Medications_1.Add("Zyrtec");
        test_Medications_2.Add("Pepto-Bismol"); test_Medications_2.Add("Tums");
        Diagnosis d = new Diagnosis("Respiratory", "Acidosis", "Uncompensated", test_Stories_L[0], test_Stories_S[0], test_Medications_1, test_Symptoms_1, test_Conditions_1);
        Diagnosis e = new Diagnosis("Metabolic", "Alkalosis", "Compensated", test_Stories_L[1], test_Stories_S[1], test_Medications_2, test_Symptoms_2, test_Conditions_2);
        diagnoses.Add(d); diagnoses.Add(e);

        //get the values for the diagnoses
        for (int i = 0; i < diagnoses.Count; i++ )
        {
            diagnoses[i] = DiagnosisAnswerValues(diagnoses[i]);
        }
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

}
