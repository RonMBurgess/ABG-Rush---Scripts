using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;

public class UI_ExamRoomComputer : MonoBehaviour {

	private Patient patient;//the patient I am currently displaying information about.

	public Button button_Close, button_Diagnose, button_PatientHistory, button_Bloodwork, button_Submitassessment;
	public Text textfield_name, textfield_DOB, textfield_Bloodwork,/*textfield_History, textfield_Symptoms, textfield_Conditions, textfield_Medications, textfield_PH, textfield_HCO3, textfield_CO2,*/ textfield_InitialRM, textfield_InitialAA, textfield_assessmentAnswer;

	//The following are text fields for patient information and patient signs and symptoms.
	public List<Text> patientHistory, patientSignsSymptoms;

	public Image image_Patient;
	public GameObject screen_PatientHistory, screen_Diagnosis, panel_assessment1, panel_assessment2;
	public DiagnosisTool diagnosisTool;

	private Manager manager;
	private bool init = false;
	private string defaultRM, defaultAA;

	private List<Text> patientABGValues, patientStoryInformation;
	

	/// <summary>
	/// Set the patient that the UI will access for data
	/// </summary>
	/// <param name="p"></param>
	public void SetPatient(Patient p)
	{
		patient = p;
		Debug.Log("ExamRoomComputerUI Patient has been set.");
		
	}

