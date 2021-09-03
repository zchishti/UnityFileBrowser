using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonSelect : MonoBehaviour {

    private FilleBrowserTest fileBrowser;

	public void buttonClick()
    {
        string dirText = getButtonText();
        fileBrowser.DirectoryClick(dirText);
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
