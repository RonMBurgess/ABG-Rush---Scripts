using UnityEngine;
using System.Collections;
using UnityEngine.UI;


public class UI_ExamRoomComputer : MonoBehaviour {

	private Patient patient;//the patient I am currently displaying information about.

	public Button button_Close, button_ABGTool, button_PatientInformation, button_Bloodwork;
	public Text text_name, text_DOB, text_History, text_Symptoms, text_Conditions, text_Medications;
	public Image image_Patient;

	/// <summary>
	/// Set the patient that the UI will access for data
	/// </summary>
	/// <param name="p"></param>
	public void SetPatient(Patient p)
	{
		patient = p;
	}

	void OnEnable()
	{
		if (patient)
		{
			//Name
			text_name.text = patient.name;

			//DOB
			text_DOB.text = Random.Range(1960, 2001).ToString();
			
			//Picture

			
			//RON COME BACK AND CHANGE THIS

			
			//determine what needs to be displayed.
			string status = patient.Status();
			
			if (status == "ExamRoom" || status == "Vitals")
			{
				//display the basic information
				text_History.text = patient.MyDiagnosis().Story("S");

				//set status of buttons

				//ABG Tool/DIagnosis should be disabled/Invisible
				if (button_ABGTool)
				{
					button_ABGTool.interactable = false;
				}
				
				//Bloodwork button should also be disabled/invis
				if (button_Bloodwork)
				{
					button_Bloodwork.interactable = false;
				}
			}
			else if (status == "Bloodwork" || status == "VitalsComplete" || status == "Diagnosis")
			{
				Diagnosis d = patient.MyDiagnosis();
				//display all the information
				//History
				text_History.text = d.Story("L");
				
				//Symptoms
				text_Symptoms.text = d.Symptoms();

				//Conditions
				text_Conditions.text = d.Conditions();

				//Medications
				text_Medications.text = d.Medications();
			}
		}
	}
}
