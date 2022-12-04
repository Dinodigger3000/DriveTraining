using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.DualShock;
using TMPro;
public class ProfileSelectMenu : MonoBehaviour
{
    public string selectedProfileName = "Player";
    [Tooltip("The parent of this menu.")]
    public Transform UI;
    [Tooltip("The content collection that has the profile buttons.")]
    public Transform content;
    public void InitProfiles() {
        List<string> profileNames = SaveManager.GetProfiles();
        GameObject templateProfile = content.Find("TemplateProfile").gameObject;
        foreach (string profileName in profileNames) {
            Transform profileObj;
            GameObject newProfileObj;
            profileObj = content.Find(profileName);
            if (profileObj) {
                newProfileObj = profileObj.gameObject;
            } else {
                newProfileObj = Instantiate(templateProfile, content);
                newProfileObj.name = profileName;
            }
            newProfileObj.transform.Find("ProfileName").gameObject.GetComponent<TMP_Text>().text = (profileName);
            newProfileObj.transform.Find("ProfileNameSelected").gameObject.GetComponent<TMP_Text>().text = (profileName);
            if (profileName == selectedProfileName) {
                newProfileObj.transform.Find("ProfileName").gameObject.SetActive(false);
                newProfileObj.transform.Find("ProfileNameSelected").gameObject.SetActive(true);
            }
            else {
                newProfileObj.transform.Find("ProfileName").gameObject.SetActive(true);
                newProfileObj.transform.Find("ProfileNameSelected").gameObject.SetActive(false);
            }
            newProfileObj.transform.SetAsFirstSibling();
            newProfileObj.SetActive(true);
        }
        
    }
    public void SetPlayerProfile(string profileName) {
        Transform oldProfile = content.Find(selectedProfileName);
        Transform newProfile = content.Find(profileName);
        if (oldProfile) {
            oldProfile.Find("ProfileName").gameObject.SetActive(true);
            oldProfile.Find("ProfileNameSelected").gameObject.SetActive(false);
        }
        newProfile.Find("ProfileName").gameObject.SetActive(false);
        newProfile.Find("ProfileNameSelected").gameObject.SetActive(true);
        selectedProfileName = profileName;
        foreach (Transform child in UI.Find("TitlePlayer")) {
            child.gameObject.GetComponent<TMP_Text>().text = selectedProfileName;
        }
    }
    public void CreateNewProfile() {
        content.Find("NameNewProfile").gameObject.SetActive(true);
        content.Find("CreateNewProfileButton").gameObject.SetActive(false);
        content.Find("NameNewProfile").gameObject.GetComponent<TMP_InputField>().Select();
        content.Find("NameNewProfile").SetAsFirstSibling();
    }
    public void NameNewProfile() {
        List<string> profileNames = SaveManager.GetProfiles();
        string newName = content.Find("NameNewProfile").gameObject.GetComponent<TMP_InputField>().text;
        content.Find("NameNewProfile").gameObject.SetActive(false);
        content.Find("CreateNewProfileButton").gameObject.SetActive(true);
        content.Find("CreateNewProfileButton").gameObject.GetComponent<Button>().Select();
        content.Find("NameNewProfile").gameObject.GetComponent<TMP_InputField>().text = "";
        if (!profileNames.Contains(newName)) {
            SaveManager.NewProfile(newName);
        }
        InitProfiles();
    }
    void Start() {
        if (content == null) {
            content = transform.Find("ProfileSelectWindow/Viewport/Content");
        }
        InitProfiles();
    }
    public void OnEnable() {
        InitProfiles();
    }
    void Update() {}
}
