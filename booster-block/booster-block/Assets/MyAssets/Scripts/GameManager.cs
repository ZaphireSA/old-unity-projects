using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.UI;
using System.Linq;

public class GameManager : MonoBehaviour
{

    [SerializeField]
    GameObject wallPrefab, BGPrefab, healthPickupPrefab;
    [SerializeField]
    float heightScale = 1.0F;
    [SerializeField]
    float smoothness;
    float seed1;
    float seed2;
    [SerializeField]
    float minGapSize = 1.5f;

    [SerializeField]
    float moveChance = 1f;

    int healthPickedUp = 0;    

    [SerializeField]
    int colorChangeDistance = 200;

    [SerializeField]
    float heightAddition = 10f;

    Cube player;

    int lastX = -30;

    Color color1;
    Color color2;

    int lastColorChange = 0;

    bool isDead = false;

    [SerializeField]
    TMP_Text txtScore, txtHealth;
    [SerializeField]
    Slider slider;

    float heightChangerY = 0;

    Queue<GameObject> wallsForUse = new Queue<GameObject>();
    List<GameObject> wallList = new List<GameObject>();
    bool hasChangedColor = false;
    private void Awake()
    {
        player = FindObjectOfType<Cube>();
    }

    public void HealthPickedUp()
    {
        healthPickedUp++;
    }

    void Start()
    {
        lastColorChange = -colorChangeDistance;
        color1 = new Color(Random.value, Random.value, Random.value, 1.0f);
        color2 = new Color(Random.value, Random.value, Random.value, 1.0f);
        seed1 = Random.Range(-10000f, 10000f);
        seed2 = Random.Range(-10000f, 10000f);


        for (int i = 0; i < 300; i++)
        {
            var newWall = Instantiate(wallPrefab, new Vector3(-1000, 0, 0), Quaternion.identity);
            wallList.Add(newWall);
            wallsForUse.Enqueue(newWall);
        }
        
        while(wallsForUse.Count >2)
        {
            GenerateMap();
        }
        StartCoroutine(MapChecker());               
    }
   

    IEnumerator MapChecker()
    {
        while (true)
        {
            for (int i = 0; i < wallList.Count; i++)
            {
                if (wallList[i].transform.position.x < player.transform.position.x - 40)
                {
                    if (!wallsForUse.Contains(wallList[i]))
                    {
                        wallsForUse.Enqueue(wallList[i]);
                    }
                }
            }
            GenerateMap();
            //if (Mathf.RoundToInt(player.transform.position.x % colorChangeDistance) == 0)
            if (lastColorChange + colorChangeDistance < player.transform.position.x)
            {
                //if (!hasChangedColor) {
                //    hasChangedColor = true;

                var wall = wallList.FirstOrDefault(x => Mathf.RoundToInt(x.transform.position.x) == Mathf.RoundToInt(player.transform.position.x));
                if (wall != null)
                {
                    player.ChangeColor(wall.GetComponent<Renderer>().material.color);
                }
                lastColorChange = lastColorChange + colorChangeDistance;
            }
               // }
            //} else
            //{                
            //    hasChangedColor = false;
            //}
            yield return new WaitForEndOfFrame();
        }
    }

    private void FixedUpdate()
    {
        //Debug.Log("S: " + player.GetComponent<Rigidbody>().velocity);
        txtScore.text = player.transform.position.x.ToString("0");

        
    }

    public void HealthUpdate(float health)
    {
        slider.value = health / 100;
        txtHealth.text = health.ToString("0");
        if (health <= 0)
        {
            Die();
        } else
        {
            isDead = false;
        }
    }

    public void Die()
    {
        if (!isDead)
        {
            isDead = true;
            StartCoroutine(DieDelay());
        }

    }



