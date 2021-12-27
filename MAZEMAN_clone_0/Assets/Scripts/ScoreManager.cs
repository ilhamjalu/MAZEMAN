using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Photon.Pun;
using Photon.Realtime;

public class ScoreManager : MonoBehaviour
{
    public int myScore, enemyScore;
    public TextMeshProUGUI myScoretext, enemyScoreText;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        myScoretext.text = "My Score : " + myScore.ToString();
        enemyScoreText.text = "Enemy Score : " + enemyScore.ToString();
    }
}
