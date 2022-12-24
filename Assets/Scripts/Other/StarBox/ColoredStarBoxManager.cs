using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ColoredStarBoxManager : MonoBehaviour
{
    public GameObject starBoxPrefab;
    public Transform topLeft;
    public Transform bottomRight;
    public float timer;

    private bool timerOn;

    public Material[] starColors;

    private GameObject currentStarBox;
    private int currentMaterial;


    public int stars;
    public int starsTillEnd = 6;
    public Text timerText;



    void Start()
    {
        //  topLeft = gameObject.transform.Find("StarBoxManager/TopLeft");
        //bottomRight = gameObject.transform.Find("StarBoxManager/BottomRight");
        startStarBox();
        timerOn = true;

    }

    // Update is called once per frame
    void Update()
    {
        if (timerOn)
        {
            timer += Time.deltaTime;
            timerText.text = (Mathf.Round(timer * 100f) / 100f).ToString();
                
              
        }
    }

    private void startStarBox()
    {
        Vector3 random = RandomPositionManager.instance.randomVector();
        currentStarBox = Instantiate(starBoxPrefab, random, new Quaternion(0, 0, 0, 0));

        currentMaterial = Random.Range(0, starColors.Length);

        currentStarBox.GetComponentInChildren<MeshRenderer>().material = starColors[currentMaterial];

        currentStarBox.GetComponentInChildren<Light>().color = starColors[currentMaterial].color;

        currentStarBox.tag = (starColors[currentMaterial].name);
         
    }


    public void newStarBox(string tag)
    {
        if (tag == currentStarBox.tag) {
            stars = stars + 2;
        }
        else
        {
            stars = stars + 1;
        }

            Destroy(currentStarBox);
        AudioManager.instance.Play("Collect");
        CameraShake.Instance.Shake();
        
        

       

        if (starsTillEnd <= stars)
        {
            timerOn = false;
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