	void OnEnable()
	{

		if (!init)
		{
			Initialize();
		}

		if (manager)
		{
			//inform the nurse that they are currently busy and should not be able to perform actions outside of this UI.
			manager.MyNurse.IsBusy(1);
		}
		Debug.Log("ExamRoomComputer " + patient);
		if (patient)
		{
			//tell the patient to stop counting down.
			patient.Patient_ToggleCountdown(true);

			//Turn text fields off
			//ToggleTexts("ABG Values", false);
			//ToggleTexts("Story Information", false);


			//Name
			textfield_name.text = patient.name;

			//DOB
			textfield_DOB.text = patient.DateOfBirth(); //Random.Range(1960, 2001).ToString();
			
			//Picture

			//setup patient history information ***RON COME BACK AND UNCOMMENT THIS***
			DisplayHistoryAndSignsSymptoms();

			//RON COME BACK AND CHANGE THIS

			//set the values of the PH, HCO3, CO2, Medications, Symptoms, and Conditions
			Diagnosis d = patient.MyDiagnosis();
			/*if (textfield_CO2 && textfield_HCO3 && textfield_PH)
			{
				textfield_PH.text = "PH : " + d.PH.ToString("F2");
				textfield_HCO3.text = "HCO3 : " + d.HCO3.ToString("F2");
				textfield_CO2.text = "CO2 : " + d.CO2.ToString("F2");
			}

			if (textfield_Symptoms && textfield_Conditions && textfield_Medications)
			{
				textfield_Symptoms.text = d.Symptoms();
				textfield_Conditions.text = d.Conditions();
				textfield_Medications.text = d.Medications();
			}*/
			
			//display the initial patient history screen, and make sure diagnosis screen is off.
			PatientHistoryDiagnosisTabSwitch(true);
			button_Diagnose.interactable = false;


			//determine what needs to be displayed.
			string status = patient.Status();
			
			if (status == "ExamRoom" || status == "Vitals")
			{
				//display the basic information
				//textfield_History.text = d.Story("S");

				//set status of buttons

				//ABG Tool/DIagnosis should be disabled/Invisible
				if (button_Diagnose)
				{
					//since the player is not currently able to diagnose the patient, disable this button
					button_Diagnose.interactable = false;
					//make sure that the diagnose button is not currently in it's focused animation.
					button_Diagnose.GetComponent<Animator>().SetBool("Focused", false);
				}

				if (button_PatientHistory)
				{
					//Since the player cannot diagnose, they are unable to change tabs/screens. Therefore, this button should also be disabled.
					button_PatientHistory.interactable = false;
					//Make sure that the patient history button is in it's focused animation.
					button_PatientHistory.GetComponent<Animator>().SetBool("Focused", true);
				}
				
				//Bloodwork button should also be disabled/invis
				if (button_Bloodwork)
				{
					//We cannot currently request bloodwork, so this button should also be disabled.
					button_Bloodwork.interactable = false;
					button_Bloodwork.gameObject.SetActive(false);
				}
			}
			else if (status == "VitalsComplete" || status == "Bloodwork" || status == "Diagnosis" || status == "Assessment")
			{
				//display all the information
				
				//History
				//textfield_History.text = d.Story("L");
				
				//Extra Story Information
				//ToggleTexts("Story Information", true);

				if (status == "VitalsComplete")
				{
					//the patient is now able to get their bloodwork done.
					if (button_Bloodwork)
					{
						//display the bloodwork button
						button_Bloodwork.gameObject.SetActive(true);
						//make the button interactable
						button_Bloodwork.interactable = true;
					}
					
				}

				if (status == "Assessment")
				{
					//determine if the player has already made an assessment
					string aa = patient.InitialAssessmentGetAA(), rm = patient.InitialAssessmentGetRM();
					
					//create comparisons
					string aci = "Acidosis", alk = "Alkalosis", r = "Respiratory", m = "Metabolic";
					if (LanguageManager._LanguageManager)
					{
						LanguageManager lm = LanguageManager._LanguageManager;
						aci = lm.DirectTranslation("ABG", aci);
						alk = lm.DirectTranslation("ABG", alk);
						r = lm.DirectTranslation("ABG", r);
						m = lm.DirectTranslation("ABG", m);
					}

					if ((aa == alk || aa == aci) && (rm == r || rm == m))
					{
						//turn the first assessment panel off
						panel_assessment1.SetActive(false);
						//setup the answer the player provided.
						textfield_assessmentAnswer.text = rm + " " + aa;
						//turn the second panel on.
						panel_assessment2.SetActive(true);
						//turn off the submit assessment button
						if (button_Submitassessment)
						{
							button_Submitassessment.gameObject.SetActive(false);
						}
						//display the bloodwork button
						if (button_Bloodwork)
						{
							button_Bloodwork.interactable = true;
							textfield_Bloodwork.gameObject.GetComponent<LanguageText>().SwitchCurrentText(1);
							button_Bloodwork.gameObject.SetActive(true);
						}
					}
					else
					{
						//make sure the second panel is off.
						panel_assessment2.SetActive(false);

						//make sure the initial panel has it's values and buttons prepared.
						textfield_InitialAA.text = defaultAA;
						textfield_InitialRM.text = defaultRM;

						//make sure that the bloodwork button is not being displayed
						if (button_Bloodwork)
						{
							button_Bloodwork.interactable = false;
							button_Bloodwork.gameObject.SetActive(false);
						}

						//turn the initial panel on.
						panel_assessment1.SetActive(true);

						//make sure the submit button is active and interactable
						if (button_Submitassessment)
						{
							button_Submitassessment.interactable = true;
							button_Submitassessment.gameObject.SetActive(true);
						}
					}
				}

				if (status == "BloodworkWaiting")
				{
					//The patient is already getting their bloodwork done. Make sure that the bloodwork button is either disabled, or gone. Maybe place some kind of animation/visual to show the percentage /time remaining
					if (button_Bloodwork)
					{
						//stop showing the bloodwork button
						//button_Bloodwork.gameObject.SetActive(true);
						
						//make sure the button is not interactable
						button_Bloodwork.interactable = false;
					}
				}

				if (status == "Diagnosis")
				{
					if (diagnosisTool)
					{
						//supply the diagnosis tool with the required information
						diagnosisTool.Reset(patient.MyDiagnosis());
					}

					if (button_Diagnose)
					{
						button_Diagnose.interactable = true;
					}

					if (button_Bloodwork)
					{
						//stop displaying bloodwork since we have the results now.
						button_Bloodwork.interactable = false;
						button_Bloodwork.gameObject.SetActive(false);

					}
					//Display the bloodwork results / ABG Values
					ToggleTexts("ABG Values", true);
				}
			}
		}
	}

	void OnDisable()
	{
		if (manager)
		{
			//inform the nurse that they are busy
			manager.MyNurse.IsBusy(-1);
		}

		//make sure that the diagnose button is uninteractable
		button_Diagnose.interactable = false;

		//inform the patient to resume counting down
		patient.Patient_ToggleCountdown(false);

		//turn the extra text fields off
		ToggleTexts("ABG Values", false);
		ToggleTexts("Story Information", false);
	}

	#region Private Methods

