using UnityEngine;
using TMPro;

public class TutorialInputChecker : MonoBehaviour
{
    [SerializeField] private TMP_Text tutorialText;

    private bool pressedW;
    private bool pressedA;
    private bool pressedS;
    private bool pressedD;
    private bool pressedMouse1;

    private bool movementComplete;
    private bool clickComplete;

    void Start()
    {
        UpdateTutorialUI();
    }

    void Update()
    {
        CheckInputs();
    }

    private void CheckInputs()
    {
        if (!movementComplete)
        {
            if (Input.GetKeyDown(KeyCode.W)) pressedW = true;
            if (Input.GetKeyDown(KeyCode.A)) pressedA = true;
            if (Input.GetKeyDown(KeyCode.S)) pressedS = true;
            if (Input.GetKeyDown(KeyCode.D)) pressedD = true;

            if (pressedW && pressedA && pressedS && pressedD)
            {
                movementComplete = true;
                UpdateTutorialUI();
            }
        }

        if (!clickComplete && Input.GetMouseButtonDown(0))
        {
            clickComplete = true;
            UpdateTutorialUI();
        }

        if (movementComplete && clickComplete)
        {
            Invoke("CompleteTutorial", 2f);
        }
    }

    private void UpdateTutorialUI()
    {
        string text = "";

        if (movementComplete) text += "<color=green>[X] Move with WASD</color>\n";
        else text += "<color=white>[ ] Move with WASD</color>\n";

        if (clickComplete) text += "<color=green>[X] Press Left Click to Fire</color>";
        else text += "<color=white>[ ] Press Left Click to Fire</color>";

        tutorialText.text = text;
    }

    private void CompleteTutorial()
    {
        tutorialText.text = "<color=yellow>Tutorial Complete!</color>";
        gameObject.SetActive(false);
    }
}
