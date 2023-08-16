using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameOverManager : MonoBehaviour
{
    [SerializeField] GameObject gameOverScreen;
    [SerializeField] Text finalScore;
    [SerializeField] Text currentScore;

    public void SetGameOver()
    {
        gameOverScreen.SetActive(true);
        finalScore.text = PointController.points.ToString();
    }
    public void restartGame()
    {
      SceneManager.LoadScene(0);
    }
}
