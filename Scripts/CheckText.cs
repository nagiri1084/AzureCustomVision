using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckText : MonoBehaviour
{
    public static CheckText Instance;

    /// <summary>
    /// The label used to display the analysis on the objects in the real world
    /// </summary>
    public GameObject label;
    /// <summary>
    /// The label used to display the analysis on the objects in the real world
    /// </summary>
    float k = 0;
    private GameObject DebugPosition;

    /// <summary>
    /// Initializes this class
    /// </summary>
    private void Awake()
    {
        // Allows this instance to behave like a singleton
        Instance = this;
        DebugPosition = this.gameObject;
    }
    public void SetStatus(string statusText)
    {
        if (statusText!=null)
        {
            GameObject display = this.gameObject;
            display.transform.localPosition = new Vector3(0, 0, 1);
            display.SetActive(true);
            display.transform.localScale = new Vector3(0.03f,0.03f,1.0f);
            display.transform.rotation = new Quaternion();
            TextMesh textMesh = display.GetComponent<TextMesh>();
            textMesh.GetComponent<TextMesh>().text = statusText;
        Debug.Log(statusText);
        }
    }
    public void CreateDebug(string debugLog)
    {
        if (debugLog != null)
        {
            GameObject display = Instantiate(label, DebugPosition.transform.position, DebugPosition.transform.rotation);
            display.transform.localPosition = new Vector3(0, 0, 1);
            display.SetActive(true);
            display.transform.localScale = new Vector3(0.03f, 0.03f, k); k--;
            display.transform.rotation = new Quaternion();
            TextMesh textMesh = display.GetComponent<TextMesh>();
            textMesh.GetComponent<TextMesh>().text = debugLog;
            Debug.Log(debugLog);
        }
    }
}
