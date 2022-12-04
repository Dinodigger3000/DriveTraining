using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.DualShock;
using TMPro;
public class PlayLocalMenu : MonoBehaviour
{
    public MenuScreen menuScreen;
    //Player data ==============
    public int p1DeviceID;
    public int p2DeviceID;
    public InputDevice p1Device;
    public InputDevice p2Device;
    public Transform player1UI;
    public Transform player2UI;
    public ProfileSelectMenu player1ProfileSelect;
    public ProfileSelectMenu player2ProfileSelect;
    public string p1ProfileName = "PLAYER 1";
    public string p2ProfileName = "PLAYER 2";
    // =========================
    private int p1InputCooldown;
    private int p2InputCooldown;
    public int playerInputListener = 0;
    public string subMenu;
    private GameDataManager gameDataManager;
    private readonly InputAction _anyKeyWait = new(binding: "/*/<button>", type:InputActionType.Button);
    private void Awake() => _anyKeyWait.performed += DoSomething;
    private void OnDestroy() => _anyKeyWait.performed -= DoSomething;
    private void OnDisable() => _anyKeyWait.Disable();
    public void OnEnable() => _anyKeyWait.Enable();
    // private void DoSomething(InputAction.CallbackContext ctx) => Debug.Log("'Any' button pressed. : " + ctx.control.device.description.interfaceName);
    private void DoSomething(InputAction.CallbackContext ctx)
    {
        // Debug.Log("'Any' button pressed. : " + ctx.control.device.description.ToString());
        // Debug.Log("'Any' button pressed. : " + ctx.control.device.GetType().ToString().Split(".").Last());
        if (ctx.control.device.description.interfaceName == "HID" || ctx.control.device.description.interfaceName == "XInput")
        {
            string deviceName = ctx.control.device.GetType().ToString().Substring(ctx.control.device.GetType().ToString().LastIndexOf(".") + 1);
            // Debug.Log(deviceName);
            int currentDeviceId = ctx.control.device.deviceId;
            // Debug.Log(currentDeviceId + "," + p1DeviceID + "," + p2DeviceID + "," + playerInputListener);
            if (playerInputListener == 1 && currentDeviceId != p2DeviceID)
            {
                p1DeviceID = currentDeviceId;
                p1Device = ctx.control.device;
                if (p1Device.GetType().ToString() == "UnityEngine.InputSystem.DualShock.DualShock4GamepadHID")
                {
                    DualShockGamepad ds4 = (DualShockGamepad)p1Device;
                    ds4.SetLightBarColor(Color.red);
                }
                player1UI.Find("ControllerConnectMenu/DisconnectButton").gameObject.SetActive(true);
                // player1UI.Find("DisconnectButton").gameObject.GetComponent<Button>().Select();
                player1UI.Find("ControllerConnectMenu/TitleListeningForInput").gameObject.SetActive(false);
                player1UI.Find("ControllerConnectMenu/TitleDevicePairedSuccessfully").gameObject.SetActive(true);
                player1UI.Find("ControllerConnectMenu/TitleDevicePairedSuccessfully/Text (TMP)").gameObject.GetComponent<TMP_Text>().text = (deviceName);
                p1InputCooldown = 10;
                playerInputListener = 0;
            }
            else if (playerInputListener == 2 && currentDeviceId != p1DeviceID)
            {
                p2DeviceID = currentDeviceId;
                p2Device = ctx.control.device;
                if (p2Device.GetType().ToString() == "UnityEngine.InputSystem.DualShock.DualShock4GamepadHID")
                {
                    DualShockGamepad ds4 = (DualShockGamepad)p2Device;
                    ds4.SetLightBarColor(Color.blue);
                }
                player2UI.Find("ControllerConnectMenu/DisconnectButton").gameObject.SetActive(true);
                // player2UI.Find("DisconnectButton").gameObject.GetComponent<Button>().Select();
                player2UI.Find("ControllerConnectMenu/TitleListeningForInput").gameObject.SetActive(false);
                player2UI.Find("ControllerConnectMenu/TitleDevicePairedSuccessfully").gameObject.SetActive(true);
                player2UI.Find("ControllerConnectMenu/TitleDevicePairedSuccessfully/Text (TMP)").gameObject.GetComponent<TMP_Text>().text = (deviceName);
                p2InputCooldown = 10;
                playerInputListener = 0;
            }
            else
            {
                if (currentDeviceId == p1DeviceID)
                {
                    p1InputCooldown = 30;
                }
                if (currentDeviceId == p2DeviceID)
                {
                    p2InputCooldown = 30;
                }
            }
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        // gameDataManager = GameObject.Find("GameDataManager").GetComponent(GameDataManager);
        ControllerSelectMenu();
    }
    public void ControllerSelectMenu() {
        transform.Find("Player1/ControllerConnectMenu").gameObject.SetActive(true);
        transform.Find("Player1/ProfileSelectMenu").gameObject.SetActive(false);
        transform.Find("Player2/ControllerConnectMenu").gameObject.SetActive(true);
        transform.Find("Player2/ProfileSelectMenu").gameObject.SetActive(false);        
        subMenu = "ControllerSelect";
    }
    public void ProfileSelectMenu() {
        transform.Find("Player1/ControllerConnectMenu").gameObject.SetActive(false);
        transform.Find("Player1/ProfileSelectMenu").gameObject.SetActive(true);
        transform.Find("Player2/ControllerConnectMenu").gameObject.SetActive(false);
        transform.Find("Player2/ProfileSelectMenu").gameObject.SetActive(true);
        subMenu = "ProfileSelect";
    }
    public void SetPlayerDevice(int playerNum) 
    {
        // Debug.Log(p1InputCooldown +","+ p2InputCooldown);
        if (playerNum == 1 && p1InputCooldown != 10) {
            playerInputListener = playerNum;
            player1UI.Find("ControllerConnectMenu/TitleListeningForInput").gameObject.SetActive(true);
            player1UI.Find("ControllerConnectMenu/TitleConnectController").gameObject.SetActive(false);
            player1UI.Find("ControllerConnectMenu/TitleDevicePairedSuccessfully").gameObject.SetActive(false);
            // Debug.Log("Listening for player "+playerNum);
        } else if (playerNum == 2 && p2InputCooldown != 10) {
            playerInputListener = playerNum;
            player2UI.Find("ControllerConnectMenu/TitleListeningForInput").gameObject.SetActive(true);
            player2UI.Find("ControllerConnectMenu/TitleConnectController").gameObject.SetActive(false);
            player2UI.Find("ControllerConnectMenu/TitleDevicePairedSuccessfully").gameObject.SetActive(false);
            // Debug.Log("Listening for player "+playerNum);
        } else {
            // Debug.Log("Not listening for player "+playerNum);
        }
        
    }
    public void DisconnectPlayerDevice(int playerNum)
    {
        // Debug.Log("Disconnected player "+playerNum + " " + playerInputListener);
        if (playerNum == 1)
        {
            p1DeviceID = 0;
            player1UI.Find("ControllerConnectMenu/TitleListeningForInput").gameObject.SetActive(false);
            player1UI.Find("ControllerConnectMenu/TitleDevicePairedSuccessfully").gameObject.SetActive(false);
            player1UI.Find("ControllerConnectMenu/TitleConnectController").gameObject.SetActive(true);
            player1UI.Find("ControllerConnectMenu/ConnectButton").GetComponent<Button>().interactable = true;
            player1UI.Find("ControllerConnectMenu/ConnectButton").GetComponent<Button>().Select();
            player1UI.Find("ControllerConnectMenu/DisconnectButton").gameObject.SetActive(false);
        }
        else if (playerNum == 2)
        {
            p2DeviceID = 0;
            player2UI.Find("ControllerConnectMenu/TitleListeningForInput").gameObject.SetActive(false);
            player2UI.Find("ControllerConnectMenu/TitleDevicePairedSuccessfully").gameObject.SetActive(false);
            player2UI.Find("ControllerConnectMenu/TitleConnectController").gameObject.SetActive(true);
            player2UI.Find("ControllerConnectMenu/ConnectButton").GetComponent<Button>().interactable = true;
            player2UI.Find("ControllerConnectMenu/ConnectButton").GetComponent<Button>().Select();
            player2UI.Find("ControllerConnectMenu/DisconnectButton").gameObject.SetActive(false);
        }
    }
    public void SwapControls()
    {
        int tempP1Id = p1DeviceID;
        p1DeviceID = p2DeviceID;
        p2DeviceID = tempP1Id;
        InputDevice tempP1Device = p1Device;
        p1Device = p2Device;
        p2Device = tempP1Device;
        player1UI.Find("ControllerConnectMenu/TitleDevicePairedSuccessfully/Text (TMP)").gameObject.GetComponent<TMP_Text>().text = (p1Device.GetType().ToString().Substring(p1Device.GetType().ToString().LastIndexOf(".") + 1));
        player2UI.Find("ControllerConnectMenu/TitleDevicePairedSuccessfully/Text (TMP)").gameObject.GetComponent<TMP_Text>().text = (p2Device.GetType().ToString().Substring(p2Device.GetType().ToString().LastIndexOf(".") + 1));
        if (p1Device.GetType().ToString() == "UnityEngine.InputSystem.DualShock.DualShock4GamepadHID")
        {
            DualShockGamepad ds4 = (DualShockGamepad)p1Device;
            ds4.SetLightBarColor(Color.red);
        }
        if (p2Device.GetType().ToString() == "UnityEngine.InputSystem.DualShock.DualShock4GamepadHID")
        {
            DualShockGamepad ds4 = (DualShockGamepad)p2Device;
            ds4.SetLightBarColor(Color.blue);
        }
    }
    // public void InitProfiles(Transform UI) {
    //     List<string> profileNames = SaveManager.GetProfiles();
    //     Transform profileList = UI.Find("ProfileSelectMenu/ProfileSelectWindow/Viewport/Content");
    //     GameObject templateProfile = profileList.Find("TemplateProfile").gameObject;
    //     foreach (string profileName in profileNames) {
    //         Transform profileObj;
    //         GameObject newProfileObj;
    //         profileObj = profileList.Find(profileName);
    //         if (profileObj) {
    //             newProfileObj = profileObj.gameObject;
    //         } else {
    //             newProfileObj = Instantiate(templateProfile, profileList);
    //             newProfileObj.name = profileName;
    //             newProfileObj.transform.Find("ProfileName").gameObject.GetComponent<TMP_Text>().text = (profileName);
    //         }
    //         newProfileObj.transform.SetAsFirstSibling();
    //         newProfileObj.SetActive(true);
    //     }
    // }

    public void SetPlayerProfile(Transform button) {
        string profileName = button.name;
        if (profileName != p1ProfileName && profileName != p2ProfileName) {
            if (button.IsChildOf(player1UI)) {
                player1ProfileSelect.SetPlayerProfile(profileName);
                p1ProfileName = profileName;
            }
            if (button.IsChildOf(player2UI)) {
                player2ProfileSelect.SetPlayerProfile(profileName);
                p2ProfileName = profileName;
            }
        }
    }
    // public void CreateNewProfile(Transform list) {
    //     list.Find("NameNewProfile").gameObject.SetActive(true);
    //     list.Find("CreateNewProfileButton").gameObject.SetActive(false);
    //     list.Find("NameNewProfile").gameObject.GetComponent<TMP_InputField>().Select();
    //     list.Find("NameNewProfile").SetAsFirstSibling();
    // // }
    // public void NameNewProfile(Transform list) {
    //     List<string> profileNames = SaveManager.GetProfiles();
    //     string newName = list.Find("NameNewProfile").gameObject.GetComponent<TMP_InputField>().text;
    //     list.Find("NameNewProfile").gameObject.SetActive(false);
    //     list.Find("CreateNewProfileButton").gameObject.SetActive(true);
    //     list.Find("CreateNewProfileButton").gameObject.GetComponent<Button>().Select();
    //     list.Find("NameNewProfile").gameObject.GetComponent<TMP_InputField>().text = "";
    //     if (!profileNames.Contains(newName)) {
    //         SaveManager.NewProfile(newName);
    //     }
    //     InitProfiles(player1UI);
    //     InitProfiles(player2UI);
    // }
    private void SetData() {
        GameDataManager.Instance.player1ControllerId = p1DeviceID;
        GameDataManager.Instance.player1Controller = p1Device;
        GameDataManager.Instance.Player1ProfileName = p1ProfileName;
    
        GameDataManager.Instance.player2ControllerId = p2DeviceID;
        GameDataManager.Instance.player2Controller = p2Device;
        GameDataManager.Instance.Player2ProfileName = p2ProfileName;
    }
    // Update is called once per frame
    void Update()
    {
        if (subMenu == "ControllerSelect") {
            if (p1DeviceID != 0 && p2DeviceID != 0) {
                transform.Find("SwapControlsButton").gameObject.SetActive(true);
                transform.Find("ContinueButton").gameObject.SetActive(true);
            } else {
                transform.Find("SwapControlsButton").gameObject.SetActive(false);
                transform.Find("ContinueButton").gameObject.SetActive(false);
            }
        }else if (subMenu == "ProfileSelect") {
            if (p1ProfileName != "PLAYER 1" && p2ProfileName != "PLAYER 2") {
                transform.Find("SwapControlsButton").gameObject.SetActive(true);
                transform.Find("ContinueButton").gameObject.SetActive(true);
            } else {
                transform.Find("SwapControlsButton").gameObject.SetActive(false);
                transform.Find("ContinueButton").gameObject.SetActive(false);
            }
        }   
        if (p1InputCooldown > 0 && p1DeviceID != 0) {
            if (p1InputCooldown == 30) {
                player1UI.Find("TitlePlayer/TextHighlight (TMP)").gameObject.SetActive(true);
            }
            p1InputCooldown -= 1;
        } else {
            player1UI.Find("TitlePlayer/TextHighlight (TMP)").gameObject.SetActive(false);
        }
        if (p2InputCooldown > 0 && p2DeviceID != 0) {
            if (p2InputCooldown == 30) {
            player2UI.Find("TitlePlayer/TextHighlight (TMP)").gameObject.SetActive(true);
            }
            p2InputCooldown -= 1;
        } else {
            player2UI.Find("TitlePlayer/TextHighlight (TMP)").gameObject.SetActive(false);
        }
    }
    
    public void Continue() {
        if (subMenu == "ControllerSelect") {
            ProfileSelectMenu();
            player1ProfileSelect = player1UI.Find("ProfileSelectMenu").gameObject.GetComponent<ProfileSelectMenu>();
            player2ProfileSelect = player2UI.Find("ProfileSelectMenu").gameObject.GetComponent<ProfileSelectMenu>();
            player1ProfileSelect.InitProfiles();
            player2ProfileSelect.InitProfiles();
            // transform.Find("SwapControlsButton").gameObject.SetActive(false);
            transform.Find("BackButton").gameObject.GetComponent<Button>().Select();
        } else if (subMenu == "ProfileSelect") {
            SetData();
            GameDataManager.Instance.gameType = "Local";
            menuScreen.PlayGame();
        }
    }
    public void Back() {
        if (subMenu == "ControllerSelect") {
            menuScreen.OpenMainMenu("PlayModeSelect");
        }
        else if (subMenu == "ProfileSelect") {
            ControllerSelectMenu();
        }
    }
    public void Play() {
        GameDataManager.Instance.gameType = "Local";
        Debug.Log(GameDataManager.Instance.gameType);
        GameDataManager.Instance.player1ControllerId = p1DeviceID;
        GameDataManager.Instance.player2ControllerId = p2DeviceID;
    }
}

