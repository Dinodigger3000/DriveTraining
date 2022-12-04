using System.Collections;
using UnityEngine;
public class GameManager : MonoBehaviour
{
    string gameType;
    public GameObject LocalMultiplayerPrefab;
    public GameObject SingleplayerPrefab;
    void Awake()
    {
        gameType = GameDataManager.Instance.gameType;

        if (gameType == "Local") {
            Debug.Log("Local");
            Instantiate(LocalMultiplayerPrefab, new Vector3(0, 0, 0), Quaternion.identity);
        }else if (gameType == "Solo") {
            Debug.Log("Solo");
            Instantiate(SingleplayerPrefab, new Vector3(0, 0, 0), Quaternion.identity);
        }
        else {
            Debug.Log("NoGametype");
        }
    }
    void Start() {}
    void Update() {}
}
