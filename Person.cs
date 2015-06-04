using UnityEngine;
using System.Collections;


[RequireComponent(typeof(PolyNavAgent))]

/// <summary>
/// The purpose of this class is to be the parent of both the nurse and patients. This will contain functions or variables that both will use.
/// </summary>
public class Person : MonoBehaviour {

    private PolyNavAgent agent;
    private string destinationName;
    private Manager manager;


    public Manager Manager
    {
        get { return manager; }
    }


    /// <summary>
    /// Prepare all variables and components
    /// </summary>
    public void Person_Initialize()
    {
        Debug.Log("Initializing Person");
        agent = GetComponent<PolyNavAgent>();
        destinationName = "";
        if(GameObject.Find("Manager")) manager = GameObject.Find("Manager").GetComponent<Manager>();
    }


    /// <summary>
    /// Move to specified location. Called by Hotspot or Manager
    /// </summary>
    /// <param name="m">Location to move to</param>
    /// <param name="destinationName">Tag of destination</param>
    public void Person_Move(Vector2 m, string dName)
    {
        if (agent == null)
        {
            agent = GetComponent<PolyNavAgent>();
        }
        agent.SetDestination(m, Person_MovementStatus);
        destinationName = dName;
    }

    /// <summary>
    /// Called after a movement action has been taken
    /// </summary>
    /// <param name="moved">True - Reached Destination | False - Did not reach Destination</param>
    private void Person_MovementStatus(bool moved)
    {
        //inform self that my status has changed, so if I'm a nurse, I should now be either waiting at triage, waiting at waiting room, or waiting in patient room
        if (moved)
        {
            if (gameObject.CompareTag("Patient"))
            {
                (this as Patient).Patient_LocationChange(destinationName);
            }
        }
    }
}
