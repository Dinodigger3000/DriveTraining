using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;


public static class SaveManager
{
    public static string directory = "/Profiles/";

    public static void Save(PlayerData data) {
        string dir = Application.persistentDataPath + directory;
        string fileName = (data.playerName + ".txt");
        if(!Directory.Exists(dir)) 
            Directory.CreateDirectory(dir);
        string json = JsonUtility.ToJson(data);
        File.WriteAllText(dir + fileName, json);
        Debug.Log("json saved at: " + dir + fileName);
    }
    public static PlayerData Load(string playerName) {
        string fullPath = Application.persistentDataPath + directory + playerName + ".txt";
        PlayerData data = new PlayerData();

        if (File.Exists(fullPath)) {
            string json = File.ReadAllText(fullPath);
            data = JsonUtility.FromJson<PlayerData>(json);
            Debug.Log("data loaded from: "+fullPath);
        }
        else {
            Debug.Log("Save file does not exist");
        }

        return data;
    }
    public static List<string> GetProfiles() {
        string path = Application.persistentDataPath + directory;
        DirectoryInfo dir = new DirectoryInfo(path);
        FileInfo[] info = dir.GetFiles();
        // int fileCount = info.Length;
        List<string> profileNames = new List<string>();
        foreach (FileInfo infoTemp in info)
            profileNames.Add(infoTemp.Name.Replace(".txt", ""));
        return profileNames;
    }
    public static void NewProfile(string profileName) {
        PlayerData newProfileData = new PlayerData();
        newProfileData.playerName = profileName;
        Save(newProfileData);
    }
}
