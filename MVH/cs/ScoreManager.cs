using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class ScoreManager : MonoBehaviour
{
    public int score = 0;
    public TextMeshProUGUI scoreUI;

    private void Start()
    {
        scoreUI.text = "Skóre: " + score;
    }
    // Start is called before the first frame update
    public void Add()
    {
        score++;
        scoreUI.text = "Skóre: " + score;
    }
}
