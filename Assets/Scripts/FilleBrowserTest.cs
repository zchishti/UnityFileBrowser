using GracesGames.Common.Scripts;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class FilleBrowserTest : MonoBehaviour {

    private bool _isOpen;
    private string currentPath;
    private string currentFile;
    private string[] _fileExtensions;

    private UIHandler uiScript;

    // Stacks to keep track for backward and forward navigation feature
    private readonly FiniteStack<string> _backwardStack = new FiniteStack<string>();

    private readonly FiniteStack<string> _forwardStack = new FiniteStack<string>();

    private void SetUpFileBrowser(string startPath = "")
    {
        GameObject uiCanvas = GameObject.Find("Canvas");
        if (uiCanvas != null)
        {
            uiCanvas.SetActive(true);
            //Maybe Instantiate an Innstance of Canvas.  Otherwie have it already set up in game hirearchy.
        }
        else
        {
            Debug.LogError("Make sure there is a canvas GameObject present in the Hierarcy (Create UI/Canvas)");
        }

        SetupPath(startPath);
    }

    private void SetupPath(string startPath)
    {
        if (!String.IsNullOrEmpty(startPath) && Directory.Exists(startPath))
        {
            currentPath = startPath;
            uiScript.SetupPath(currentPath);
        }
        else
        {
            currentPath = Directory.GetCurrentDirectory();
            uiScript.SetupPath(currentPath);
        }
    }

    public bool IsOpen()
    {
        return _isOpen;
    }

    public void DirectoryBackward()
    {
        // See if there is anything on the backward stack
        if (_backwardStack.Count > 0)
        {
            // If so, push it to the forward stack
            _forwardStack.Push(currentPath);
        }

        // Get the last path entry
        string backPath = _backwardStack.Pop();
        if (backPath != null)
        {
            // Set path and update the file browser
            currentPath = backPath;
            UpdateFileBrowser();
        }
    }

    public void DirectoryForward()
    {
        // See if there is anything on the redo stack
        if (_forwardStack.Count > 0)
        {
            // If so, push it to the backward stack
            _backwardStack.Push(currentPath);
        }

        // Get the last level entry
        string forwardPath = _forwardStack.Pop();
        if (forwardPath != null)
        {
            // Set path and update the file browser
            currentPath = forwardPath;
            UpdateFileBrowser();
        }
    }

    public void DirectoryUp()
    {
        _backwardStack.Push(currentPath);
        if (!IsTopLevelReached())
        {
            currentPath = Directory.GetParent(currentPath).FullName;
            UpdateFileBrowser();
        }
        else
        {
            UpdateFileBrowser(true);
        }
    }

    private bool IsTopLevelReached()
    {
        return Directory.GetParent(currentPath) == null;
    }

    public void SelectFile()
    {
       //Avtion File Select Event
     
    }

    // Updates the file browser by updating the path, file name, directories and files
    private void UpdateFileBrowser(bool topLevel = false)
    {
        UpdatePathText();
        UpdateLoadFileText();
        uiScript.resetFileBrowerPanels();
        BuildDirectories(topLevel);
        BuildFiles();
    }

    private void UpdatePathText()
    {
        uiScript.SetupPath(currentPath);
    }

    private void UpdateLoadFileText()
    {
        uiScript.UpdateLoadFileText(currentFile);
    }

    private void BuildDirectories(bool topLevel)
    {
        // Get the directories
        string[] directories = Directory.GetDirectories(currentPath);
        // If the top level is reached return the drives
        if (topLevel)
        {
            if (IsWindowsPlatform())
            {
                directories = Directory.GetLogicalDrives();
            }
            else if (IsMacOsPlatform())
            {
                directories = Directory.GetDirectories("/Volumes");
            }
        }


        // Apply Alphanumeric sort to directories
        Array.Sort(directories, new AlphanumComparatorFast());

        // For each directory in the current directory, create a DirectoryButton and hook up the DirectoryClick method
        foreach (string dir in directories)
        {
            if (Directory.Exists(dir))
            {
                // uiScript.SetupFolderButton(CleanDirName(dir));
                uiScript.SetupFolderButton(dir);
            }
        }
    }

    public string CleanDirName(string d)
    {
        return d.Replace(currentPath,"");
    }

    // Returns whether the application is run on a Windows Operating System
    private bool IsWindowsPlatform()
    {
        return (Application.platform == RuntimePlatform.WindowsEditor ||
                Application.platform == RuntimePlatform.WindowsPlayer);
    }
    
    // Returns whether the application is run on a Mac Operating System
    private bool IsMacOsPlatform()
    {
        return (Application.platform == RuntimePlatform.OSXEditor ||
                Application.platform == RuntimePlatform.OSXPlayer);
    }

    // Creates a FileButton for each file in the current path
    private void BuildFiles()
    {
        // Get the files
        string[] files = Directory.GetFiles(currentPath);
        
        // Apply Alphanumeric sort to files
        Array.Sort(files, new AlphanumComparatorFast());

        // For each file in the current directory, create a FileButton and hook up the FileClick method
        foreach (string file in files)
        {
            if (!File.Exists(file)) return;

            //    uiScript.SetupFileButton(CleanDirName(file));
            uiScript.SetupFileButton(file);
        }
    }

    public void DirectoryClick(string path)
    {
        
        _backwardStack.Push(currentPath.Clone() as string);
        currentPath = path;
        UpdateFileBrowser();
    }

    public void FileClick(string clickedFile)
    {
        currentFile = clickedFile;
        UpdateFileBrowser();
    }

    private void Destroy()
    {
        // Set _isOpen
        _isOpen = false;
        GameObject uiCanvas = GameObject.Find("Canvas");
        uiCanvas.SetActive(false);
        //   Destroy(GameObject.Find("FileBrowserUI"));
        //   Destroy(GameObject.Find("FileBrowser"));
    }

    // Use this for initialization
    void Start () {
        uiScript = GameObject.FindObjectOfType<UIHandler>();
        SetUpFileBrowser();
        UpdateFileBrowser(); 
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
