using UnityEngine;
using System.Collections;

public class Triage : PatientObject {

	// Use this for initialization
	void Start () {
        tag = "Triage";
        OfficeObject_Initialize();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnMouseOver()
    {
        if (OfficeObject_Ready() && OfficeObject_MousedOver())
        {
            if (Input.GetMouseButtonDown(0))
            {
                //Debug.Log("Clicked on");


                ExamRoom e = Manager.Manager_Empty_ExamRoom();
                if (e != null)
                {
                    //tell patient to move to next hotspot
                    MyPatient.Person_Move(e.PatientObject_LocationPatient(), "WaitingChair");

                    e.PatientObject_Patient_Add(MyPatient);
                    //remove my current patient
                    PatientObject_Patient_Remove();
                    Debug.Log("Patient has been given to: " + e.name);
                }
                else
                {
                    WaitingChair w = Manager.Manager_Empty_WaitingChair();
                    if (w != null)
                    {
                        //tell patient to move to next hotspot
                        MyPatient.Person_Move(w.PatientObject_LocationPatient(), "WaitingChair");

                        w.PatientObject_Patient_Add(MyPatient);
                        //remove my current patient
                        PatientObject_Patient_Remove();
                        Debug.Log("Patient has been given to: " + w.name);
                    }
                }
            }

        }
    }
}
