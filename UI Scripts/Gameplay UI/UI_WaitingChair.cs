using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class UI_WaitingChair : UI_Patient
{

    //should be 2 buttons, 3 at the most
    //0 - Send to Exam Room, 1 - Pacifiy, 2 - Exit/Cancel
    public Button[] button_Choices;
    public Text text_name, text_Story;

    //Add in Name
    //Add in DOB


    // Update is called once per frame
    void Update()
    {
        if (MyManager != null)
        {
            button_Choices[0].interactable = (MyManager.Manager_Empty_ExamRoom() != null);
            button_Choices[1].interactable = (MyPatient.Patient_Pacify_AmountLeft() > 0);
        }
    }

    void OnEnable()
    {
		//tell the nurse that they are busy.
		if (MyManager != null)
		{
			MyManager.MyNurse.IsBusy(1);
		}

        if (button_Choices.Length < 1)
        {
            Debug.LogWarning(name + " is missing buttons in it's UI_Triage Script");
        }
        if (text_Story == null)
        {
            Debug.LogWarning(name + " is missing the textfield to display it's story");
        }
        else
        {
            //text_Story.text = MyPatient.
        }
        if (MyPatient != null) {
			MyPatient.Patient_ToggleCountdown(true);
			text_name.text = MyPatient.name;
			//RON come back and change this to say some mixed information from Signs and Symptoms as well as History.
			string s = "";
			foreach (string h in MyPatient.MyDiagnosis().History())
			{
				if (s == "")
				{
					s += "- " + h;
				}
				else
				{
					s += "\n- " + h;
				}

			}
			//display portions of the history from the patient.
			text_Story.text = s;
		}
        
    }

	void OnDisable()
	{
		//inform the nurse they are no longer busy.
		if (MyManager != null)
		{
			MyManager.MyNurse.IsBusy(-1);
		}
	}

    ///// <summary>
    ///// Send the patient to an open exam room. Only called by a button click
    ///// </summary>
    //public void Send_ExamRoom()
    //{
    //    //verify that there is an open room
    //    ExamRoom e = MyManager.Manager_Empty_ExamRoom();
    //    if (e != null)
    //    {
    //        //add the patient to it's new hotspot
    //        e.PatientObject_Patient_Add(MyPatient);

    //        //make the patient move to the proper location of it's new hotspot
    //        MyPatient.Person_Move(e.PatientObject_LocationPatient(), e.tag);

    //        //remove my patient from it's current hotspot
    //        MyPatient.Patient_Hotspot_Get().PatientObject_Patient_Remove();
    //        Debug.Log("Patient has been given to: " + e.name);

    //        Close();
    //    }
    //}

    //public void Pacify()
    //{
    //    if (MyPatient.Patient_Pacify_AmountLeft() > 0)
    //    {
    //        MyPatient.Patient_Pacify();
    //        Debug.Log(MyPatient.name + " has been pacified");
    //        Close();
    //    }
        
    //}
}
