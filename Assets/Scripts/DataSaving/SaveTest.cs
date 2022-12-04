using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveTest : MonoBehaviour
{
    public PlayerData data;
    public void OnSave() {
        Debug.Log("space pressed");
        SaveManager.Save(data);
    }
    public void OnLoad() {
        Debug.Log("enter pressed");
        data = SaveManager.Load("test");
    }
    private void  update() {
    }
}
