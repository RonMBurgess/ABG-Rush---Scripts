using UnityEngine;
using System.Collections;

public class PatientObject : OfficeObject {


    public Vector2 location_Patient;
    private Patient patient;
    private UI_Patient ui;

    #region Patient

    public Patient MyPatient
    {
        get { return patient; }
    }

    public UI_Patient MyUI
    {
        get { return ui; }
        set { Debug.Log(value.name);
            ui = value; 
            ui.gameObject.SetActive(false);
        }
    }

    /// <summary>
    /// Add a new patient
    /// </summary>
    /// <param name="p">Patient</param>
    public void PatientObject_Patient_Add(Patient p)
    {
        patient = p;
        p.Patient_Hotspot(this);
        OfficeObject_SetReadyState(true);
    }

    /// <summary>
    /// Remove the current patient
    /// </summary>
    public void PatientObject_Patient_Remove()
    {
        patient = null;
        OfficeObject_SetReadyState(false);
    }

    /// <summary>
    /// Is this object occupied?
    /// </summary>
    /// <returns>True - occupied, false - free</returns>
    public bool PatientObject_Occupied()
    {
        return patient != null;
    }

    #endregion

    /// <summary>
    /// The location the patient should move to
    /// </summary>
    /// <returns></returns>
    public Vector2 PatientObject_LocationPatient()
    {
        return location_Patient;
    }


    /// <summary>
    /// Open up the UI for whatever the player has recently clicked on.
    /// </summary>
    public void PatientObject_OpenUI()
    {
        //set the patient information for the UI
        Debug.Log("Patient's name is " + patient.name);
        ui.MyPatient = patient;
        //turn the UI on.
        ui.gameObject.SetActive(true);
    }
}
