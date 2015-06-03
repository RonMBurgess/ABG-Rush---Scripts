using UnityEngine;
using System.Collections;

public class GameplayUIScript : MonoBehaviour {

    ///Summary
    ///This class will be the Go-Between for the UI in the gameplay screen and the manager that is handling everything
    ///


    public ABGToolManagerScript abgTool;
    public NotificationManagerScript notifications;
    public SatisfactionBarScript satisfaction;
    public MeetAndGreetScript meetandgreet;



	// Use this for initialization
	void Start () {
        meetandgreet.gameObject.SetActive(false);
        abgTool.gameObject.SetActive(false);

	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyUp(KeyCode.RightArrow))
        {
            satisfaction.SatisfactionModify(Random.Range(1, 4));
        }
        else if (Input.GetKeyUp(KeyCode.LeftArrow))
        {
            satisfaction.SatisfactionModify(Random.Range(-4, 1));
        }
        else if (Input.GetKeyUp(KeyCode.N))
        {
            notifications.NewNotification();
        }

	}

    public void ABGToolUse(bool t)
    {
        abgTool.gameObject.SetActive(t);

    }
}