	/// <summary>
	/// Toggle the Text components to On or Off
	/// </summary>
	/// <param name="w">ABG Values or Story Information</param>
	/// <param name="on">On enables, False Disables</param>
	private void ToggleTexts(string w, bool on)
	{
		if (w == "ABG Values")
		{
			foreach (Text t in patientABGValues)
			{
				t.enabled = on;
			}
		}
		else if (w == "Story Information")
		{
			foreach (Text t in patientStoryInformation)
			{
				t.enabled = on;
			}
		}
	}


	private void Initialize()
	{
		//Initialize both lists.
		patientABGValues = new List<Text>();
		patientStoryInformation = new List<Text>();

		//populate the lists.
		//patientABGValues.Add(textfield_PH); patientABGValues.Add(textfield_CO2); patientABGValues.Add(textfield_HCO3);
		//patientStoryInformation.Add(textfield_Symptoms); patientStoryInformation.Add(textfield_Conditions); patientStoryInformation.Add(textfield_Medications);

		//clear the placeholder values inside.
		foreach (Text t in patientABGValues)
		{
			t.text = " ";
		}
		foreach (Text t in patientStoryInformation)
		{
			t.text = " ";
		}

		//turn off buttons that may not be used immediately
		if (button_Bloodwork)
		{
			button_Bloodwork.interactable = false;
		}
		//gain access to the manager
		if (GameObject.Find("Manager"))
		{
			manager = GameObject.Find("Manager").GetComponent<Manager>();
		}

		//Prepare assestment field
		if (textfield_InitialAA && textfield_InitialRM)
		{
			defaultAA = textfield_InitialAA.text;
			defaultRM = textfield_InitialRM.text;
		}
		else
		{
			Debug.LogWarning(name + "'s UI_ExamRoomComputer does not have access to the assessment fields.");
		}

		//initialize the diagnosis tool
		diagnosisTool.Initialize(true, this);

		init = true;

	}

	/// <summary>
	/// Populate the patient information area as well as the Signs and Symptoms area.
	/// </summary>
	private void DisplayHistoryAndSignsSymptoms()
	{
		//make sure we have a patient
		if (patient)
		{
			//grab the diagnosis
			Diagnosis d = patient.MyDiagnosis();

			//gain access to the history and signs and symptoms of that patient
			List<string> h = d.History(), ss = d.SignsAndSymptoms();

			//patient history bullet points
			for (int i = 0; i < patientHistory.Count; i++)
			{
				//verify that there is something in this location
				if (h.Count > i)
				{
					//place the value in the text field.
					patientHistory[i].text = "- " + h[i];
					//make sure this textfield is on.
					patientHistory[i].gameObject.SetActive(true);
				}
				else
				{
					//become inactive since there is no value to go inside.
					patientHistory[i].gameObject.SetActive(false);
				}
			}

			//signs and symptoms bullet points
			for (int i = 0; i < patientSignsSymptoms.Count; i++)
			{
				//verify that there is something in this location
				if (ss.Count > i)
				{
					//place the value in the text field.
					patientSignsSymptoms[i].text = "- " + ss[i];
					//make sure this textfield is on.
					patientSignsSymptoms[i].gameObject.SetActive(true);
				}
				else
				{
					//become inactive since there is no value to go inside.
					patientSignsSymptoms[i].gameObject.SetActive(false);
				}
			}
		}
		
	}


	#endregion


	#region Button Functions

	/// <summary>
	/// Change the Tabs and screens being shown
	/// </summary>
	/// <param name="pHistory">True = Patient History, False = Diagnosis</param>
	public void PatientHistoryDiagnosisTabSwitch(bool pHistory)
	{
		//turn the diagnosis screen off.
		screen_Diagnosis.SetActive(!pHistory);

		//turn the patient history screen on.
		screen_PatientHistory.SetActive(pHistory);

		//change the focus animations for the tab buttons
		button_Diagnose.interactable = pHistory;
		button_Diagnose.GetComponent<Animator>().SetBool("Focused", !pHistory);

		button_PatientHistory.interactable = !pHistory;
		button_PatientHistory.GetComponent<Animator>().SetBool("Focused", pHistory);
	}

