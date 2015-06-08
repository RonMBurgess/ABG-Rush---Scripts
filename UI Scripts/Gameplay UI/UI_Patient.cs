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
                manager = GameObject.Find("Manager").GetComponent<Manager>();
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
}
