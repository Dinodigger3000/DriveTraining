using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LootLocker.Requests;
using TMPro;

public class scoreManager : MonoBehaviour
{
    public LeaderBoard leaderboard;
    public TMP_InputField playerNameInputField;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(SetupRoutine());
    }

    public void refresh()
    {
        StartCoroutine(SetupRoutine());
    }

    IEnumerator SetupRoutine()
    {
        yield return LoginRoutine();
        yield return leaderboard.fetchTopHighScoreRoutine();
    }

    public void setupPlayerName()
    {

        PlayerPrefs.SetString("name", playerNameInputField.text);

       // LootLockerSDKManager.SetPlayerName(playerNameInputField.text, (response) =>
      //  {
       //     if (response.success)
       //     {
       //         Debug.Log("Succesfully set player name");
      //      }
      //      else
      //      {
       //         Debug.LogWarning("Could not set player name" + response.Error);
      //      }
      //  });
    }

    private IEnumerator LoginRoutine()
    {
        bool done = false;

        LootLockerSDKManager.StartGuestSession((response) =>
        {

            if (response.success)
            {
                Debug.Log("player logged in");
                PlayerPrefs.SetString("PlayerID", response.player_id.ToString());
                done = true;
            }
            else
            {
                done = true;
                Debug.LogWarning("could not start session");
            }
        });
        yield return new WaitWhile(()=> done == false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
