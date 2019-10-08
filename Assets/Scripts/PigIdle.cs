using System.Linq;
using UnityEngine;
using System.Collections.Generic;
using System.Collections;

/// <summary>
/// Here will all of the pig behaviors be accessable by the player
/// </summary>
public class PigIdle : MonoBehaviour
{
    //Public variables through blackboard
    private float minTime = BlackBoard.minTime;
    private float maxTime = BlackBoard.maxTime;
    private float cooldownTime = BlackBoard.cooldown;

    public bool idle = true;

    private PigLearnController learnController;
    private List<Actions> actions = new List<Actions>();
    internal bool isActive = false;

    private void Awake()
    {
        learnController = FindObjectOfType<PigLearnController>();
        actions = FindObjectsOfType<Actions>().ToList();
    }

    private void Update()
    {
        if (idle && !isActive)
        {
            StartCoroutine(RandomAction());
        }
        else
        {
            StopCoroutine(RandomAction());
            StartCoroutine(Cooldown());
        }
    }

    private IEnumerator RandomAction()
    {
        float waitTime = UnityEngine.Random.Range(minTime, maxTime);
        yield return new WaitForSeconds(waitTime);
        PerformAction();
    }
    private IEnumerator Cooldown()
    {
        yield return new WaitForSeconds(cooldownTime);
        idle = true;
    }

    private void PerformAction()
    {
        foreach (Actions action in actions)
        {
            foreach (KeyValuePair<string, dynamic> command in action.commands)
            {
                action.Run(command.Value);
            }
        }
    }
}