    IEnumerator DieDelay()
    {
        var rb = player.GetComponent<Rigidbody>();
        var velocity = rb.velocity.magnitude;
        while(velocity > 1 && isDead)
        {            
            yield return new WaitForSeconds(0.2f);
            velocity = rb.velocity.magnitude;           
        }

        if (!isDead) yield break;

        
        
        
        yield return new WaitForSeconds(1);
        GooglePlayManager.instance.UpdateStats(Mathf.RoundToInt(player.transform.position.x), healthPickedUp);
        yield return new WaitForSeconds(0.5f);
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    void GenerateMap()
    {
        if (wallsForUse.Count < 2) return;

        var newBlock = wallsForUse.Dequeue();
        var newBlock2 = wallsForUse.Dequeue();

        

        //for (float y = 0; y < 50f; y++)
        //{
        lastX++;
        var tempHeightScale = Mathf.Lerp(heightScale, 15, Mathf.Clamp(lastX, 0, 200) / 200);
        if (lastX % colorChangeDistance == 0)
        {
            color1 = new Color(Random.value, Random.value, Random.value, 1.0f);
            color2 = new Color(Random.value, Random.value, Random.value, 1.0f);
        }

        //if (lastX % 10 == 0)
        //{
        heightChangerY += Random.Range(-1f, 1f);
        heightChangerY = Mathf.Clamp(heightChangerY, -5, 5);
        //}

        float height = Mathf.PerlinNoise(seed1, lastX / smoothness) * tempHeightScale;
        newBlock.transform.position = new Vector3(lastX, -(heightAddition / 2) + heightChangerY, 0);
        //var newBlock = Instantiate(wallPrefab, new Vector3(lastX, -(heightAddition/2) + heightChangerY, 0), Quaternion.identity);
        var scale = newBlock.transform.localScale;
        scale.y = height + heightAddition;
        newBlock.transform.localScale = scale;

        float height2 = Mathf.PerlinNoise(seed2, lastX / smoothness) * tempHeightScale;
        //var newBlock2 = Instantiate(wallPrefab, new Vector3(lastX, 15 + (heightAddition/2) + heightChangerY, 0), Quaternion.identity);
        newBlock2.transform.position = new Vector3(lastX, 15 + (heightAddition / 2) + heightChangerY, 0);
        var scale2 = newBlock2.transform.localScale;
        scale2.y = height2 + heightAddition;
        newBlock2.transform.localScale = scale2;

        newBlock.GetComponent<Renderer>().material.color = lastX % 2 == 0 ? color1 : color2;
        newBlock2.GetComponent<Renderer>().material.color = lastX % 2 == 0 ? color1 : color2;

        var minY = newBlock.transform.position.y + (scale.y / 2);
        var maxY = newBlock2.transform.position.y - (scale2.y / 2);
        var gap = maxY - minY;
        
        while (gap < minGapSize)
        {
            var pos1 = newBlock.transform.position;
            pos1.y -= 1;
            newBlock.transform.position = pos1;

            var pos2 = newBlock2.transform.position;
            pos2.y += 1;
            newBlock2.transform.position = pos2;

            minY = newBlock.transform.position.y + (scale.y / 2);
            maxY = newBlock2.transform.position.y - (scale2.y / 2);
            gap = maxY - minY;
        }


        if (lastX == 0)
        {
           // Debug.Log(minY + ": " + maxY + ":" + gap);
            var pos = player.transform.position;
            pos.y = maxY - ((maxY - minY) / 2);
            player.transform.position = pos;
        }


        int randHealth = Random.Range(0, 1000);
        if (randHealth < 20)
        {


            var newPickup = Instantiate(healthPickupPrefab, new Vector3(lastX, Random.Range(minY + 1, maxY - 1), 0), Quaternion.identity);
            newPickup.GetComponent<HealthPickup>().SetColor(color2);

        }

        var randMove1 = Random.Range(0, 100);
        var randMove2 = Random.Range(0, 100);

        newBlock.GetComponent<Wall>().UpdateState(randMove1 <= moveChance);
        newBlock2.GetComponent<Wall>().UpdateState(randMove2 <= moveChance);

    }
}
