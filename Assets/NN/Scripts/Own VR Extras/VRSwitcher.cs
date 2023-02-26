using System.Collections;
using UnityEngine;

using UnityEngine.XR.Management;

public class VRSwitcher : MonoBehaviour
{
    public static VRSwitcher Instance { get; private set; }
    public GameObject VR;
    public GameObject PC;
    public Camera PCCamera;
    public bool isVRMode;
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
    public void Start()
    {
        StartCoroutine(StartXRCoroutine(isVRMode));
    }
    IEnumerator InitXR1()
    {
        Debug.Log("Initializing XR...");
        if (XRGeneralSettings.Instance.Manager.activeLoader != null)
        {
            Debug.Log("Deinitializing loader...");
            XRGeneralSettings.Instance.Manager.DeinitializeLoader();
        }
        Debug.Log("Initializing loader...");
        yield return XRGeneralSettings.Instance.Manager.InitializeLoader();
        XRGeneralSettings.Instance.Manager.StartSubsystems();
        InitVR();
    }
    IEnumerator InitXR2()
    {
        yield return XRGeneralSettings.Instance.Manager.InitializeLoader();
        XRGeneralSettings.Instance.Manager.StartSubsystems();
        InitVR();
    }

    // This function checks out startup arguments to see if we want VR
    // To do this, create a desktop shortcut and add the arg at the end.
    // Example: "C:\Path\To\Game.exe" --enable-vr
    private static bool GetArg(string name)
    {
        return Instance.isVRMode;
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
            yield return null;
            //XRGeneralSettings.Instance.Manager.StartSubsystems();

            if (XRGeneralSettings.Instance.Manager.activeLoader != null)
            {
                XRGeneralSettings.Instance.Manager.StopSubsystems();
                XRGeneralSettings.Instance.Manager.DeinitializeLoader();
            }
            yield return XRGeneralSettings.Instance.Manager.InitializeLoader();
            
            if (XRGeneralSettings.Instance.Manager.activeLoader == null)
            {
                Debug.LogError("Initializing XR Failed. Check Editor or Player log for details.");
            }
            else
            {
                XRGeneralSettings.Instance.Manager.StartSubsystems();
                InitVR();
                Debug.Log("Starting XR...");
            }
        }
        else
        {
            InitPC();
            XRGeneralSettings.Instance.Manager.StopSubsystems();
            XRGeneralSettings.Instance.Manager.DeinitializeLoader();
            Debug.Log("Did not find VR arg, starting in 2D");
        }
    }
    // These should be exactly eachothers opposite. So that a user could jump in-out of VR(for the far future).
    // As for now this is just a dev tool
    public void InitVR()
    {
        Instance.VR.SetActive(true);
        Instance.PC.SetActive(false);
        Camera.SetupCurrent(Instance.VR.GetComponentInChildren<Camera>());
        /*
        XRSettings.enabled = true;*/
    }
    public void InitPC()
    {
        Instance.VR.SetActive(false);
        Instance.PC.SetActive(true);
        Camera.SetupCurrent(Instance.PC.GetComponentInChildren<Camera>());
        /*
        XRDevice.DisableAutoXRCameraTracking(Instance.PCCamera,true);
        XRDevice.fovZoomFactor = 0.1f;
        XRSettings.enabled = false;*/
    }
}
