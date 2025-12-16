using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class GameManager : MonoBehaviour
{
    //[SerializeField] PlayerMove playerScript;

    public TextMeshProUGUI goalText;
    public TextMeshProUGUI healthText;

    [SerializeField] private GameObject gameOver;

    void Awake()
    {
        //playerScript = GetComponent<PlayerMove>();
    }

    // Update is called once per frame
    void Update()
    {
        if (goalText.text == "You need to win: 0" || healthText.text == "Health: 0")
        {
            gameOver.gameObject.SetActive(true);
            Time.timeScale = 0;
        }

        if (Input.GetKeyDown(KeyCode.Escape)) 
        {
            gameOver.gameObject.SetActive(true);
            Time.timeScale = 0;
            Cursor.lockState = CursorLockMode.None;
        }
    }
}
