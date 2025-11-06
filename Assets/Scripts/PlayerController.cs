using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;


public class Pla : MonoBehaviour
{
    private float elapsedTime = 0f;
    private float score = 0f;
    private float highscore = 0f;
    public float scoreMultiplier = 10f;
    public float thrustForce = 1f;
    Rigidbody2D rb;
    public GameObject boosterFlame;
    public UIDocument uiDocument;
    private Label scoreText;
    private Button restartButton;
    public GameObject explosionEffect;
    private Label highScoreText;



    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        scoreText = uiDocument.rootVisualElement.Q<Label>("ScoreLabel");
        restartButton = uiDocument.rootVisualElement.Q<Button>("RestartButton");
        restartButton.style.display = DisplayStyle.None;
        restartButton.clicked += ReloadScene;
        highScoreText = uiDocument.rootVisualElement.Q<Label>("HighScore");
        highScoreText.style.display = DisplayStyle.None;

        highscore = PlayerPrefs.GetFloat("HighScore", 0f);

    }

    // Update is called once per frame
    void Update()
    {
        
        elapsedTime += Time.deltaTime;
        score = Mathf.FloorToInt(elapsedTime * scoreMultiplier);

        
        //Debug.Log("Score : " + score);
        scoreText.text = "Score: " + score;
        


        if (Mouse.current.leftButton.isPressed)
        {
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Mouse.current.position.value);
            //Debug.Log("Mouse Position : " + mousePos);

            Vector2 direction = (mousePos - transform.position).normalized;
            transform.up = direction;
            rb.AddForce(direction * thrustForce);


        }

        if (Mouse.current.leftButton.wasPressedThisFrame)
        {
            boosterFlame.SetActive(true);
        }
        else if (Mouse.current.leftButton.wasReleasedThisFrame)
        {
            boosterFlame.SetActive(false);
        }

    }


    void OnCollisionEnter2D(Collision2D collision)
    {
        
        if (score > highscore)
        {
            highscore = score;
            PlayerPrefs.SetFloat("HighScore", highscore);
            PlayerPrefs.Save();
        }
        else if (score < highscore)
        {
            highscore = highscore;
        }

        Destroy(gameObject);
        Instantiate(explosionEffect, transform.position, transform.rotation);
        restartButton.style.display = DisplayStyle.Flex;
        highScoreText.style.display = DisplayStyle.Flex;

        highScoreText.text = "High Score : " + highscore;
        //Debug.Log("High Score : " + highscore);




    }

    void ReloadScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);        
    }


}
