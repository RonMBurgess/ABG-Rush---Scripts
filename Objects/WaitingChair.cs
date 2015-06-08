using UnityEngine;
using System.Collections;

public class WaitingChair : PatientObject {

    public UI_WaitingChair ui_waitingchair;

	// Use this for initialization
	void Start () {
        tag = "WaitingChair";
        OfficeObject_Initialize();
        MyUI = ui_waitingchair;
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
