using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class Item : NetworkBehaviour
{
    public GameObject Explosion;

    public int ItemType;
    // Start is called before the first frame update
    void Start()
    {
        Vector3 pos = transform.position;
        transform.position = new Vector3(pos.x, 15f, pos.z); 
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Tank"))
        {
            Explode();
			
            if (isServer)
            {
                NetworkServer.Destroy(gameObject);
                SetItem(other.gameObject);
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
