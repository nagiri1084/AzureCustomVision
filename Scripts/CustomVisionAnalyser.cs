using System.Collections.Generic;
using Newtonsoft.Json;
using System.Collections;
using System.IO;
using System.Linq;
using UnityEngine;
using UnityEngine.Networking;

public class CustomVisionAnalyser : MonoBehaviour
{

    /// <summary>
    /// Split JsonFile
    /// </summary>
    char separatorChar = '"';
    public string[] tagName = new string[] { "chair", "swivelchair", "laptop", "table" };
    //public string[] tagName = { "chair", "swivelchair", "laptop", "table" };

    /// <summary>
    /// Unique instance of this class
    /// </summary>
    public static CustomVisionAnalyser Instance;

    /// <summary>
    /// Insert your prediction key here
    /// </summary>
    private string predictionKey = "616811e1fabf47c6b09180dd83095164";

    /// <summary>
    /// Insert your prediction endpoint here
    /// </summary>
    private string predictionEndpoint = "https://azurecustomvisionproject-prediction.cognitiveservices.azure.com/customvision/v3.0/Prediction/c2ec8fb0-d34a-4abb-975f-b8e3ca5bee35/detect/iterations/Iteration1/image";

    /// <summary>
    /// Bite array of the image to submit for analysis
    /// </summary>
    [HideInInspector] public byte[] imageBytes;

    /// <summary>
    /// Initializes this class
    /// </summary>
    private void Awake()
    {
        // Allows this instance to behave like a singleton
        Instance = this;
    }

    /// <summary>
    /// Call the Computer Vision Service to submit the image.
    /// </summary>
    public IEnumerator AnalyseLastImageCaptured(string imagePath)
    {
        Debug.Log("Analyzing...");
        CheckText.Instance.SetStatus("Analyzing..");

        WWWForm webForm = new WWWForm();

        using (UnityWebRequest unityWebRequest = UnityWebRequest.Post(predictionEndpoint, webForm))
        {
            // Gets a byte array out of the saved image
            imageBytes = GetImageAsByteArray(imagePath);

            unityWebRequest.SetRequestHeader("Content-Type", "application/octet-stream");
            unityWebRequest.SetRequestHeader("Prediction-Key", predictionKey);

            // The upload handler will help uploading the byte array with the request
            unityWebRequest.uploadHandler = new UploadHandlerRaw(imageBytes);
            unityWebRequest.uploadHandler.contentType = "application/octet-stream";

            // The download handler will help receiving the analysis from Azure
            unityWebRequest.downloadHandler = new DownloadHandlerBuffer();
            CheckText.Instance.SetStatus("DownloadHandlerBuffer");

            //******************************************************************
            // Send the request
            yield return unityWebRequest.SendWebRequest();
            CheckText.Instance.SetStatus("Send the Request");

            string jsonResponse = unityWebRequest.downloadHandler.text;

            Debug.Log("response: " + jsonResponse);
            CheckText.Instance.SetStatus(jsonResponse);
            SplitJsonFile(jsonResponse);
            File.WriteAllText(@"C:\Users\kjm\Desktop\ngr", jsonResponse);

            // Create a texture. Texture size does not matter, since
            // LoadImage will replace with the incoming image size.
            Texture2D tex = new Texture2D(1, 1);
            tex.LoadImage(imageBytes);
            SceneOrganiser.Instance.quadRenderer.material.SetTexture("_MainTex", tex);

            //The response will be in JSON format, therefore it needs to be deserialized
            AnalysisRootObject analysisRootObject = new AnalysisRootObject();
            //CheckText.Instance.SetStatus("Send the Request3");
            analysisRootObject = JsonConvert.DeserializeObject<AnalysisRootObject>(jsonResponse);
            analysisRootObject = JsonUtility.FromJson<AnalysisRootObject>(jsonResponse);
            CheckText.Instance.SetStatus("Send the Request4");

            if (analysisRootObject != null)
            {
                    SceneOrganiser.Instance.FinaliseLabel(analysisRootObject);
                    CheckText.Instance.SetStatus(analysisRootObject.predictions[0].tagName);
            }
            else
                CheckText.Instance.SetStatus("analysisRootObject Null");
        }
    }

    /// <summary>
    /// Returns the contents of the specified image file as a byte array.
    /// </summary>
    static byte[] GetImageAsByteArray(string imageFilePath)
    {
        FileStream fileStream = new FileStream(imageFilePath, FileMode.Open, FileAccess.Read);

        BinaryReader binaryReader = new BinaryReader(fileStream);

        return binaryReader.ReadBytes((int)fileStream.Length);
    }

    public void SplitJsonFile(string jsonFileData)
    {
        if (jsonFileData != null)
        {
            List<string> textLines = new List<string>();
            List<string> findTagName = new List<string>();
            List<int> tagOrder = new List<int>();

            //textLines = jsonFileData.Split(separatorChar, System.StringSplitOptions.RemoveEmptyEntries);
            textLines.AddRange(jsonFileData.Split(separatorChar));
            //CheckText.Instance.SetStatus(textLines[0] + " " + textLines[1] + " " + textLines[2] + " " + textLines[3] + " " + textLines[4] + " " + textLines[5] + " " + textLines[6] + " "
            //    +textLines[7] + " " + textLines[8] + " " + textLines[9] + " " + textLines[10] + " " + textLines[11]);
            //CheckText.Instance.SetStatus(tagName[0] + " " + tagName[1] + " " + tagName[2] + " " + tagName[3]);

            for (int i = 0; i < tagName.Length; i++)
            {
                if (textLines.Contains(tagName[i]))
                {
                    int index = textLines.IndexOf(tagName[i]);
                    if (index > 0)
                    {
                        tagOrder.Add(index);
                    }
                }
            }
            tagOrder.Sort();
            //tagOrder.Reverse();
            for (int i = 0; i < tagOrder.Count; i++)
            {
                findTagName.Add(textLines[tagOrder[i]]);
                Debug.Log(textLines[tagOrder[i]]);
                Debug.Log(findTagName[i]);
            }
            CheckText.Instance.SetStatus(findTagName[0]);
        }

    }
}