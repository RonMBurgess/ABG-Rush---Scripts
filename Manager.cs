using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class Manager : MonoBehaviour {

    //change this later to private and load from resource folder
    public GameObject prefab_Patient;
    public Transform location_Entrance, location_Exit;
    public Texture2D cursor; // change this to a dictionary later depending on how many cursors we have.
	public GameplayUIScript gameplayUI;
	public float timerSpawn = 15f;
	public float timerSpawnRate = 3f;
	public float timerSpawnDelayMin = 25f;
	public static Manager _manager;

	public List<string> namesFirst, namesLast;
    private Triage triage;
    //private List<Patient> list_Patients;
    private List<WaitingChair> list_WaitingChairs;
    private List<ExamRoom> list_ExamRooms;
    private int score_Patients_Total;
	private int scoreCorrectDiagnoses, scoreCorrectInitialAssessment, scoreAngryPatients;
	private int currentPatients;
    private float score_Satisfaction;
	private float timerSpawnUsed;
    private Nurse nurse;
	private ABG abg;
	private Sink sink;


    public Nurse MyNurse
    {
        get { return nurse; }
    }

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

		if (timerSpawnUsed > 0)
		{
			timerSpawnUsed -= Time.deltaTime;
			if (timerSpawnUsed <= 0)
			{
				Manager_PatientSpawn();
				
			}
		}

		if (score_Satisfaction > 0)
		{
			score_Satisfaction -= Time.deltaTime / 6f;
			gameplayUI.satisfaction.SatisfactionUpdate(score_Satisfaction);
			if (score_Satisfaction <= 0)
			{
				GameOver();
			}
		}


	}

    #region Patient Leaving
    //I should definitely come back and revamp this. This can easily be simplified and molded into a single function depending on what we have each of them do.

    /// <summary>
    /// Leave the practice / emergency facility angrily. 
    /// </summary>
    /// <param name="p">The Patient leaving</param>
    public void Manager_Patient_StormOut(Patient p)
    {
		//update the amount of patients.
		currentPatients--;
		//spawn another patient if we are down to 0.
		if (currentPatients <= 0)
		{
			Manager_PatientSpawn();
		}

        //list_Patients.Remove(p);

        //Perform some kind of animation

		//update the total amount of patients to leave angry
		scoreAngryPatients++;

        //decrease points based on level of satisfaction
		UpdateSatisfactionScore(-5);

		//inform abg that the diagnosis is no longer being used.
		//abg.PatientDiagnosisComplete(p.MyDiagnosis());

		//make the patient go to the exit
        p.Person_Move(location_Exit.position,"Exit");
    }

    /// <summary>
    /// Leave the facility happily, and award points
    /// </summary>
    /// <param name="p"></param>
    public void Manager_Patient_Leave(Patient p)
    {
		//update the amount of patients.
		currentPatients--;
		//spawn another patient if we are down to 0.
		if (currentPatients <= 0)
		{
			Manager_PatientSpawn();
		}

        //list_Patients.Remove(p);
        //perform some kind of animation

		//Determine if the initial assessment of the patient was correct.
		bool rm = (p.InitialAssessmentGetRM() == p.MyDiagnosis().Answer_Respiratory_Metabolic);
		bool aa = (p.InitialAssessmentGetAA() == p.MyDiagnosis().Answer_Acidosis_Alkalosis);

		//update score
		scoreCorrectDiagnoses++;
		if (rm && aa)
		{
			scoreCorrectInitialAssessment++;
		}

		gameplayUI.PatientFeedback().PatientFeedback(p, rm, aa);
		gameplayUI.PatientFeedback().gameObject.SetActive(true);
		Time.timeScale = 0;
        //gain/increase points based on level of satisfaction
		UpdateSatisfactionScore(6);

		//inform abg that the diagnosis is no longer being used.
		//abg.PatientDiagnosisComplete(p.MyDiagnosis());

        p.Person_Move(location_Exit.position, "Exit");
    }

    #endregion
	#region Check Hotspots
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
	#endregion

	/// <summary>
    /// Set up the manager
    /// </summary>
    private void Manager_Initialize()
    {
		//Initialize the ABG class and prepare all the diagnoses
		abg = new ABG("NursingInterventions");

		//Initialize the lists for waiting chairs, Examination rooms, and Patients.
        list_WaitingChairs = new List<WaitingChair>();
        list_ExamRooms = new List<ExamRoom>();
        //list_Patients = new List<Patient>();

		//Populate the list of waiting chairs.
        GameObject[] wc = GameObject.FindGameObjectsWithTag("WaitingChair");
        foreach (GameObject w in wc)
        {
            list_WaitingChairs.Add(w.GetComponent<WaitingChair>());
        }

		//Populate the list of exam rooms.
        GameObject[] er = GameObject.FindGameObjectsWithTag("ExamRoom");
        foreach (GameObject e in er)
        {
            list_ExamRooms.Add(e.GetComponent<ExamRoom>());
        }

		//Find the triage
        triage = GameObject.FindGameObjectWithTag("Triage").GetComponent<Triage>();

		//Find the nurse
        nurse = GameObject.FindGameObjectWithTag("Nurse").GetComponent<Nurse>();

		//Find the Sink
		sink = GameObject.FindGameObjectWithTag("Sink").GetComponent<Sink>();

		//Reset the score
        score_Patients_Total = 0;
		scoreAngryPatients = 0;
		scoreCorrectDiagnoses = 0;
		scoreCorrectInitialAssessment = 0;
		score_Satisfaction = 100f;
		gameplayUI.satisfaction.SatisfactionUpdate(score_Satisfaction);
		currentPatients = 0;
		//reset the spawn timer
		timerSpawnUsed = 0.1f;

		//set the manager
		_manager = this;
    }

    private void Manager_PatientSpawn()
    {
		//prepare a dob
		string dob = Random.Range(1, 13).ToString() + "/" + Random.Range(1, 29).ToString() + "/" + Random.Range(1940, 2000).ToString();

        Patient p = (Instantiate(prefab_Patient,location_Entrance.position, prefab_Patient.transform.rotation) as GameObject).GetComponent<Patient>();

		p.Patient_Setup(namesFirst[Random.Range(0, namesFirst.Count)] + " " + namesLast[Random.Range(0, namesLast.Count)], dob, abg.PatientDiagnosis());
        //Debug.Log(p);
        //Debug.Log(triage.location_Patient);
        //p.Person_Move(triage.location_Patient, "Triage");
        //Debug.Log("Adding Patient to the triage");
        //triage.PatientObject_Patient_Add(p);
        triage.Triage_Patient_Add(p);
		currentPatients++;

		timerSpawn -= timerSpawnRate;
		timerSpawn = Mathf.Clamp(timerSpawn, timerSpawnDelayMin, 120f);
		//allow patient to spawn either 10 seconds sooner or 10 seconds after base time
		timerSpawnUsed = Random.Range(-10, 11);
		timerSpawnUsed += timerSpawn;
    }

    /// <summary>
    /// Change the cursor based on what is currently moused over.
    /// </summary>
    /// <param name="enter"></param>
    public void Manager_MouseOver(bool enter)
    {
        //come back and update this later to accept parameters to use multiple cursors. Will most likely accept a string and look through a dictionary.
        if (enter)
        {
            Cursor.SetCursor(cursor, Vector2.zero, CursorMode.Auto);
        }
        else
        {
            Cursor.SetCursor(null, new Vector2(0, 0), CursorMode.Auto);
        }
        
    }





	public GameplayUIScript GamePlayUI()
	{
		return gameplayUI;
	}

	public void UpdateSatisfactionScore(int s){
		score_Satisfaction += s;
		//clamp the score so it cannot go above 100
		score_Satisfaction = Mathf.Clamp(score_Satisfaction, 0f, 100f);

		gameplayUI.satisfaction.SatisfactionModify(s);

		if (score_Satisfaction <= 0)
		{
			GameOver();
		}
	}

	public Sink MySink()
	{
		return sink;
	}

	private void GameOver()
	{
		gameplayUI.GameOverUI().GameOver(scoreAngryPatients,scoreCorrectDiagnoses,scoreCorrectInitialAssessment);
		Time.timeScale = 0;
		gameplayUI.GameOverUI().gameObject.SetActive(true);
		
	}
}
