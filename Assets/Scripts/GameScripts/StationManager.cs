using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

[Serializable] public class Station {
    public GameObject station;
    public string name;
    public Transform cameraPos;
    public Transform spawn;
    public bool enabled;
    public bool IsActive() {
        return enabled;
    }
    public void Activate() {
        station.GetComponent<Camera>().enabled = true;
        enabled = true;
    }
    public void Deactivate() {
        station.GetComponent<Camera>().enabled = false;
        enabled = false;
    }
    public void Setup() {
        cameraPos = station.transform;
        spawn = station.transform.Find("Spawn");
        name = station.name;
    }
    public void Setup(GameObject stationObj) {
        station = stationObj;
        cameraPos = station.transform;
        spawn = station.transform.Find("Spawn");
        name = stationObj.name;
    }
    public void SetSpawn(Transform newSpawn) {
        spawn = newSpawn;
    }
    public Camera GetCamera() {
        return station.GetComponent<Camera>();
    }
}
public class StationManager : MonoBehaviour
{
    public List<Station> stations = new List<Station>();

    public Station GetNextOpenStation() {
        for (int i = 0; i < stations.Count; i++) {
            if (!(stations[i].IsActive())) {
                return stations[i];
            }
        }
        return null;
    }
    public Station FindStation(string name) {
        for (int i = 0; i < stations.Count; i++) {
            if (stations[i].name == name) {
                return stations[i];
            }
        }
        return null;
    }
    void Start() {
        for (int i = 0; i < stations.Count; i++) {
            stations[i].Setup();
        }
    }
    void Update() {}
}
