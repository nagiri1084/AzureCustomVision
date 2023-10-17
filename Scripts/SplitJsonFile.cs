using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class SplitJsonFile : MonoBehaviour
{
    /// <summary>
    /// Split JsonFile
    /// </summary>
    char separatorChar = '"';
    public string[] tagName = new string[] { "chair", "swivelchair", "laptop", "table" };

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

            //textLines = jsonFileData.Split(separatorChar, System.StringSplitOptions.RemoveEmptyEntries);
            textLines.AddRange(jsonFileData.Split(separatorChar));
            Debug.Log(textLines);
            //CheckText.Instance.SetStatus(textLines[0] + " " + textLines[1] + " " + textLines[2] + " " + textLines[3] + " " + textLines[4] + " " + textLines[5] + " " + textLines[6] + " "
            //    +textLines[7] + " " + textLines[8] + " " + textLines[9] + " " + textLines[10] + " " + textLines[11]);
            //CheckText.Instance.SetStatus(tagName[0] + " " + tagName[1] + " " + tagName[2] + " " + tagName[3]);

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
            tagOrder.Sort();
            //tagOrder.Reverse();
            for (int i = 0; i < tagOrder.Count; i++)
            {
                findTagName.Add(textLines[tagOrder[i]]);
                Debug.Log(tagOrder[i]);
                Debug.Log(findTagName[i]);
            }
        }
    }
}
