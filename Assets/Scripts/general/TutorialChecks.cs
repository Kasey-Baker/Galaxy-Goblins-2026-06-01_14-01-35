using UnityEngine;
using TMPro;

public class TutorialInputChecker : MonoBehaviour
{
    [SerializeField] private TMP_Text tutorialText;

    private bool pressedForward;
    private bool pressedLeft;
    private bool pressedBackward;
    private bool pressedRight;
    private bool pressedShift;

    private bool movementComplete;
    private bool fireComplete;
    private bool shiftComplete;

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
            if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow)) pressedForward = true;
            if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow)) pressedLeft = true;
            if (Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow)) pressedBackward = true;
            if (Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow)) pressedRight = true;

            if (pressedForward && pressedLeft && pressedBackward && pressedRight)
            {
                movementComplete = true;
                UpdateTutorialUI();
            }
        }

        if (!shiftComplete && (Input.GetKeyDown(KeyCode.LeftShift) || Input.GetKeyDown(KeyCode.RightShift)))
        {
            shiftComplete = true;
            UpdateTutorialUI();
        }

        if (!fireComplete && (Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.Z)))
        {
            fireComplete = true;
            UpdateTutorialUI();
        }

        if (movementComplete && shiftComplete && fireComplete)
        {
            Invoke("CompleteTutorial", 2f);
        }
    }

    private void UpdateTutorialUI()
    {
        string text = "";

        if (movementComplete) text += "<color=green>[X] Move with WASD / Arrow Keys</color>\n";
        else text += "<color=white>[ ] Move with WASD / Arrow Keys</color>\n";

        if (shiftComplete) text += "<color=green>[X] Press Shift to Slow Down</color>\n";
        else text += "<color=white>[ ] Press Shift to Slow Down</color>\n";

        if (fireComplete) text += "<color=green>[X] Press Left Click or Z to Fire</color>";
        else text += "<color=white>[ ] Press Left Click or Z to Fire</color>";

        tutorialText.text = text;
    }

    private void CompleteTutorial()
    {
        tutorialText.text = "<color=yellow>Tutorial Complete!</color>";
        gameObject.SetActive(false);
    }
}
