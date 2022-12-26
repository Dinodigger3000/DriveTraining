using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LootLocker.Requests;
using TMPro;


public class LeaderBoard : MonoBehaviour
{

    string leaderboardID = "10040";

    public TextMeshProUGUI playerNames;
    public TextMeshProUGUI playerScores;
    public TextMeshProUGUI playerRank;
    public TextMeshProUGUI playerTimeText;

    public GameObject yourRank;
    public TextMeshProUGUI yourRankText;
    public TextMeshProUGUI yourScoreText;


    private string lastPlayerID;

    // Start is called before the first frame update
    void Start()
    {
        yourRank.SetActive(false);
    }

   public IEnumerator SubmitScoreRoutine(int scoreToUpload)
    {
        bool done = false;

        string playerID = PlayerPrefs.GetString("PlayerID") + GetAndIncrementScoreCharacters();
        string metaData = PlayerPrefs.GetString("name");

        lastPlayerID = playerID;

        LootLockerSDKManager.SubmitScore(playerID, scoreToUpload, leaderboardID, metaData, (response) =>
           {
               if(response.success)
               {
                   Debug.Log("Successfully uploaded score");
                   done = true;
               }
               else
               {
                   Debug.LogWarning("Failed" + response.Error);
                   done = true;
               }
           });
        yield return new WaitWhile(() => done == false);
    }

    public void submitScore()
    {
        StartCoroutine(SubmitScoreRoutine(Mathf.RoundToInt(PlayerPrefs.GetFloat("playerScore") *100)));
        StartCoroutine(fetchTopHighScoreRoutine());
    }

    public void fetch()
    {
        StartCoroutine(fetchTopHighScoreRoutine());
    }


    public IEnumerator fetchTopHighScoreRoutine()
    {
        bool done = false;
        playerNames.text = "";
        playerScores.text = "";
        playerRank.text = "";
         
        LootLockerSDKManager.GetScoreList(leaderboardID, 10, 0, (response) =>
        {

            if(response.success)
            {
                string tempPlayerNames = "Name:\n\n";
                string tempPlayerScores = "Score:\n\n";
                string tempPlayerRank = "Rank:\n\n";

                LootLockerLeaderboardMember[] members = response.items;

                for (int i = 0; i< members.Length; i++)
                {
                    tempPlayerRank += members[i].rank.ToString() + ". \n";
                    if(members[i].metadata != "")
                    {
                        tempPlayerNames += members[i].metadata;
                    }
                    else
                    {
                        tempPlayerNames += "Guest";
                    }
                    tempPlayerScores += members[i].score /100f + "\n";
                    tempPlayerNames += "\n";
                }
                done = true;
                playerNames.text = tempPlayerNames;
                playerScores.text = tempPlayerScores; 
                playerRank.text = tempPlayerRank;
                playerTimeText.text = PlayerPrefs.GetFloat("playerScore").ToString();
            }
            else
            {
                Debug.LogWarning("Failed" + response.Error);
                done = true;
            }

        });

        LootLockerSDKManager.GetMemberRank(leaderboardID, lastPlayerID, (response) =>
        {
            done = false;

            if (response.success)
            {
                Debug.Log("Successful");
                done = true;


                if(response.rank > 10)
                {
                    yourRank.SetActive(true);
                    yourRankText.text = response.rank.ToString();
                    yourScoreText.text = (response.score/100f).ToString();

                    print("rank is: " + response.rank);
                }
            }
            else
            {
                Debug.LogWarning("Error: " + response.Error);
                done = true;
            }
        });
        

        yield return new WaitWhile(() => done == false);
    }


    string GetAndIncrementScoreCharacters()
    {
        // Get the current score string
        string incrementalScoreString = PlayerPrefs.GetString(nameof(incrementalScoreString), "a");

        // Get the current character
        char incrementalCharacter = PlayerPrefs.GetString(nameof(incrementalCharacter), "a")[0];

        // If the previous character we added was 'z', add one more character to the string
        // Otherwise, replace last character of the string with the current incrementalCharacter
        if (incrementalScoreString[incrementalScoreString.Length - 1] == 'z')
        {
            // Add one more character
            incrementalScoreString += incrementalCharacter;
        }
        else
        {
            // Replace character
            incrementalScoreString = incrementalScoreString.Substring(0, incrementalScoreString.Length - 1) + incrementalCharacter.ToString();
        }

        // If the letter int is lower than 'z' add to it otherwise start from 'a' again
        if ((int)incrementalCharacter < 122)
        {
            incrementalCharacter++;
        }
        else
        {
            incrementalCharacter = 'a';
        }

        // Save the current incremental values to PlayerPrefs
        PlayerPrefs.SetString(nameof(incrementalCharacter), incrementalCharacter.ToString());
        PlayerPrefs.SetString(nameof(incrementalScoreString), incrementalScoreString.ToString());

        // Return the updated string
        return incrementalScoreString;
    }
}
