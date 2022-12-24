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
         randomItemAmmount = Random.Range(Mathf.RoundToInt(RandomItemRange.x), Mathf.RoundToInt(RandomItemRange.y));
    }
    
    // Update is called once per frame
    void Update()
    {
        if(currentAmmount < randomItemAmmount)
        {
            GameObject currentPrefab = prefabs[Random.Range(0, prefabs.Length)];

            Quaternion randomX = new Quaternion(0, Random.Range(0f, 360f), 0, 0); 
            Instantiate(currentPrefab, RandomPositionManager.instance.randomVector(), new Quaternion(0,0,0,0));

            //currentPrefab.transform.localRotation = randomX;
            currentAmmount++;
        }
      
    }


}
