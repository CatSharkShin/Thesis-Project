using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class NetworkBlueprint : MonoBehaviour
{
    public NeuralNetworkGenerator neuralNetworkGenerator;
    public Button delete;
    public TextMeshProUGUI NetworkID;
    public Slider NodeCount;
    public Slider RelationCount;
    public Toggle Spherical;
    public TMP_InputField radius;

    public TMP_InputField minX;
    public TMP_InputField minY;
    public TMP_InputField minZ;

    public TMP_InputField maxX;
    public TMP_InputField maxY;
    public TMP_InputField maxZ;

    public TMP_InputField offsetX;
    public TMP_InputField offsetY;
    public TMP_InputField offsetZ;
    public void AddListener(UnityAction a)
    {
        NodeCount.onValueChanged.AddListener((float f) => a.Invoke());
        RelationCount.onValueChanged.AddListener((float f) => a.Invoke());
        Spherical.onValueChanged.AddListener((bool b) => a.Invoke());
        radius.onValueChanged.AddListener((string s) => a.Invoke());
        minX.onValueChanged.AddListener((string s) => a.Invoke());
        minY.onValueChanged.AddListener((string s) => a.Invoke());
        minZ.onValueChanged.AddListener((string s) => a.Invoke());
        maxX.onValueChanged.AddListener((string s) => a.Invoke());
        maxY.onValueChanged.AddListener((string s) => a.Invoke());
        maxZ.onValueChanged.AddListener((string s) => a.Invoke());
        offsetX.onValueChanged.AddListener((string s) => a.Invoke());
        offsetY.onValueChanged.AddListener((string s) => a.Invoke());
        offsetZ.onValueChanged.AddListener((string s) => a.Invoke());
    }
    public bool Validate()
    {
        if(!int.TryParse(NetworkID.text, out _))
            return false;
        if (!int.TryParse(radius.text, out _))
            return false;

        if (!int.TryParse(minX.text, out _))
            return false;
        if (!int.TryParse(minY.text, out _))
            return false;
        if (!int.TryParse(minZ.text, out _))
            return false;
        if (!int.TryParse(maxX.text, out _))
            return false;
        if (!int.TryParse(maxY.text, out _))
            return false;
        if (!int.TryParse(maxZ.text, out _))
            return false;
        if (!int.TryParse(offsetX.text, out _))
            return false;
        if (!int.TryParse(offsetY.text, out _))
            return false;
        if (!int.TryParse(offsetZ.text, out _))
            return false;

        return true;
    }
    private void Start()
    {
        delete.onClick.AddListener(() => Delete());
    }
    private void Delete()
    {
        if(neuralNetworkGenerator != null)
        {
            neuralNetworkGenerator.networkBlueprints.Remove(this);
        }
        neuralNetworkGenerator.Generate();
        Destroy(gameObject);
    }
}
