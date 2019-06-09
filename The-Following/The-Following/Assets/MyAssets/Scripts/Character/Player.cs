using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Player : MonoBehaviour {

    public float detectVisualDistance = 1;
    public float detectHearingDistance = 0;
    List<FollowerAI> followers = new List<FollowerAI>();
    
    [SerializeField]
    float biteDistance = 0.5f;

    [SerializeField]
    GameObject followerPrefab;
    [SerializeField]
    GameObject soulPrefab;
    [SerializeField]
    float followerPosFactor = 2f;

    [SerializeField]
    GameObject nextStagePrefab;
    [SerializeField]
    int requiredProtein = 5;

    [SerializeField]
    int currentProtein = 0;

    [SerializeField] AudioSource audioSource;
    [SerializeField] AudioClip biteAudio;
    [SerializeField] AudioClip convertAudio;
    [SerializeField] AudioClip absorbAudio;
    [SerializeField] AudioClip pickupAudio;

    private void Awake()
    {
        audioSource = GetComponentInChildren<AudioSource>();
    }

    private void Start()
    {
        StartCoroutine(BiteEnemies());
        StartCoroutine(ControlFollowers());
        FindObjectOfType<CameraFollow>().target = transform;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            ExplodeFollowers();
        }
    }

    void ExplodeFollowers()
    {
        if (followers.Count > 0) audioSource.PlayOneShot(convertAudio);
        while(followers.Count > 0)
        {
            var follower = followers[0];
            followers.RemoveAt(0);
            if (follower != null)
            {
                Instantiate(soulPrefab, follower.transform.position, Quaternion.identity);
                Destroy(follower.gameObject);
            }
        }
    }

    IEnumerator ControlFollowers()
    {
        while (true)
        {
            float x = -3;
            float y = 1;
            for (int i = 0; i < followers.Count; i++)
            {
                x += 2;
                FollowerAI fol = followers[i];
                if (fol == null) continue;
                Vector3 targetPos = Vector3.zero;
                targetPos.z += y * followerPosFactor;
                targetPos.x += x * followerPosFactor;
                fol.SetTarget(transform.TransformPoint(-targetPos));

                

                if (x == y)
                {
                    y++;
                    x = -y -2;
                }
                
            }
            yield return new WaitForSeconds(0.2f);
        }
    }

    public void GiveProtein(int amount)
    {
        currentProtein += amount;
        if (currentProtein >= requiredProtein && nextStagePrefab != null)
        {
            var newPlayer = Instantiate(nextStagePrefab, transform.position, transform.rotation);
            Player p = newPlayer.GetComponent<Player>();
            p.followers = followers;
            Destroy(gameObject);      
        }
    }

    IEnumerator BiteEnemies()
    {
        while(true)
        {
            var enemyList = FindObjectsOfType<HumanAI>().Where(x => Vector3.Distance(transform.position, x.transform.position) < biteDistance).ToList();
            for (int i = 0; i < enemyList.Count; i++)
            {
                audioSource.PlayOneShot(biteAudio);
                var newFollower = Instantiate(followerPrefab, enemyList[i].transform.position, enemyList[i].transform.rotation);
                Renderer rend = newFollower.GetComponentInChildren<FollowerAI>().rend;
                if (rend != null)
                {
                    rend.materials[1].color = enemyList[i].shirtColor;
                    rend.materials[2].color = enemyList[i].pantsColor;
                    rend.materials[3].color = enemyList[i].shoeColor;
                }
                followers.Add(newFollower.GetComponent<FollowerAI>());
                Destroy(enemyList[i].gameObject);
            }
            yield return new WaitForSeconds(0.2f);
        }
    }

    public void PlayPickupClip()
    {
        audioSource.PlayOneShot(pickupAudio);
    }

    public void PlayAbsorbClip()
    {
        audioSource.PlayOneShot(absorbAudio);
    }

}