	public void RequestBloodwork()
	{
		//make the button un-interactable
		button_Bloodwork.interactable = false;
		//textfield_Bloodwork.text = "REQUESTED BLOOD WORK";
		textfield_Bloodwork.gameObject.GetComponent<LanguageText>().SwitchCurrentText(1);
		//Change the status of the patient to bloodwork

		//Inform the patient's computer to send the bloodwork and begin counting down.
		if (patient)
		{
			(patient.Patient_Hotspot_Get() as ExamRoom).Computer().SendBloodwork();
		}
		
		//inform the patient to stop/begin counting down. Not sure which at the moment, or if they should just be halted.

		//Close this window
		Close();
	}

	public void InitialassessmentTextSwap(Text t)
	{
		string s = t.text;
		//make sure we have access to both of these text fields.
		if (textfield_InitialAA && textfield_InitialRM)
		{
			//check for language
			if (LanguageManager._LanguageManager)
			{
				LanguageManager lm = LanguageManager._LanguageManager;

				//swap the text inside of the text field
				if (s == lm.DirectTranslation("ABG", "Respiratory") || s == lm.DirectTranslation("ABG", "Metabolic"))
				{
					textfield_InitialRM.text = s;
				}
				else if (s == lm.DirectTranslation("ABG", "Acidosis") || s == lm.DirectTranslation("ABG", "Alkalosis"))
				{
					textfield_InitialAA.text = s;
				}
			}
			else
			{
				//default to english since we are probably testing.

				//swap the text inside of the text field
				if (s == "Respiratory" || s == "Metabolic")
				{
					textfield_InitialRM.text = s;
				}
				else if (s == "Acidosis" || s == "Alkalosis")
				{
					textfield_InitialAA.text = s;
				}
			}
			
		}
		else
		{
			Debug.LogWarning(name + "'s UI_ExamRoomComputer does not have access to the assessment text fields");
		}
	}

	public void InitialassessmentSubmit()
	{
		//take in the values submitted.
		string aa = textfield_InitialAA.text, rm = textfield_InitialRM.text;

		//create comparison values.
		string r = "Respiratory", m = "Metabolic", aci = "Acidosis", alk = "Alkalosis";
		//translate comparison values if possible.
		if (LanguageManager._LanguageManager)
		{
			LanguageManager lm = LanguageManager._LanguageManager;
			r = lm.DirectTranslation("ABG", r);
			m = lm.DirectTranslation("ABG", m);
			aci = lm.DirectTranslation("ABG", aci);
			alk = lm.DirectTranslation("ABG", alk);
		}

		//make sure that the submitted values are indeed proper values.
		if ((aa == alk || aa == aci) && (rm == r || rm == m))
		{
			//turn off the panel for the first part.
			panel_assessment1.SetActive(false);

			//turn off the assessment submit button
			if (button_Submitassessment)
			{
				button_Submitassessment.gameObject.SetActive(false);
			}

			//set the text of the second panel
			textfield_assessmentAnswer.text = rm + " " + aa;

			//give these values to the patient.
			if (patient)
			{
				patient.InitialAssessmentSet(rm, aa);
			}
			
			//display this secondary panel
			panel_assessment2.SetActive(true);

			//turn off the submit assessment button
			if (button_Submitassessment)
			{
				button_Submitassessment.gameObject.SetActive(false);
			}
			
			//display bloodwork button.
			if (button_Bloodwork)
			{
				button_Bloodwork.gameObject.SetActive(true);
				button_Bloodwork.interactable = true;
			}

		}

	}

	/// <summary>
	/// Close the interface
	/// </summary>
	public void Close()
	{
		gameObject.SetActive(false);
	}

	#endregion


	

	/// <summary>
	/// Tell the Patient to leave.
	/// Close Self/Turn Self off.
	/// </summary>
	public void FinishDiagnosis()
	{
		Debug.Log("FinishDiagnosis");
		gameObject.SetActive(false);
		patient.Patient_StatusUpdate("DiagnosisComplete");
		//patient.Patient_Leave(); // this is now handled inside the patient.

		//close since the diagnosis is complete
		//StartCoroutine("Deactivate", 1f);
		//gameObject.SetActive(false);
	}

	private IEnumerator Deactivate(float delay)
	{
		yield return new WaitForSeconds(delay);
		if (gameObject.activeInHierarchy)
		{
			gameObject.SetActive(false);
		}
	}
}
