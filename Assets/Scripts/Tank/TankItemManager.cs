using System.Collections;
using System.Collections.Generic;
using NUnit.Framework.Constraints;
using Unity.Collections;
using UnityEngine;

public class TankItemManager : MonoBehaviour
{
    
    public GameObject tankShotgun;
    public GameObject tankNormal;
        
    
    [ReadOnly] public bool effectShotgun = false;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void RemoveItemEffects()
    {
        effectShotgun = false;
        tankNormal.SetActive(true);
        tankShotgun.SetActive(false);
    }

    public void AddItemEffect(int id)
    {
        if (id == 1 && !effectShotgun)
        {
            effectShotgun = true;
            tankNormal.SetActive(false);
            tankShotgun.SetActive(true);
        }
    }
}
