using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StarBoxManager : MonoBehaviour
{

    public GameObject starBoxPrefab;


    private GameObject currentStarBox;


    public int stars;
    public int starsTillEnd = 6;
    public Text text;
    

    // Start is called before the first frame update
    void Start()
    {
      //  topLeft = gameObject.transform.Find("StarBoxManager/TopLeft");
        //bottomRight = gameObject.transform.Find("StarBoxManager/BottomRight");
        startStarBox();
 
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void startStarBox()
    {
        Vector3 random = RandomPositionManager.instance.randomVector();
       currentStarBox = Instantiate(starBoxPrefab, random, new Quaternion(0,0,0,0));
    }

    

    public void newStarBox()
    {
        Destroy(currentStarBox);
        AudioManager.instance.Play("Collect");
        CameraShake.Instance.Shake();
        stars = stars + 1;

        if (starsTillEnd <= stars)
        {
            print("yay");
        }
        else
        {
            startStarBox();
        }


    }

    private void endCollection()
    {
        print("collected");
    }

}
