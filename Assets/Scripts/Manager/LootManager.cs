using System.Collections;
using System.Collections.Generic;
using emotitron.NST;
using UnityEngine;
using UnityEngine.Networking;

public class LootManager : NetworkBehaviour
{
    public GameObject LootPrefab;

    private float _secToSpawn = 1f;
    private float _spawnChance = 0.2f;

    private float _timer;
    // Start is called before the first frame update
    void Start()
    {
        _timer = 0f;
        if (!isServer)
        {
            Destroy(this);
        }
    }

    // Update is called once per frame
    void Update()
    {
        _timer += Time.deltaTime;

        if (_timer > _secToSpawn)
        {
            RollLoot();
            _timer -= _secToSpawn;
        }
    }

    private void SpawnLoot()
    {
        GameObject loot = Instantiate(LootPrefab);
        Vector3 pos = new Vector3((Random.value * 80) - 40, 1, (Random.value * 80) - 40);
        loot.transform.position = pos; 
        NetworkServer.Spawn(loot);
    }

    private void RollLoot()
    {
        if (Random.value < _spawnChance)
        {
            SpawnLoot();
        }
    }

    public void RemoveAllLootcrates()
    {
        GameObject[] loot = GameObject.FindGameObjectsWithTag("Loot");

        foreach (GameObject go in loot)
        {
            NetworkServer.Destroy(go);
        }
    }

    public void RemoveAllItemEffects()
    {
        GameObject[] loot = GameObject.FindGameObjectsWithTag("Tank");

        foreach (GameObject go in loot)
        {
            go.GetComponent<TankItemManager>().RemoveItemEffects();
        }
    }
}
