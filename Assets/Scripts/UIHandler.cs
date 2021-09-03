using GracesGames.Common.Scripts;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIHandler : MonoBehaviour {

    public Text path;
    public Text loadFilePath;
    public GameObject folderList;
    public GameObject fileList;

    public GameObject folderPrefab;
    public GameObject filePrefab;

    public int startPosition = 50;

    public static int fileHeight  = 50;
    public static int folderHeight = 50;

    private FilleBrowserTest fileBrowser;

    private void SetupClickListeners()
    {
        // Hook up Directory Navigation methods to Directory Navigation Buttons
        Utilities.FindButtonAndAddOnClickListener("DirUpBtn", fileBrowser.DirectoryUp);
    }

    public void SetupPath(string path)
    {
        this.path.text = path;
    }

    public void resetFileBrowerPanels()
    {
        foreach (Transform child in folderList.transform)
        {
            GameObject.Destroy(child.gameObject);
        }

        foreach (Transform child in fileList.transform)
        {
            GameObject.Destroy(child.gameObject);
        }
        fileHeight = 50;
        folderHeight = 50;

    }

    public void UpdateLoadFileText(string loadfiletext)
    {
        loadFilePath.text = loadfiletext;
    }

    public void SetupFolderButton(string folder)
    {
        GameObject temp = Instantiate(folderPrefab, Vector3.zero, Quaternion.identity);
        temp.transform.SetParent(folderList.transform, false);
        temp.GetComponentInChildren<Text>().text = folder;
        folderHeight += 50;
    }

    public void SetupFileButton(string file)
    {
        GameObject temp = Instantiate(filePrefab, Vector3.zero, Quaternion.identity);
        temp.transform.SetParent(fileList.transform, false);
        temp.GetComponentInChildren<Text>().text = file;
        fileHeight += 50;
    }

	// Use this for initialization
	void Start () {
        fileBrowser = FindObjectOfType<FilleBrowserTest>();
        SetupClickListeners();
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
