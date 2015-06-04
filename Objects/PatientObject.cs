using UnityEngine;
using System.Collections;

public class PatientObject : OfficeObject {


    public Vector2 location_Patient;
    private Patient patient;

    #region Patient

    public Patient MyPatient
    {
        get { return patient; }
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
}
