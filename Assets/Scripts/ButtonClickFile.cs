using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonClickFile : MonoBehaviour
{

    private FilleBrowserTest fileBrowser;

    public void buttonClick()
    {
        string fileText = getButtonText();
        fileBrowser.FileClick(fileText);
    }

    public string getButtonText()
    {
        return gameObject.GetComponentInChildren<Text>().text;
    }

    private void Start()
    {
        fileBrowser = GameObject.FindObjectOfType<FilleBrowserTest>();
    }

}
