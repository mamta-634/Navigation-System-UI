using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public GameObject[] panels; // Array to hold references for panels
    private Stack<GameObject> panelStack = new Stack<GameObject>(); // Stack to store back navigation history
    private Stack<GameObject> forwardPanelStack = new Stack<GameObject>(); // Stack to store forward navigation history

    public Button[] panelButtons; // Array to hold references to UI buttons corresponding to panels
    public Button backButton; // Reference to the back button
    public Button forwardButton; // Reference to the forward button

    private void Start()
    {
        // Initialize the navigation with the first panel
        SwitchPanel(0);

        // Assign navigation methods to panel buttons
        for (int i = 0; i < panelButtons.Length; i++)
        {
            int index = i; // Capturing the current value of i for each iteration
            panelButtons[i].onClick.AddListener(() => SwitchPanel(index));
        }

        // Assign the GoBack method to the back button
        //backButton.onClick.AddListener(GoBack);

        // Assign the GoForward method to the forward button
        //forwardButton.onClick.AddListener(GoForward);

    }
    private void Update()
    {
        // Check for Android back button press
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            GoBack();
        }
    }

    // Method to switch between panels
    public void SwitchPanel(int panelIndex)
    {
        // Deactivate the current panel
        if (panelStack.Count > 0)
        {
            GameObject currentPanel = panelStack.Peek();
            currentPanel.SetActive(false);
        }

        // Activate the desired panel
        panels[panelIndex].SetActive(true);

        // Push the activated panel into the history stack
        panelStack.Push(panels[panelIndex]);

        // Clear forward history when a new panel is navigated to
        forwardPanelStack.Clear();
        Debug.Log(panelIndex);
        PrintPanelNames();
        PrintPanelstackNames();





        // Enable or disable back and forward buttons
        UpdateButtonInteractivity();

    }

    // Method to go back to the previous panel
    public void GoBack()
    {
        // If there are panels in the history stack
        if (panelStack.Count > 1)
        {
            // Deactivate the current panel
            GameObject currentPanel = panelStack.Pop();
            currentPanel.SetActive(false);

            // Push the current panel into the forward panel stack
            forwardPanelStack.Push(currentPanel);

            // Activate the previous panel
            GameObject previousPanel = panelStack.Peek();
            previousPanel.SetActive(true);

            // Enable or disable back and forward buttons
            UpdateButtonInteractivity();
        }
    }

    // Method to go forward to the next panel
    public void GoForward()
    {
        // If there are panels in the forward panel stack
        if (forwardPanelStack.Count > 0)
        {
            // Deactivate the current panel
            GameObject currentPanel = panelStack.Pop();
            currentPanel.SetActive(false);

            // Push the current panel into the history stack
            panelStack.Push(currentPanel);

            // Activate the next panel
            GameObject nextPanel = forwardPanelStack.Pop();
            nextPanel.SetActive(true);

            // Push the next panel into the history stack
            panelStack.Push(nextPanel);

            // Enable or disable back and forward buttons
            UpdateButtonInteractivity();
        }
    }

    // Method to enable or disable back and forward buttons based on navigation history
    private void UpdateButtonInteractivity()
    {
        backButton.interactable = panelStack.Count > 1;
        forwardButton.interactable = forwardPanelStack.Count > 0;
    }
    private void PrintPanelNames()
    {
        for (int i = 0; i < panels.Length; i++)
        {
            GameObject panel = panels[i];
            Debug.Log(panel.name);
        }
    }


    private void PrintPanelstackNames()
    {
        Debug.Log("Panel Stack History:");

        // Loop through the panelStack and print each panel's name along with its position in the stack
        int index = 1;
        foreach (GameObject panel in panelStack)
        {
            Debug.Log("Position " + index + ": " + panel.name);
            index++;
        }


    }
}