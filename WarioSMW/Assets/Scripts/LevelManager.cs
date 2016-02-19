using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour {

	public void GameOver()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene("Game Over");
    }

    public void StopWalking()
    {
        transform.parent.GetComponent<Player>().FinishWin();
    }

    public void ChangePlayer()
    {
        transform.parent.GetComponent<Player>().ChangePlayer();
    }

    public void Win()
    {
        SceneManager.LoadScene("Level 2");
    }

}
