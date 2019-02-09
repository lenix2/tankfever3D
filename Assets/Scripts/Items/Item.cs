using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

/**
 * Handle Items
 */
public class Item : NetworkBehaviour
{
    public GameObject Explosion; // Explosion
    public GameObject ItemShotgun; // shotgun Container
    public GameObject ItemNormal; // normal container

    public int ItemType = 0;
    // Start is called before the first frame update
    void Start()
    {
        Vector3 pos = transform.position;
        transform.position = new Vector3(pos.x, 15f, pos.z);
    }

    // Set Itemtype by Server
    public void SetItemType(int i)
    {
        if (isServer)
        {
            RpcSetItemType(i);
        }
    }
    
    /**
     * Broadcast itemtype
     */
    [ClientRpc]
    public void RpcSetItemType(int i)
    {
        ItemType = i;

        // Change Model
        if (i == 0)
        {
            ItemNormal.SetActive(true);
            ItemShotgun.SetActive(false);
        }
        else if (i == 1)
        {
            ItemNormal.SetActive(false);
            ItemShotgun.SetActive(true);
        }
    }
    
    // Collisionhandling
    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Tank"))
        {
            Explode();
			
            if (isServer)
            {
                // Give Item to player (by Server)
                SetItem(other.gameObject);
                NetworkServer.Destroy(gameObject);
            }
            else
            {
                Destroy(gameObject);
            }
        }
    }

    private void Explode()
    {
        Instantiate(Explosion, transform.position, transform.rotation);
    }
    
    private void SetItem(GameObject tank)
    {
        tank.GetComponent<TankItemManager>().AddItemEffect(ItemType);
    }
}
