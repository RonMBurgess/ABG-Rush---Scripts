using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class UI_WaitingChair : UI_Patient
{

    //should be 2 buttons, 3 at the most
    //0 - Send to Exam Room, 1 - Pacifiy, 2 - Exit/Cancel
    public Button[] button_Choices;
    public Text text_Story;

    //Add in Name
    //Add in DOB


    // Update is called once per frame
    void Update()
    {
        if (MyManager != null)
        {
            button_Choices[0].interactable = (MyManager.Manager_Empty_ExamRoom() != null);
            button_Choices[1].interactable = (MyManager.Manager_Empty_WaitingChair() != null);
        }
    }

    void OnEnable()
    {
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
        if (MyPatient != null) { MyPatient.Patient_ToggleCountdown(true); }
        
    }

    /// <summary>
    /// Send the patient to an open exam room. Only called by a button click
    /// </summary>
    public void Send_ExamRoom()
    {
        //verify that there is an open room
        ExamRoom e = MyManager.Manager_Empty_ExamRoom();
        if (e != null)
        {
            //add the patient to it's new hotspot
            e.PatientObject_Patient_Add(MyPatient);

            //make the patient move to the proper location of it's new hotspot
            MyPatient.Person_Move(e.PatientObject_LocationPatient(), e.tag);

            //remove my patient from it's current hotspot
            MyPatient.Patient_Hotspot_Get().PatientObject_Patient_Remove();
            Debug.Log("Patient has been given to: " + e.name);

            Close();
        }
    }

    public void Pacify()
    {
        MyPatient.Patient_Pacify();

    }


    private void Close()
    {
        MyPatient.Patient_ToggleCountdown(false);
        gameObject.SetActive(false);
    }
}
