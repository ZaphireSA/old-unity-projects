using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class GameManager : MonoBehaviour
{
    public CanvasGroup menuCanvas;
    [SerializeField]
    TextMeshProUGUI txtHealth;
    [SerializeField]
    Slider slider;
    bool isAlive = true;

    [SerializeField]
    float zombieMultiplier, zombiesToSpawn, spawnRate;

    [SerializeField]
    GameObject zombiePrefab;

    private void Start()
    {
        StartCoroutine(Spawner());
    }

    IEnumerator Spawner()
    {        
        while(isAlive)
        {            
            var spawns = GameObject.FindGameObjectsWithTag("Spawner");
            for(int i = 0; i < Mathf.FloorToInt(zombiesToSpawn); i++)
            {
                var spawn = spawns[Random.Range(0, spawns.Length - 1)];                
                Instantiate(zombiePrefab, spawn.transform.position, spawn.transform.rotation);
            }
            zombiesToSpawn *= zombieMultiplier;
            yield return new WaitForSeconds(spawnRate);
        }
    }

    public void PlayerDied()
    {
        menuCanvas.interactable = true;
        menuCanvas.blocksRaycasts = true;
        menuCanvas.alpha = 1;
        isAlive = false;
    }

    // Start is called before the first frame update
    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void HealthUpdated(float value) {
        txtHealth.text = value.ToString();
        slider.value = value;
    }
}
