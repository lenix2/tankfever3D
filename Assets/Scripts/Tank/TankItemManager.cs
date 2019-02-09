using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using UnityEngine;
using UnityEngine.Networking;

/**
 * manage tank items
 */
public class TankItemManager : NetworkBehaviour
{
    
    public GameObject tankShotgun;
    public GameObject tankNormal;
        
    [SyncVar]
    [ReadOnly] public bool effectShotgun = false;
    
    // remove all effects from tank
    public void RemoveItemEffects()
    {
        effectShotgun = false;
        tankNormal.SetActive(true);
        tankShotgun.SetActive(false);
        RpcRemoveItemEffects();
    }
    
    // remove all effect from tank on the clientside
    [ClientRpc]
    public void RpcRemoveItemEffects()
    {
        effectShotgun = false;
        tankNormal.SetActive(true);
        tankShotgun.SetActive(false);
    }

    // Add new Item-effect
    public void AddItemEffect(int id)
    {
        RpcAddItemEffect(id);
    }
    
    // add item-effect to clients
    [ClientRpc]
    public void RpcAddItemEffect(int id)
    {
        gameObject.GetComponent<TankShoot>().AddAmmo(1f);
        
        if (id == 0)
        {
            gameObject.GetComponent<TankShoot>().AddAmmo(2f);
        } else if (id == 1 && !effectShotgun) {
            effectShotgun = true;
            tankNormal.SetActive(false);
            tankShotgun.SetActive(true);
        }
    }
}
