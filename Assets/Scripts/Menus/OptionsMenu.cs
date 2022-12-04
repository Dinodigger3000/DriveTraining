using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.DualShock;
using TMPro;
public class OptionsMenu : MonoBehaviour
{
    public MenuScreen menuScreen;

    void Start() {}
    void Update() {}
    public void Back() {
        menuScreen.OpenMainMenu("Options");
    }
}
