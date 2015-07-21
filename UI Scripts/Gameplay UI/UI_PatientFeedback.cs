using UnityEngine;
using System.Collections;
using UnityEngine.UI;


public class UI_PatientFeedback : MonoBehaviour {

	public GameObject correctInitialAssessmentObject;// This object will be turned on/Off depending on whether or not the initial assessment was correct.
	public Sprite spriteCorrect, spriteIncorrect;
	public Image imageInitial, imageDiagnosis;
	public Text textfieldName, textfieldInitialRM, textfieldInitialAA, textfieldDiagnosisRM, textfieldDiagnosisAA, textfieldDiagnosisC;
	public LanguageText successLevel;

	public Color colorCorrect, colorWrong;

	private bool timebonus = false;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void Close(){
		Time.timeScale = 1;
		if (timebonus)
		{
			if (Manager._manager)
			{
				Manager._manager.UpdateSatisfactionScore(4);
			}
		}
		gameObject.SetActive(false);
	}

	public void PatientFeedback(Patient p, bool initRMCorrect, bool initAACorrect)
	{
		textfieldName.text = p.name;
		textfieldInitialRM.text = p.InitialAssessmentGetRM();
		textfieldInitialAA.text = p.InitialAssessmentGetAA();

		if (initRMCorrect)
		{
			textfieldInitialRM.color = colorCorrect;
		}
		else
		{
			textfieldInitialRM.color = colorWrong;
		}

		if (initAACorrect)
		{
			textfieldInitialAA.color = colorCorrect;
		}
		else
		{
			textfieldInitialAA.color = colorWrong;
		}

		if (initRMCorrect && initAACorrect)
		{
			successLevel.SwitchCurrentText(2);

			//display the correct sprite
			imageInitial.sprite = spriteCorrect;
			//display the correct object
			correctInitialAssessmentObject.SetActive(true);
			//set the bool for time bonus to true.
			timebonus = true;
		}
		else
		{
			successLevel.SwitchCurrentText(1);
			//display the incorrect initial sprite
			imageInitial.sprite = spriteIncorrect;
			//deactivate the correct object
			correctInitialAssessmentObject.SetActive(false);
			//set the bool for time bonus to false
			timebonus = false;
		}

		//display the correct answer for the final diagnosis
		textfieldDiagnosisRM.text = p.MyDiagnosis().Answer_Respiratory_Metabolic;
		textfieldDiagnosisAA.text = p.MyDiagnosis().Answer_Acidosis_Alkalosis;
		textfieldDiagnosisC.text = p.MyDiagnosis().Answer_Compensation;
		//make sure that the diagnosis text fields are the correct color
		textfieldDiagnosisRM.color = colorCorrect;
		textfieldDiagnosisAA.color = colorCorrect;
		textfieldDiagnosisC.color = colorCorrect;

		//make sure that the final diagnosis sprite always displays correct.
		imageDiagnosis.sprite = spriteCorrect;
	}
}
