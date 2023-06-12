using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    [field: SerializeField] public AudioManager AudioManager { get; private set; }
    public bool Pausing { get; private set; }
    public Canvas InGameCanvas;
    public Canvas UICanvas;

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
            Pausing = !Pausing;
        }
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

    private IEnumerator TogglePause()
    {
        float savedTimeScale;
        while (true) {
            yield return new WaitUntil(() => Pausing == true);
            savedTimeScale = Time.timeScale;
            Time.timeScale = 0;
            yield return new WaitUntil(() => Pausing == false);
            Time.timeScale = savedTimeScale;
        }
    }
}
