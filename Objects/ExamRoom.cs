using UnityEngine;
using System.Collections;

public class ExamRoom : PatientObject {

    public UI_ExamRoom ui_ExamRoom;

	// Use this for initialization
	void Start () {
        tag = "ExamRoom";
        OfficeObject_Initialize();
        MyUI = ui_ExamRoom;
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
                //tell the nurse to move to location
                Manager.MyNurse.Person_Move(location_Nurse, tag, true, this);
            }

        }
    }
}
