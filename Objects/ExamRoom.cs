using UnityEngine;
using System.Collections;

public class ExamRoom : PatientObject {

    public UI_ExamRoom ui_ExamRoom;
	public ExamRoomComputer computer;

	// Use this for initialization
	void Start () {
        tag = "ExamRoom";
        OfficeObject_Initialize();
		computer.MyExamRoom(this);
        MyUI = ui_ExamRoom;
	}

	/// <summary>
	/// Return the ExamRoom Computer attached to this room.
	/// </summary>
	/// <returns></returns>
	public ExamRoomComputer Computer()
	{
		return computer;
	}

    //void OnMouseOver()
    //{
    //    if (OfficeObject_Ready() && OfficeObject_MousedOver())
    //    {
    //        if (Input.GetMouseButtonDown(0))
    //        {
    //            //tell the nurse to move to location
    //            Manager.MyNurse.Person_Move(location_Nurse, tag, true, this);
    //        }

    //    }
    //}
}
