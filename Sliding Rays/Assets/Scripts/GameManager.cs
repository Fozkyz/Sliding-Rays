using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class GameManager : MonoBehaviour
{
    public GameObject line_prefab;
    public GameObject obstacle_prefab;
    public PlayerMovement player;
    public Transform[] spawn_points;
    public TextMeshProUGUI score_UI;
    public TextMeshProUGUI highscore_UI;
    public RectTransform arrow_UI;
    public GameObject game_over_UI;
    public GameObject pause_UI;
    public GameObject press_start_UI;
    public GameObject highscores_UI;
    public TMP_InputField name_input_field;
    
    public float playerSpeed;

    public int grid_size;

    public float spawning_speed_start;
    public float spawning_speed_max;
    public float spawning_speed_mult;

    ScoreManager score_manager;
    
    float spawning_speed;


    public float time;
    float obstacle_speed;

    int score; // Current score
    int highscore; // Highest Score in highscore table
    public bool is_playing;
    bool waiting_to_start;
    public bool is_paused;

    int next_direction; // Determine in which direction will the next ray move (between 0 and 3)
    
    void Awake()
	{
        score_manager = gameObject.GetComponent<ScoreManager>();
        name_input_field.characterLimit = 3;
	}

	void Start()
    {
        CreateBorder(grid_size * 1.02f);
        ResetGame();
    }

    // Update is called once per frame
    void Update()
    {
        if (is_playing && !is_paused)
		{
            time += Time.deltaTime;
            if (time >= (1 / spawning_speed))
            {
                SpawnObstacle();
                time = 0f;
            }
            if (Input.GetKeyDown(KeyCode.Escape))
			{
                PauseGame();
			}
        }
        else if (waiting_to_start)
		{
            if (Input.GetKeyDown(KeyCode.Space))
			{
                StartGame();
			}
		}
        else if (is_paused)
		{
            if (Input.GetKeyDown(KeyCode.Escape))
			{
                StartGame();
			}
		}
    }

    public void ResetGame()
	{
        highscores_UI.SetActive(false);
        game_over_UI.SetActive(false);
        is_playing = false;
        waiting_to_start = true;
        player.transform.position = Vector2.zero;
        score = 0;
        score_UI.text = score.ToString();
        highscore = score_manager.GetHighscore();
        highscore_UI.text = highscore.ToString();
        next_direction = Random.Range(0, 4);
        arrow_UI.rotation = Quaternion.Euler(0, 0, next_direction * 90);
        spawning_speed = spawning_speed_start;
        time = 0;
        press_start_UI.SetActive(true);
        SpriteRenderer renderer = player.gameObject.GetComponent<SpriteRenderer>();
        if (renderer != null)
        {
            renderer.enabled = true;
        }
    }

    public void StartGame()
	{
        pause_UI.SetActive(false);
        press_start_UI.SetActive(false);
        if (player != null)
        {
            player.speed = playerSpeed;
            is_playing = true;
            is_paused = false;
            waiting_to_start = false;
        }
    }

    void PauseGame()
	{
        is_playing = false;
        is_paused = true;
        waiting_to_start = false;
        pause_UI.SetActive(true);
        // Display Pause screen
	}

    void InstantiateLine(Vector2 v1, Vector2 v2)
	{
        LineRenderer line = Instantiate(line_prefab).GetComponent<LineRenderer>();
        line.SetPosition(0, v1);
        line.SetPosition(1, v2);
        line.startWidth = .05f;
        line.endWidth = .05f;
    }

    void CreateBorder(float size)
	{
        InstantiateLine(new Vector2(-.5f, .5f) * size, new Vector2(.5f, .5f) * size);
        InstantiateLine(new Vector2(.5f, .5f) * size, new Vector2(.5f, -.5f) * size);
        InstantiateLine(new Vector2(.5f, -.5f) * size, new Vector2(-.5f, -.5f) * size);
        InstantiateLine(new Vector2(-.5f, -.5f) * size, new Vector2(-.5f, .5f) * size);
    }

    void SpawnObstacle()
	{
        Transform point = spawn_points[next_direction];
        Obstacle obstacle = Instantiate(obstacle_prefab, point.position, point.rotation, transform).GetComponent<Obstacle>();
        next_direction = Random.Range(0, 4);
        arrow_UI.rotation = Quaternion.Euler(0, 0, next_direction * 90);
        // Change the speed to get a nice curve with a max value
        spawning_speed = spawning_speed_max * (1 - Mathf.Exp(-spawning_speed_mult * (score + 1)));
    }

    public void PlayerLost()
	{
        is_playing = false;
        player.speed = 0f;
        SpriteRenderer renderer = player.gameObject.GetComponent<SpriteRenderer>();
        if (renderer != null)
		{
            renderer.enabled = false;
		}
        DisplayGameOver();
	}

    public void DisplayGameOver()
	{
        game_over_UI.SetActive(true);        
	}

    public void PlayerScored()
	{
        if (is_playing)
		{
            score++;
            score_UI.text = score.ToString();
        }
        else
		{
            //DisplayGameOver();
		}
    }

    public void SaveHighscore()
	{
        if (name_input_field.text.Length == 3)
		{
            score_manager.AddHighscoreEntry(name_input_field.text, score);
        }
	}

    public void DisplayHighscores()
	{
        game_over_UI.SetActive(false);
        highscores_UI.SetActive(true);
        score_manager.DisplayHighscores();
	}
}
