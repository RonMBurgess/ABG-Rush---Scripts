using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class UI_GameOver : MonoBehaviour {

	public Text textfieldAngryPatients, textfieldCorrectDiagnoses, textfieldCorrectAssessments;


	public void GameOver(int angrypatients, int correctdiagnosis, int correctassessments)
	{
		if (textfieldAngryPatients && textfieldCorrectAssessments && textfieldCorrectDiagnoses)
		{
			textfieldCorrectDiagnoses.text = correctdiagnosis.ToString();
			textfieldCorrectAssessments.text = correctassessments.ToString();
			textfieldAngryPatients.text = angrypatients.ToString();
		}

		
	}


	public void MainMenu()
	{
		Time.timeScale = 1;
		Application.LoadLevel(1);
	}
}
