using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropOnDeath : MonoBehaviour {

    public int minDrops = 0;
    public int maxDrops = 4;
    public DropItem[] dropItems;

    void Start()
    {
        GetComponent<LivingEntity>().OnDeath += SpawnItems;
    }

    public void SpawnItems()
    {
        int dropAmount = Random.Range(minDrops, maxDrops);
        for (int i = 0; i < dropAmount; i++)
        {
            int randomI = GetRandomItem();
            if (randomI != -1)
            {
                Transform drop = Instantiate(dropItems[randomI].dropPrefab, transform.position + transform.up, Random.rotation);
                Vector3 randDir = new Vector3(Random.Range(-2f, 2f), Random.Range(-2f, 2f), Random.Range(-2f, 2f));
                drop.GetComponent<Rigidbody>().AddForce(((Vector3.up * 8) + randDir), ForceMode.Impulse);
            }
        }
        //Destroy(gameObject);
    }

    int GetRandomItem()
    {
        int range = 0;
        for (int i = 0; i < dropItems.Length; i++)
        {
            range += dropItems[i].chance;
        }

        int rand = Random.Range(0, range);
        int top = 0;

        for (int i = 0; i < dropItems.Length; i++)
        {
            top += dropItems[i].chance;
            if (rand < top)
                return i;
        }
        return -1;
    }

}


[System.Serializable]
public class DropItem
{
    public Transform dropPrefab;
    public int chance = 5;
}