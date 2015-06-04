using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class Manager : MonoBehaviour {

    //change this later to private and load from resource folder
    public GameObject prefab_Patient;
    public Vector2 location_Exit;

    private Triage triage;
    private List<Patient> list_Patients;
    private List<WaitingChair> list_WaitingChairs;
    private List<ExamRoom> list_ExamRooms;
    private int score_Patients_Total;
    private float score_Satisfaction;


	// Use this for initialization
	void Start () {
        Manager_Initialize();
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyUp(KeyCode.P))
        {
            Manager_PatientSpawn();
        }
	}

    public void Manager_Patient_StormOut(Patient p)
    {
        list_Patients.Remove(p);
        p.Person_Move(location_Exit,"Exit");
    }

    /// <summary>
    /// Determine if there is an empty examination room
    /// </summary>
    /// <returns>Empty room or null</returns>
    public ExamRoom Manager_Empty_ExamRoom()
    {
        foreach (ExamRoom e in list_ExamRooms)
        {
            if (!e.PatientObject_Occupied())
            {
                return e;
            }
        }

        return null;
    }


    /// <summary>
    /// Determine if there is an empty Waiting Chair
    /// </summary>
    /// <returns>Empty chair or null</returns>
    public WaitingChair Manager_Empty_WaitingChair()
    {
        foreach (WaitingChair w in list_WaitingChairs)
        {
            if (!w.PatientObject_Occupied())
            {
                return w;
            }
        }
        return null;
    }


    /// <summary>
    /// Set up the manager
    /// </summary>
    private void Manager_Initialize()
    {
        list_WaitingChairs = new List<WaitingChair>();
        list_ExamRooms = new List<ExamRoom>();
        list_Patients = new List<Patient>();

        GameObject[] wc = GameObject.FindGameObjectsWithTag("WaitingChair");
        foreach (GameObject w in wc)
        {
            list_WaitingChairs.Add(w.GetComponent<WaitingChair>());
        }

        GameObject[] er = GameObject.FindGameObjectsWithTag("ExamRoom");
        foreach (GameObject e in er)
        {
            list_ExamRooms.Add(e.GetComponent<ExamRoom>());
        }

        triage = GameObject.FindGameObjectWithTag("Triage").GetComponent<Triage>();

        score_Patients_Total = 0;
        score_Satisfaction = 100f;
    }

    private void Manager_PatientSpawn()
    {
        Patient p = (Instantiate(prefab_Patient) as GameObject).GetComponent<Patient>();
        Debug.Log(p);
        Debug.Log(triage.location_Patient);
        p.Person_Move(triage.location_Patient, "Triage");
        Debug.Log("Adding Patient to the triage");
        triage.PatientObject_Patient_Add(p);
    }
}
