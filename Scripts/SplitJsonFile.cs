using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Text.RegularExpressions;
using System;
using System.Linq;

public class SplitJsonFile : MonoBehaviour
{

    [Serializable]
    public class Prediction
    {
        public float probability { get; set; }
        public string tagName { get; set; }
    }
    /// <summary>
    /// Split JsonFile
    /// </summary>
    char separatorChar = '"';
    public string[] tagName = new string[] { "chair", "swivelchair", "laptop", "table" };

    /// <summary>
    /// Current threshold accepted for displaying the label
    /// Reduce this value to display the recognition more often
    /// </summary>
    internal float probabilityThreshold = 0.02f;

    void Start()
    {
        FindTagName(this.gameObject.GetComponent<TextMesh>().text);
        Debug.Log(this.gameObject.GetComponent<TextMesh>().text);
    }

    public void FindTagName(string jsonFileData)
    {
        if (jsonFileData != null)
        {
            List<string> textLines = new List<string>();
            List<string> findTagName = new List<string>();
            List<int> tagOrder = new List<int>();
            List<Prediction> predictions = new List<Prediction> { };

    //textLines = jsonFileData.Split(separatorChar, System.StringSplitOptions.RemoveEmptyEntries);
    textLines.AddRange(jsonFileData.Split(separatorChar));
            Debug.Log(textLines);
            //CheckText.Instance.SetStatus(textLines[0] + " " + textLines[1] + " " + textLines[2] + " " + textLines[3] + " " + textLines[4] + " " + textLines[5] + " " + textLines[6] + " "
            //    +textLines[7] + " " + textLines[8] + " " + textLines[9] + " " + textLines[10] + " " + textLines[11]);
            //CheckText.Instance.SetStatus(tagName[0] + " " + tagName[1] + " " + tagName[2] + " " + tagName[3]);

            for (int i = 0; i < textLines.Count; i++)
            {
                for(int j = 0; j < tagName.Length; j++)
                {
                    if (textLines[i] == tagName[j])
                    {
                        tagOrder.Add(i);
                    }
                }
            }
            /*
            for (int i = 0; i < tagName.Length; i++)
            {
                if (textLines.Contains(tagName[i]))
                {
                    Debug.Log(tagName[i]);
                    int index = textLines.IndexOf(tagName[i]);
                    Debug.Log(index);
                    if (index >0)
                    {
                        tagOrder.Add(index);
                    }
                }
            }
            */
            for (int i = 0; i < tagOrder.Count; i++)
            {
                Prediction temp = new Prediction();
                temp.tagName = textLines[tagOrder[i]];
                //temp.probability = float.Parse(Regex.Replace(textLines[tagOrder[i] + 2], @"[^a-zA-Z0-9가-힣\s]", ""));
                temp.probability = ConvertTofloat(textLines[tagOrder[i] + 2]) ;
                predictions.Add(temp);
                Debug.Log(tagOrder[i]);
                Debug.Log(predictions[i].tagName);
                Debug.Log(predictions[i].probability);
                //predictions[i].tagName = textLines[tagOrder[i]];
                //predictions[i].probability = float.Parse(Regex.Replace(textLines[tagOrder[i] + 2], @"[^0-9]", "1"));
                //Debug.Log(tagOrder[i]);
                //Debug.Log(predictions[i].tagName);
                //Debug.Log(predictions[i].probability);
                //Debug.Log(float.Parse(Regex.Replace(textLines[tagOrder[i] + 2], @"[^0-9]", "1")));
            }

            FindBestTag(predictions);
        }
    }

    private float ConvertTofloat(string str)
    {
        Regex r = new Regex(@"[0-9]*\.*[0-9]+");
        Match m = r.Match(str);
        return float.Parse(m.Value);
    }

    /// <summary>
    /// Set the Tags as Text of the last label created. 
    /// </summary>
    public void FindBestTag(List<Prediction> predictions)
    {
        if (predictions != null)
        {
            // Sort the predictions to locate the highest one
            List<Prediction> sortedPredictions = new List<Prediction>();
            sortedPredictions = predictions.OrderByDescending(p => p.probability).ToList();

            for (int i = 0; i < sortedPredictions.Count; i++)
            {
                if (sortedPredictions[i].probability > probabilityThreshold)
                {
                    Debug.Log(sortedPredictions[i].tagName + ", " + sortedPredictions[i].probability);
                }
            }
        }
    }
}
