using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Triage : PatientObject {

    //public UI_Triage ui_Triage;//Change this later to be accessed from the gameplay UI script instead of a public variable.
    public Transform[] patientStandingLocation;
    public float set_Time_Greeting = 10, set_Time_InitialDelay = 2;

    private Animator myAnim;
    private int hash_Talking = Animator.StringToHash("Talking");
    private List<Patient> patients;
    private List<Vector2> patient_Locations;
    private float time_Greeting, time_InitialDelay;//the amount of time spent greeting the patient. // The amount of time to wait before speaking to patient
    private bool newPatient;//Determine if I am speaking to a new patient. 
    private int stage_Greeting_Total, stage_Greeting; // what stage of the greeting process am I in? There should be 5 in all. R,P,R,P,R

	// Use this for initialization
	void Start () {
        
        Triage_Initialize();
        OfficeObject_Initialize();
        
        //MyUI = ui_Triage;

	}

    void Triage_Initialize()
    {
        tag = "Triage";
        //initialize the lists
        patients = new List<Patient>();
        patient_Locations = new List<Vector2>();
        Debug.Log("Patients: " + patients);
        Debug.Log("Patient Locations: " + patient_Locations);
        //populate the patient location list
        foreach (Transform t in patientStandingLocation)
        {
            patient_Locations.Add(t.position);
        }

        myAnim = GetComponent<Animator>();

        newPatient = false;
        stage_Greeting_Total = 5;
        stage_Greeting = 0;

        time_Greeting = set_Time_Greeting / stage_Greeting_Total;
        time_InitialDelay = set_Time_InitialDelay;
    }

    /// <summary>
    /// Called by Manager after spawning a new patient.
    /// </summary>
    /// <param name="p">The New Patient</param>
    public void Triage_Patient_Add(Patient p)
    {
        //p.Person_Move(triage.location_Patient, "Triage");

        //Add patient to patient list
        
        //Debug.Log(patients);
        patients.Add(p);
        //Debug.Log(patients.Count);
        //tell patient to move to next open location

        p.Person_Move(patient_Locations[patients.Count -1],tag,true);
        //tell patient to wait, and not tick down timer
        p.Patient_ToggleCountdown(true);
        //determine if I only have 1 patient
        if (patients.Count == 1)
        {
            newPatient = true;
            time_InitialDelay = set_Time_InitialDelay;
        }
    }


	
	// Update is called once per frame
	void Update () {
        if (patients.Count > 0)
        {
            Triage_Greeting();
        }
        
	}

    private void Triage_Greeting()
    {
        if (newPatient)
        {
            time_InitialDelay -= Time.deltaTime;
            if (time_InitialDelay <= 0)
            {
                newPatient = false;
                stage_Greeting = 1;
                //turn receptionist talking animation on.
                myAnim.SetBool(hash_Talking, true);
            }
        }
        else
        {
            time_Greeting -= Time.deltaTime;
            if (time_Greeting <= 0)
            {
                //the process should go R,P,R,P,R. The Initial R is handled above.
                switch (stage_Greeting)
                {
                    case 1: patients[0].Patient_Animation("Talking", false, true); myAnim.SetBool(hash_Talking, false); break;
                    case 2: patients[0].Patient_Animation("Talking", false, false); myAnim.SetBool(hash_Talking, true); break;
                    case 3: patients[0].Patient_Animation("Talking", false, true); myAnim.SetBool(hash_Talking, false); break;
                    case 4: patients[0].Patient_Animation("Talking", false, false); myAnim.SetBool(hash_Talking, true); break;
                }
                stage_Greeting++;
                if (stage_Greeting < stage_Greeting_Total)
                {
                    time_Greeting = set_Time_Greeting / stage_Greeting_Total;
                }
                else
                {
                    //send the patient to an open waiting chair.
                    WaitingChair wc = Manager.Manager_Empty_WaitingChair();
                    if (wc)
                    {
                        //grab a reference to the patient
                        Patient p = patients[0];
                        //remove the patient from the queue
                        patients.RemoveAt(0);
                        //add the patient to the waiting chair
                        wc.PatientObject_Patient_Add(p);
                        //the waiting chair should tell the patient to move.

                        //patients[0].Person_Move(wc.location_Patient, wc.tag, true, wc);

                        //make each patient move up in the queue
                        Triage_UpdateLine();
                        //set greeting time and initial time
                        time_Greeting = set_Time_Greeting / stage_Greeting_Total;
                        time_InitialDelay = set_Time_InitialDelay;
                        if (patients.Count > 0)
                        {
                            //set new patient = true
                            newPatient = true;
                        }
                        //set stage greeting to 0
                        stage_Greeting = 0;
                        //turn nurse speech bubble off
                        myAnim.SetBool(hash_Talking, false);
                    }
                    else
                    {
                        //what if there are no empty waiting chairs?
                    }
                }
            }
        }
    }

    private void Triage_UpdateLine()
    {
        for (int i = 0; i < patients.Count; i++)
        {
            patients[i].Person_Move(patient_Locations[i],tag,true);
        }
    }

    //void OnMouseOver()
    //{
    //    if (OfficeObject_Ready() && OfficeObject_MousedOver())
    //    {
    //        if (Input.GetMouseButtonDown(0))
    //        {
    //            //tell the nurse to move to location
    //            Manager.MyNurse.Person_Move(location_Nurse,tag,true,this);
    //        }

    //    }
    //}

    
}
