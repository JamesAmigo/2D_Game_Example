using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Goal : MonoBehaviour
{
    [SerializeField] private UIManager uiManager;

    private void OnTriggerEnter2D(Collider2D other) 
    {
        if (other.gameObject.tag == "Player") 
        {
            other.gameObject.GetComponent<ExamplePlayerController>().PlayerWin();
            uiManager.ShowResultPanel(true);
        }
    }
}
