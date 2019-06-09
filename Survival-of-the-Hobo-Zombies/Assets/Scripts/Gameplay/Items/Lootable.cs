using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lootable : Interactable {

    public int minDrops = 0;
    public int maxDrops = 4;
    public LootableItem[] lootableItems;
    public Transform modelNotLooted;
    public Transform modelLooted;

    void Start()
    {
        if (modelLooted != null) modelLooted.gameObject.SetActive(false);
    }

    public override void Interact()
    {
        Interact(transform.position + transform.up);
    }

    public override void Interact(Player player)
    {
        Interact(player.transform.position);
    }

    IEnumerator SpawnItems(Vector3 position)
    {
        if (modelLooted != null)
        {
            modelLooted.gameObject.SetActive(true);
            modelNotLooted.gameObject.SetActive(false);
        }

        int dropAmount = Random.Range(minDrops, maxDrops);
        for (int i = 0; i < dropAmount; i++)
        {
            int randomI = GetRandomItem();
            if (randomI != -1)
            {
                Transform drop = Instantiate(lootableItems[randomI].dropPrefab, transform.position + transform.up, Random.rotation);
                Vector3 randDir = new Vector3(Random.Range(-0.5f, 0.5f), Random.Range(-0.5f, 0.5f), Random.Range(-0.5f, 0.5f));
                drop.GetComponent<Rigidbody>().AddForce(((position - drop.transform.position) + (Vector3.up * 4) + randDir), ForceMode.Impulse);
                yield return new WaitForSeconds(0.2f);
            }
        }  
    }

    public override void Interact(Vector3 position)
    {
        if (isInteractable)
        {
            isInteractable = false;
            StartCoroutine(SpawnItems(position));
        }
    }

    int GetRandomItem()
    {
        int range = 0;
        for (int i = 0; i < lootableItems.Length; i++)
        {
            range += lootableItems[i].chance;
        }

        int rand = Random.Range(0, range);
        int top = 0;

        for (int i = 0; i < lootableItems.Length; i++)
        {
            top += lootableItems[i].chance;
            if (rand < top)
                return i;
        }
        return -1;
    }
	
}

[System.Serializable]
public class LootableItem {
    public Transform dropPrefab;
    public int chance = 5;
}
