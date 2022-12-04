using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDetails : MonoBehaviour
{
    public int playerID;
    public Transform spawn;
    void Start() {
        transform.SetPositionAndRotation(spawn.position, spawn.rotation);
    }
}
