﻿using TMPro;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using System.Collections.Generic;

/// <summary>
/// Here will all of the pig behaviors be accessable by the player
/// </summary>
public class PigController : MonoBehaviour
{
    //Public variables through blackboard
    private int buttonOffset = BlackBoard.ButtonOffset;

    private Vector2 buttonSize;
    private bool active = false;

    private GameInput controls;
    private Transform userInterface;
    private GameObject commandButton;
    private PigLearnController learnController;
    private PigIdle pigIdle;

    private List<Actions> actions = new List<Actions>();
    private List<GameObject> commandButtons = new List<GameObject>();

    private void Awake()
    {
        controls = new GameInput();
        commandButton = (GameObject)Resources.Load("CommandButton");
        userInterface = GetComponentInChildren<Canvas>().transform;
        buttonSize = commandButton.GetComponent<RectTransform>().rect.size;
        learnController = FindObjectOfType<PigLearnController>();

        if (FindObjectOfType<PigIdle>())
            pigIdle = FindObjectOfType<PigIdle>();

        actions = FindObjectsOfType<Actions>().ToList();
    }

    private void OnEnable()
    {
        controls.Player.Cancel.performed += ctx => Reset();
        controls.Player.Interact.performed += ctx => ShowCommands(Input.mousePosition);
        controls.Player.Cancel.Enable();
        controls.Player.Interact.Enable();
    }

    private void OnDisable()
    {
        controls.Player.Cancel.performed -= ctx => Reset();
        controls.Player.Interact.performed -= ctx => ShowCommands(Input.mousePosition);
        controls.Player.Cancel.Disable();
        controls.Player.Interact.Disable();
    }

    /// <summary>
    /// Show all of the commands the player has access to
    /// </summary>
    /// <param name="input">Mouse input for button position</param>
    private void ShowCommands(Vector2 input)
    {
        Vector2 offset = new Vector2();

        if (active) { return; }

        ClearCommands(ref commandButtons);

        List<Actions> copy = new List<Actions>(actions);
        if (input.y < Screen.height / 2) { copy.Reverse(); }

        foreach (Actions action in copy)
        {
            foreach (KeyValuePair<string, dynamic> command in action.commands)
            {
                if (input.x < Screen.width / 2 && input.y < Screen.height / 2)
                {
                    offset.y += buttonSize.y + buttonOffset;
                    offset.x = buttonSize.x / 2;
                }
                else if (input.x > Screen.width / 2 && input.y < Screen.height / 2)
                {
                    offset.y += buttonSize.y + buttonOffset;
                    offset.x = -buttonSize.x / 2;
                }
                else if (input.x > Screen.width / 2 && input.y > Screen.height / 2)
                {
                    offset.y -= buttonSize.y + buttonOffset;
                    offset.x = -buttonSize.x / 2;
                }
                else if (input.x < Screen.width / 2 && input.y > Screen.height / 2)
                {
                    offset.y -= buttonSize.y + buttonOffset;
                    offset.x = buttonSize.x / 2;
                }

                commandButtons.Add(SetupButton(input, offset, action, command));
            }
        }

        active = true;
    }

    /// <summary>
    /// Setup the command button
    /// </summary>
    /// <param name="input">Mouse position</param>
    /// <param name="offset">The offset for the button from root</param>
    /// <param name="action"></param>
    /// <param name="command"></param>
    /// <returns></returns>
    private GameObject SetupButton(Vector2 input, Vector2 offset, Actions action, KeyValuePair<string, dynamic> command)
    {
        GameObject buttonObject = Instantiate(commandButton, userInterface, true);
        buttonObject.transform.position = input + offset;
        buttonObject.name = buttonObject.name.Replace("(Clone)", "");

        Button buttonComponent = buttonObject.GetComponent<Button>();
        if (pigIdle)
            buttonComponent.onClick.AddListener(delegate { pigIdle.isActive = true; });
        buttonComponent.onClick.AddListener(delegate { action.Run(command.Value); });
        buttonComponent.onClick.AddListener(delegate { Reset(); });

        TextMeshProUGUI buttonText = buttonObject.GetComponentInChildren<TextMeshProUGUI>();
        buttonText.text = command.Key;

        return buttonObject;
    }

    /// <summary>
    /// Destroys all gameobjects in a list
    /// </summary>
    /// <param name="list">List to destroy</param>
    private void ClearCommands(ref List<GameObject> list)
    {
        foreach (GameObject item in list)
        {
            Destroy(item);
        }
        list.Clear();
    }

    /// <summary>
    /// Reset the selector
    /// </summary>
    private void Reset()
    {
        active = false;
        ClearCommands(ref commandButtons);
    }
}
