using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;

public class UI_ExamRoomComputer : MonoBehaviour {

	private Patient patient;//the patient I am currently displaying information about.

	public Button button_Close, button_Diagnose, button_PatientHistory, button_Bloodwork;
	public Text text_name, text_DOB, text_History, text_Symptoms, text_Conditions, text_Medications, text_PH, text_HCO3, text_CO2;
	public Image image_Patient;
	public GameObject screen_PatientHistory, screen_Diagnosis;
	public DiagnosisTool diagnosisTool;

	private Manager manager;
	private bool init = false;

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
			ToggleTexts("ABG Values", false);
			ToggleTexts("Story Information", false);


			//Name
			text_name.text = patient.name;

			//DOB
			text_DOB.text = Random.Range(1960, 2001).ToString();
			
			//Picture

			
			//RON COME BACK AND CHANGE THIS

			//set the values of the PH, HCO3, CO2, Medications, Symptoms, and Conditions
			Diagnosis d = patient.MyDiagnosis();
			if (text_CO2 && text_HCO3 && text_PH)
			{
				text_PH.text = "PH : " + d.PH.ToString("F2");
				text_HCO3.text = "HCO3 : " + d.HCO3.ToString("F2");
				text_CO2.text = "CO2 : " + d.CO2.ToString("F2");
			}

			if (text_Symptoms && text_Conditions && text_Medications)
			{
				text_Symptoms.text = d.Symptoms();
				text_Conditions.text = d.Conditions();
				text_Medications.text = d.Medications();
			}
			
			//display the initial patient history screen, and make sure diagnosis screen is off.
			PatientHistoryDiagnosisTabSwitch(true);
			button_Diagnose.interactable = false;


			//determine what needs to be displayed.
			string status = patient.Status();
			
			if (status == "ExamRoom" || status == "Vitals")
			{
				//display the basic information
				text_History.text = d.Story("S");

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
				}
			}
			else if (status == "VitalsComplete" || status == "Bloodwork" || status == "Diagnosis")
			{
				//display all the information
				
				//History
				text_History.text = d.Story("L");
				
				//Extra Story Information
				ToggleTexts("Story Information", true);

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

		//Change the status of the patient to bloodwork

		//Inform the patient's computer to send the bloodwork and begin counting down.
		(patient.Patient_Hotspot_Get() as ExamRoom).Computer().SendBloodwork();

		//inform the patient to stop/begin counting down. Not sure which at the moment, or if they should just be halted.
	}

	/// <summary>
	/// Close the interface
	/// </summary>
	public void Close()
	{
		gameObject.SetActive(false);
	}

	#endregion


	private void Initialize()
	{
		//Initialize both lists.
		patientABGValues = new List<Text>();
		patientStoryInformation = new List<Text>();

		//populate the lists.
		patientABGValues.Add(text_PH); patientABGValues.Add(text_CO2); patientABGValues.Add(text_HCO3);
		patientStoryInformation.Add(text_Symptoms); patientStoryInformation.Add(text_Conditions); patientStoryInformation.Add(text_Medications);

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
		
		//initialize the diagnosis tool
		diagnosisTool.Initialize(true,this);

		init = true;

	}


	/// <summary>
	/// Tell the Patient to leave.
	/// Close Self/Turn Self off.
	/// </summary>
	public void FinishDiagnosis()
	{
		Debug.Log("FinishDiagnosis");
		patient.Patient_StatusUpdate("DiagnosisComplete");
		//patient.Patient_Leave(); // this is now handled inside the patient.

		//close since the diagnosis is complete
		//StartCoroutine("Deactivate", 1f);
		gameObject.SetActive(false);
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
