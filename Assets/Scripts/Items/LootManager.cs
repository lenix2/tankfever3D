using System.Collections;
using System.Collections.Generic;
using emotitron.NST;
using UnityEngine;
using UnityEngine.Networking;

/**
 * Manage Itemspawn
 */
public class LootManager : NetworkBehaviour
{
    public GameObject LootPrefab;
    public float[] TypeChances; // Array of types and chances
    
    private float _secToSpawn = 1f; // Spawninterval
    private float _spawnChance = 0.35f; // Spawnchance

    private float _timer;
    
    // Start is called before the first frame update
    void Start()
    {
        _timer = 0f;
        if (!isServer)
        {
            // Only ceep on server
            Destroy(this);
        }
    }

    // Update is called once per frame
    void Update()
    {
        _timer += Time.deltaTime;

        if (_timer > _secToSpawn)
        {
            // check for Lootspawn
            RollLoot();
            _timer -= _secToSpawn;
        }
    }

    // Do Lootspawn
    private void SpawnLoot(int id)
    {
        GameObject loot = Instantiate(LootPrefab);
        Vector3 pos = new Vector3((Random.value * 80) - 40, 15f, (Random.value * 80) - 40);
        loot.transform.position = pos; 
        
        // Network init
        NetworkServer.Spawn(loot);
        
        // Change type
        loot.GetComponent<Item>().SetItemType(id);
    }

    // Check if loot should spawn
    private void RollLoot()
    {
        if (Random.value < _spawnChance)
        {
            SpawnLoot(CalculateItemType());
        }
    }

    // Calculate itemtype
    private int CalculateItemType()
    {
        float maxProp = 0f;

        // get total propability-count
        foreach (float f in TypeChances)
        {
            maxProp += f;
        }

        // find type
        float rdm = Random.value * maxProp;
        float prop = 0f;
        for (int i = 0; i < TypeChances.Length; i++)
        {
            prop += TypeChances[i];
            if (rdm <= prop)
            {
                return i;
            }
        }

        return 0;
    }

    /*
     * Delete all Lootcrates
     */
    public void RemoveAllLootcrates()
    {
        GameObject[] loot = GameObject.FindGameObjectsWithTag("Loot");

        foreach (GameObject go in loot)
        {
            NetworkServer.Destroy(go);
        }
    }

    /*
     * Remove all item Effects from tanks 
     */
    public void RemoveAllItemEffects()
    {
        GameObject[] loot = GameObject.FindGameObjectsWithTag("Tank");

        foreach (GameObject go in loot)
        {
            go.GetComponent<TankItemManager>().RemoveItemEffects();
        }
    }
}
