using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { private set; get; }

    [SerializeField] private int NumberOfTargets;
    [SerializeField] private GameObject pfTarget;
    //[SerializeField] private GameObject GameOverUI;
    [SerializeField] private PlayerController playerController;
    [SerializeField] private GameObject MenuUI;
    [SerializeField] private GameObject GameOverUI;

    public static bool GameIsPaused = false;

    private float spawnTime = 5f;
    private float xPos;
    private float zPos;
    private float yPos;
    private int numberSpawned = 0;


    private List<GameObject> targetList = new List<GameObject>();

    private void Awake()
    {
        Instance = this;
        GameOverUI.SetActive(false);
        playerController.enabled = false;
        MenuUI.SetActive(true);
        GameIsPaused = true;
        Time.timeScale = 0f;
    }

    private void Start()
    {
        StartCoroutine(SpawnEnemies());
    }

    IEnumerator SpawnEnemies()
    {
        while (spawnTime >= 5f)
        {
            Debug.Log("ENTRA");
            while (numberSpawned < NumberOfTargets)
            {
                xPos = Random.Range(-12.92f, -98.42f);
                zPos = Random.Range(-47.49f, 48.11f);
                yPos = Random.Range(0, 7.71f);
                var target = Instantiate(pfTarget, new Vector3(xPos, yPos, zPos), Quaternion.Euler(0,90f,0));
                targetList.Add(target);
                yield return new WaitForSeconds(0.1f);
                numberSpawned += 1;
            }
            yield return new WaitForSeconds(spawnTime);
            Debug.Log($"PASO {spawnTime} segundos");
            
            numberSpawned = 0;
        }
    }

    public void StopGame()
    {
        spawnTime = 0f;
        StopAllCoroutines();
        foreach (var target in targetList)
        {
            Destroy(target);
        }
        playerController.enabled = false;
        GameIsPaused = true;
        CanvasManager.Instance.UpdateFinalPoints();
        GameOverUI.SetActive(true);
    }

    public void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void StartGame()
    {
        GameOverUI.SetActive(false);
        MenuUI.SetActive(false);
        GameIsPaused = false;
        Time.timeScale = 1f;
        playerController.enabled = true;
    }
}
