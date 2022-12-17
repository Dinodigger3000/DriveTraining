using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObsticleManager : MonoBehaviour
{
    public GameObject[] prefabs;
    public Vector2 RandomItemRange;


    int randomItemAmmount;
    int currentAmmount;

    private StarBoxManager starBoxManager;

    // Start is called before the first frame update
    void Start()
    {
        starBoxManager = FindObjectOfType<StarBoxManager>();
         randomItemAmmount = Random.Range(Mathf.RoundToInt(RandomItemRange.x), Mathf.RoundToInt(RandomItemRange.y));
    }
    
    // Update is called once per frame
    void Update()
    {
        if(currentAmmount < randomItemAmmount)
        {
            var currentPrefab = prefabs[Random.Range(0, prefabs.Length)];

            float randomX = Random.Range(0, 360);
            Instantiate(currentPrefab, starBoxManager.randomVector(), new Quaternion(0,randomX , 0, 0));

          
            currentAmmount++;
        }
      
    }


}
