using UnityEngine;
using System.Collections;

public class Basket : MonoBehaviour
{
    public GameObject score1;
    public GameObject score2;
    public AudioClip basket;
    public GameObject shooter;

    void OnCollisionEnter()
    {
        GetComponent<AudioSource>().Play();
    }

    void OnTriggerEnter()
    {
        if (shooter.GetComponent<GUIText>().text.Equals("Player1 Shoots"))
        {
            int currentScore1 = int.Parse(score1.GetComponent<GUIText>().text) + 1;
            score1.GetComponent<GUIText>().text = currentScore1.ToString();
        }
        else if(shooter.GetComponent<GUIText>().text.Equals("Player2 Shoots"))
        {
            int currentScore2 = int.Parse(score2.GetComponent<GUIText>().text) + 1;
            score2.GetComponent<GUIText>().text = currentScore2.ToString();
        }
        AudioSource.PlayClipAtPoint(basket, transform.position);
    }
}