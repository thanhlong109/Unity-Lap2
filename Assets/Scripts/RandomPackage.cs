using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class RandomPackage : MonoBehaviour
{

    [SerializeField] private Packages[] packages;
    [SerializeField] private int numPackageDisplayInATime = 3;
    
    private void Awake()
    {
        packages = FindObjectsOfType<Packages>(true);
        
    }

    void Start()
    {
        
        RandomP(numPackageDisplayInATime);
    }

    public void RandomP(int countActivePara = 1)
    {
        int count = 0;
        int countActive = 0;
        while (count<1000 && countActive< countActivePara)
        {
            int p = Random.Range(0, packages.Length);
            var package = packages[p].GameObject();
            if (!package.active)
            {
                package.SetActive(true);
                countActive++;
            } 
            count++;
        }
    }

    
   

    
}
