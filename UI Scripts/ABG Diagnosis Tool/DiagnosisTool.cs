﻿using UnityEngine;
using System.Collections;
using UnityEngine.UI;


public class DiagnosisTool : MonoBehaviour {

	public Color colWrong, colCorrect, colNormal;
	public Text dragndropPH, dragndropCO2, dragndropHCO3, ansRM, ansAA, ansC;
	public Button btnSubmit;
	public Image imageRM, imageAA, imageC;
	public float answerTimer;

	private Transform startParentPH, startParentCO2, startParentHCO3;
	private UI_ExamRoomComputer ercUI;
	private bool practice, answerSubmitted, answerCorrect;
	private ABG abg;
	private Diagnosis diagnosis;
	private string defaultRM, defaultAA, defaultC;
	private float answerTimerUsed;


	/// <summary>
	/// Prepare the Diagnosis Tool
	/// </summary>
	/// <param name="gamemode">True = Gameplay, False = Practice</param>
	/// <param name="ui">The UI Exam Computer</param>
	public void Initialize(bool gamemode, UI_ExamRoomComputer ui = null )
	{
		if(gamemode){
			if (ui) { ercUI = ui; practice = false; }
		}
		else
		{
			abg = new ABG();
			practice = true;
		}

		//Set the original parents of the drag and drop objects
		if (dragndropCO2 && dragndropHCO3 && dragndropPH)
		{
			startParentPH = dragndropPH.transform.parent;
			startParentCO2 = dragndropCO2.transform.parent;
			startParentHCO3 = dragndropHCO3.transform.parent;
		}

		//get the original text for the answer fields.
		if (ansRM && ansAA && ansC)
		{
			defaultRM = ansRM.text;
			defaultAA = ansAA.text;
			defaultC = ansC.text;
		}
		//get the original color for the answer fields.
		if (imageAA && imageC && imageRM)
		{
			colNormal = imageRM.color;
		}

		
		
	}

	/// <summary>
	/// Reset Positions of ABG Variables. Create new Diagnosis if practice mode.
	/// </summary>
	public void Reset(Diagnosis d = null)
	{
		//Since the drag/drop slots handle positioning/scaling and everything else, just tell the objects who their parent/slot is.
		dragndropPH.transform.SetParent(startParentPH);
		dragndropCO2.transform.SetParent(startParentCO2);
		dragndropHCO3.transform.SetParent(startParentHCO3);

		//If a diagnosis was provided, use it
		if (d != null)
		{
			diagnosis = d;
		}
		//otherwise, create our own.
		else 
		{
			//we must be in practice mode
			diagnosis = abg.RandomDiagnosis();
		}

		//set the text inside of the drag and drop objects.
		dragndropPH.text = "pH\n" + diagnosis.PH.ToString("F2");
		dragndropCO2.text = "CO2\n" + diagnosis.CO2.ToString("F2");
		dragndropHCO3.text = "HCO3\n" + diagnosis.HCO3.ToString("F2");

		//reset the color of each button to it's original color/status



		// reset the value of each answer text to it's original text
		ansAA.text = defaultAA;
		ansRM.text = defaultRM;
		ansC.text = defaultC;


		//make sure submit button is clickable.
		btnSubmit.interactable = true;

		//reset timer
		answerTimerUsed = 0;

		//Set answersubmitted and correct to false;
		answerCorrect = false;
		answerSubmitted = false;
	}


	


	private void Update()
	{
		//determine if we are waiting for the submit button to become clickable again.
		//if (gameObject.activeInHierarchy && !btnSubmit.interactable)
		//{
		//	answerTimerUsed += Time.deltaTime;

		//	//if time is up, turn the button back on.
		//	if (answerTimerUsed >= answerTimer)
		//	{
		//		btnSubmit.interactable = true;
		//	}
		//}

		if (gameObject.activeInHierarchy && answerSubmitted)
		{
			answerTimerUsed += Time.deltaTime;
			if (answerTimerUsed >= answerTimer)
			{
				answerSubmitted = false;
				if (!answerCorrect)
				{
					btnSubmit.interactable = true;
				}
				else
				{
					if (practice)
					{
						//reset the tool
					}
					else
					{
						//inform the manager and the ui
						ercUI.FinishDiagnosis();
					}
				}
			}
		}
	}


	#region Functions/Methods Used by UI Buttons

	/// <summary>
	/// Called by the dropdown answer buttons
	/// </summary>
	/// <param name="t"></param>
	public void SwapText(Text t)
	{
		string s = t.text;
		//swap the text of the RM answer field.
		if (s == "Respiratory" || s == "Metabolic")
		{
			ansRM.text = s;
		}
		//swap the text of the AA answer field.
		else if (s == "Acidosis" || s == "Alkalosis")
		{
			ansAA.text = s;
		}
		//swap the text of the C answer field.
		else if (s == "Uncompensated" || s == "Partial Compensation" || s == "Compensated")
		{
			ansC.text = s;
		}
	}



	/// <summary>
	/// Called from the Submit UI Button. Determine if selected answers are correct
	/// </summary>
	public void Submit()
	{
		bool a = false, b = false, c = false;
		if (ansAA.text == diagnosis.Answer_Acidosis_Alkalosis)
		{
			a = true;
			imageAA.color = colCorrect;
		}
		else
		{
			imageAA.color = colWrong;
		}
		if (ansRM.text == diagnosis.Answer_Respiratory_Metabolic)
		{
			b = true;
			imageRM.color = colCorrect;
		}
		else
		{
			imageRM.color = colWrong;
		}
		if (ansC.text == diagnosis.Answer_Compensation)
		{
			c = true;
			imageC.color = colCorrect;
		}
		else
		{
			imageC.color = colWrong;
		}

		//inform the ui and manager about the answer
		//if (a && b && c)
		//{
		//	//if this is practice mode
		//	if (practice)
		//	{
		//		//display the new diagnosis button.
		//		//Click this button to reset the tool

		//	}
		//	else
		//	{
		//		//tell the manager to add points
		//		//Ron Come Back and Change This

		//		//inform the patient that they can leave, and tell the UI to close after a moment 
		//		Debug.Log("The submitted Answer is correct");
		//		ercUI.FinishDiagnosis();


		//	}

		//}
		//else
		//{
		//	//disable the submit button
		//	btnSubmit.interactable = false;

		//	//start the timer
		//	answerTimerUsed = 0;

		//	//and if in practice mode, lose points
		//	if (!practice)
		//	{
		//		//lose points
		//	}
		//}

		//Set the timer and values
		btnSubmit.interactable = false;
		answerCorrect = (a && b && c);
		answerSubmitted = true;
		answerTimerUsed = 0;

	}

	#endregion
}
