using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DeathZone : MonoBehaviour
{
    [SerializeField] private UIManager uiManager;
    private void OnCollisionEnter2D(Collision2D collision) 
    {
        if (collision.gameObject.tag == "Player") 
        {
            collision.gameObject.GetComponent<ExamplePlayerController>().PlayerDeath();
            uiManager.ShowResultPanel(false);
        }
    }
}
