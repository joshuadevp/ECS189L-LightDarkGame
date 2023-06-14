using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    [field: SerializeField] public AudioManager AudioManager { get; private set; }
    public bool Pausing { get; private set; }
    public Canvas InGameCanvas;
    public Canvas UICanvas;
    public GameObject WinCanvas;
    public GameObject LoseCanvas;
    public GameObject PauseCanvas;
    public GameObject objectiveCanvas;
    [SerializeField] private ObjectiveManager objectiveManager;
    [SerializeField] private DarknessSettings defaultDarknessSettings;
    [SerializeField] private DarknessSettings globalDarknessSettings;
    [SerializeField] private float updateInterval;
    [SerializeField] private float spawnScale;
    [SerializeField] private float spreadScale;
    private float time;

    [Header("Game Difficulty Settings")]
    [Tooltip("How much stronger each enemy gets per second")]
    [SerializeField] private float enemyStrengthOverTimeMultiplier = 0.01f;
    [Tooltip("How much stronger each enemy gets per level")]
    [SerializeField] private float enemyStrengthOverLevelMultiplier = 1f;

    [SerializeField] GameObject enemyPrefab;
    [SerializeField] GameObject damageInfoPrefab;

    private float startTime;
    private float level = 0;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(this);
            this.enabled = false;
        }
        startTime = Time.time;
        Pausing = false;

        WinCanvas.SetActive(false);
        LoseCanvas.SetActive(false);
        PauseCanvas.SetActive(false);

        // Init darkness
        var fogManager = GameObject.FindFirstObjectByType<FogOfDarknessManager>();
        fogManager.InitAllDarkness();
        // Clear out darkness center
        fogManager.RemoveDarknessPointsCircle(new Vector2(150,150),20);
        // Set player to center of darkness
        GameObject.FindFirstObjectByType<Player>().gameObject.transform.position = new Vector2(150,150);
    }
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(TogglePause());
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Cancel"))
        {
            GameManager.Instance.AudioManager.PlayOneShot("Cancel");
            Pausing = !Pausing;
        }
        if(objectiveManager.CompletedCount() >= 10)
        {
            Win();
        }
        // Interval update
        if(time > updateInterval)
        {
            time = 0;
            globalDarknessSettings.EnemySpawnChance = defaultDarknessSettings.EnemySpawnChance +  spawnScale * CalculateEnemyModifier();
            globalDarknessSettings.DarknessSpreadChance = defaultDarknessSettings.DarknessSpreadChance +  spreadScale * CalculateEnemyModifier();
        }
        time+=Time.deltaTime;
    }

    public void SpawnDamageInfo(Vector3 position, float damage)
    {
        GameObject dmgInfo = Instantiate(damageInfoPrefab, InGameCanvas.transform);
        dmgInfo.transform.position = position;
        dmgInfo.GetComponent<TMPro.TMP_Text>().text = damage.ToString();
        Destroy(dmgInfo, 1.5f);
    }

    /// <summary>
    /// To be fine tuned for game feel: strengthen enemy stats based on time and level
    /// </summary>
    /// <returns> The modifier to be applied on enemy's stats</returns>
    public float CalculateEnemyModifier()
    {
        return 1 + (Time.time - startTime) * enemyStrengthOverTimeMultiplier + level * enemyStrengthOverLevelMultiplier;
    }

    // Callback for main menu button
    public void MainMenu()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene("MainMenu");
    }

    public void GameOver()
    {
        GameManager.Instance.AudioManager.PlayOneShot("Explosion");
        objectiveCanvas.SetActive(false);
        Time.timeScale = 0;
        LoseCanvas.SetActive(true);
    }

    public void Win()
    {
        GameManager.Instance.AudioManager.PlayOneShot("Wave p");
        objectiveCanvas.SetActive(false);
        Time.timeScale = 0;
        WinCanvas.SetActive(true);
    }

    public void IncreaseLevel(float value)
    {
        level += value;
    }

    private IEnumerator TogglePause()
    {
        float savedTimeScale = 1;
        while (true) {
            yield return new WaitUntil(() => Pausing == true);
            // Do not pause if game is already paused for a different reason
            if(Time.timeScale!=0){
            savedTimeScale = Time.timeScale;
            Time.timeScale = 0;
            PauseCanvas.SetActive(true);
            } else {
                Pausing = false;
                continue;
            }
            yield return new WaitUntil(() => Pausing == false);
            Time.timeScale = savedTimeScale;
            PauseCanvas.SetActive(false);
        }
    }
    public void HoverOverButton()
    {
        GameManager.Instance.AudioManager.PlayOneShot("Click 2");
    }
}
