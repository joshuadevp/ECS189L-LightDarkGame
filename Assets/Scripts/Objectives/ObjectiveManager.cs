using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class ObjectiveManager : MonoBehaviour
{
    [SerializeField]
    ScriptableObject[] possibleObjectives;
    [SerializeField]
    float distToSpawnFromPlayer;
    [SerializeField]
    float distToShowPointer;
    [SerializeField]
    float distPointerFromPlayer;
    [SerializeField]
    GameObject player;
    [SerializeField]
    FogOfDarknessManager darknessManager;
    [SerializeField]
    GameObject objectivePointer;
    [SerializeField]
    TextMeshProUGUI objectiveText;
    [SerializeField]
    List<GameObject> upgradeSelectButtons;
    private List<IUpgrade> upgrades;
    private IObjective activeObjective;

    // Start is called before the first frame update
    void Start()
    {
        upgrades = new List<IUpgrade>();
        UnPauseUpgradeSelect();
        foreach (ScriptableObject o in possibleObjectives)
        {
            if (!(o is IObjective))
            {
                Debug.LogError("Possible objective is not of type IObjective!");
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        // Basic flow
        // Generate new objective(s)
        // Generate reward(s)? Might not get this far
        // Determine location to place obj
        // Setup objective at loc
        // If objective is completed, give player reward(s) choice?
        // Destroy objective object
        // Goto to generate new objective(s)

        if (activeObjective == null)
        {
            activeObjective = GenerateNewObjective();
            activeObjective.Setup(GenerateLocation());
            // TODO: Create reward
        }
        else
        {
            // Point player in direction of objective
            if(Vector3.Magnitude(player.transform.position - (Vector3)activeObjective.GetLocation()) > distToShowPointer)
            {
                objectivePointer.SetActive(true);
                objectivePointer.transform.position = Vector3.MoveTowards(player.transform.position, (Vector3)activeObjective.GetLocation(),distPointerFromPlayer)+new Vector3(0,0,-1f);
                objectivePointer.transform.rotation = Quaternion.LookRotation((player.transform.position - (Vector3)activeObjective.GetLocation()).normalized);
            } else {
                objectivePointer.SetActive(false);
            }

            // Apply description
            objectiveText.text = activeObjective.GetDescription();

            activeObjective.ManualUpdate();
            if (activeObjective.Completed() || Input.GetButtonDown("Objective Cheat"))
            {
                Debug.Log("Completed objective");
                PauseForUpgradeSelect();
                Destroy((ScriptableObject)activeObjective);
                activeObjective = null;
            }
        }
    }

    private IObjective GenerateNewObjective()
    {
        return (IObjective)Instantiate(possibleObjectives[Random.Range(0, possibleObjectives.Length)]);
    }

    private Vector2 GenerateLocation()
    {
        Vector2 playerLoc = player.transform.position;
        // Attempt to find a suitable location to place objective
        // Only try X number of times
        for (int i = 0; i < 100; i++)
        {
            float radian = Mathf.Deg2Rad * Random.Range(0, 360);
            Vector2 tempLoc = playerLoc + new Vector2(Mathf.Cos(radian) * distToSpawnFromPlayer, Mathf.Sin(radian) * distToSpawnFromPlayer);
            if (darknessManager.IsDarkness(tempLoc))
            {
                return tempLoc;
            }
        }

        return new Vector2(50, 50);
    }

    // Pause/Unpause game and show/unshow buttons
    private void PauseForUpgradeSelect()
    {
        upgradeSelectButtons.ForEach(o => o.SetActive(true));
        Time.timeScale = 0;
    }
    private void UnPauseUpgradeSelect()
    {
        upgradeSelectButtons.ForEach(o => o.SetActive(false));
        Time.timeScale = 1;
    }

    // Callbacks for selecting upgrade button
    public void SelectUpgrade1()
    {
        upgrades[0].ApplyUpgrade();
        UnPauseUpgradeSelect();
    }
    public void SelectUpgrade2()
    {
        upgrades[1].ApplyUpgrade();
        UnPauseUpgradeSelect();
    }
    public void SelectUpgrade3()
    {
        upgrades[2].ApplyUpgrade();
        UnPauseUpgradeSelect();
    }
}
