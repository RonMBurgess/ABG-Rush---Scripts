using UnityEngine;
using System.Collections;
using UnityEngine.UI;


public class UI_Patient : MonoBehaviour {


    private Patient patient;
    private Manager manager;

    /// <summary>
    /// Return the current Patient. Set the current patient. 
    /// </summary>
    public Patient MyPatient
    {
        get { return patient; }
        set { patient = value; }
    }

    public Manager MyManager
    {

        get
        {
            if (manager == null)
            {
				manager = Manager._manager;
            } 
            return manager;
        }
    }

    void Start()
    {
        if (GameObject.Find("Manager"))
        {
            manager = GameObject.Find("Manager").GetComponent<Manager>();
        }
    }



    public void Close()
    {
        MyPatient.Patient_ToggleCountdown(false);
        gameObject.SetActive(false);
    }


    #region UI Button Functions


    /// <summary>
    /// Called by button press to send a patient home
    /// </summary>
    public void Send_Away()
    {
        MyPatient.Patient_Leave();
        Close();
    }

    public void Send_WaitingRoom()
    {
        //verify that there is an open waiting chair
        WaitingChair wc = MyManager.Manager_Empty_WaitingChair();
        if (wc != null)
        {
            //remove my patient from it's current hotspot
            MyPatient.Patient_Hotspot_Get().PatientObject_Patient_Remove();

            //add patient to it's new hotspot
            wc.PatientObject_Patient_Add(MyPatient);

            //move the patient to the proper location of it's new hotspot
            MyPatient.Person_Move(wc.PatientObject_LocationPatient(), wc.tag);


            Debug.Log("Patient has been given to: " + wc.name);

            Close();
        }

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
            //remove my patient from it's current hotspot
            MyPatient.Patient_Hotspot_Get().PatientObject_Patient_Remove();

            //add the patient to it's new hotspot
            e.PatientObject_Patient_Add(MyPatient);

			//make the patient update the player's score.
			MyPatient.Patient_PatienceScore();

            //make the patient move to the proper location of it's new hotspot
            MyPatient.Person_Move(e.PatientObject_LocationPatient(), e.tag,true,e);

			//make the nurse move to the proper location of the exam room's computer.
			manager.MyNurse.Person_Move(e.Computer().OfficeObject_LocationNurse(),e.Computer().tag, false, e.Computer());
            
            Debug.Log("Patient has been given to: " + e.name);

            Close();
        }
    }

    public void Pacify()
    {
        if (MyPatient.Patient_Pacify_AmountLeft() > 0)
        {
            MyPatient.Patient_Pacify();
            Debug.Log(MyPatient.name + " has been pacified");
            Close();
        }

    }


    #endregion
}
