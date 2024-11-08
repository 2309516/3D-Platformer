using UnityEngine;
using UnityEngine.UI;
using grappleGun;

public class SpringJointScore : MonoBehaviour
{
    public int scoreIncrement = 1;
    private Text scoreText;
    private int currentScore = 0;
    grappleGun.GrappleGunFix is
    private bool springJointDetected = false;

    void Start()
    {
        scoreText = GameObject.Find("score").GetComponent<Text>();
        UpdateScoreText();
    }

    void Update()
    {
        //if (!springJointDetected && GetComponent<SpringJoint>() != null)
       // {
            //springJointDetected = true;
            //currentScore += scoreIncrement;
            //UpdateScoreText();
            //Destroy(gameObject);
        //}
    }

    private void UpdateScoreText()
    {
        scoreText.text = "Score: " + currentScore.ToString();
    }
 
   
}
