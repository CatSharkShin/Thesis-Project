using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Management;
using UnityEngine.XR;
using Unity.XR.Oculus.Input;

public class VRSwitcher : MonoBehaviour
{
    public static VRSwitcher Instance { get; private set; }
    public GameObject VR;
    public GameObject PC;
    public Camera PCCamera;
    private bool isVRMode;
    public bool IsVRMode
    {   
        get
        {
            return isVRMode;
        }
        set
        {
            isVRMode = value;
            if (Application.isPlaying)
            {
                StartCoroutine(StartXRCoroutine(value));
            }
        }
    }
    public VRSwitcher()
    {
        Instance = this;
    }
    string enableVRArg = "--enable-vr";
    public void Awake()
    {
        StartCoroutine(StartXRCoroutine(GetArg(enableVRArg)));
    }
    // This function checks out startup arguments to see if we want VR
    // To do this, create a desktop shortcut and add the arg at the end.
    // Example: "C:\Path\To\Game.exe" --enable-vr
    private static bool GetArg(string name)
    {
        return Instance.IsVRMode;
        var args = System.Environment.GetCommandLineArgs();
        for (int i = 0; i < args.Length; i++)
        {
            Debug.Log($"Arg {i}: {args[i]}");
            if (args[i] == name)
            {
                return true;
            }
        }
        return false;
    }
    public IEnumerator StartXRCoroutine(bool enableVR)
    {

        // Only run the code block when we want VR
        Debug.Log("Looking if VR should enable");
        if (enableVR)
        {
            Debug.Log("Initializing XR...");
            yield return XRGeneralSettings.Instance.Manager.InitializeLoader();

            if (XRGeneralSettings.Instance.Manager.activeLoader == null)
            {
                Debug.LogError("Initializing XR Failed. Check Editor or Player log for details.");
            }
            else
            {
                XRGeneralSettings.Instance.Manager.StartSubsystems();
                Debug.Log("Starting XR...");
                InitVR();
            }
        }
        else
        {
            XRGeneralSettings.Instance.Manager.StopSubsystems();
            XRGeneralSettings.Instance.Manager.DeinitializeLoader();
            InitPC();
            Debug.Log("Did not find VR arg, starting in 2D");
        }
    }
    // These should be exactly eachothers opposite. So that a user could jump in-out of VR(for the far future).
    // As for now this is just a dev tool
    public void InitVR()
    {
        Instance.VR.SetActive(true);
        Instance.PC.SetActive(false);
        /*
        XRSettings.enabled = true;*/
    }
    public void InitPC()
    {
        Instance.VR.SetActive(false);
        Instance.PC.SetActive(true);
        /*
        XRDevice.DisableAutoXRCameraTracking(Instance.PCCamera,true);
        XRDevice.fovZoomFactor = 0.1f;
        XRSettings.enabled = false;*/
    }
}
