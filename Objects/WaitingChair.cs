using UnityEngine;
using System.Collections;

public class WaitingChair : PatientObject {

	// Use this for initialization
	void Start () {
        tag = "WaitingChair";
        OfficeObject_Initialize();
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
                    MyPatient.Person_Move(e.PatientObject_LocationPatient(), "ExamRoom");

                    e.PatientObject_Patient_Add(MyPatient);
                    //remove my current patient
                    PatientObject_Patient_Remove();
                }
            }
            
        }
    }
}
