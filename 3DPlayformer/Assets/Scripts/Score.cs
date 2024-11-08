using UnityEngine;
using TMPro;
using grappleGun;

public class SpringJointScore : MonoBehaviour
{
    public int scoreIncrement = 1;
    private TMP_Text scoreText;
    private int currentScore = 0;
    private bool springJointDetected = false;
    private SpringJoint springJoint;

    void Start()
    {
        scoreText = GameObject.Find("score").GetComponent<TMP_Text>();
        UpdateScoreText();
        springJoint = GetComponent<SpringJoint>();
    }

    void Update()
    {
        CheckForSJ();
    }
    private void CheckForSJ()
    {
        if (!springJointDetected && springJoint != null)
        {
            springJointDetected = true;
            currentScore += scoreIncrement;
            UpdateScoreText(); 
        }
    }
    private void UpdateScoreText()
    {
        scoreText.text = "Score: " + currentScore.ToString();
    }
}
