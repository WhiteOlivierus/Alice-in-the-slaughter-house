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

    public bool isActive = false;
    private bool idleling = true;

    private bool raRunning = false;
    private bool cdRunning = false;

    private PigLearnController learnController;
    private List<Actions> actions = new List<Actions>();

    private void Awake()
    {
        learnController = FindObjectOfType<PigLearnController>();
        actions = FindObjectsOfType<Actions>().ToList();
    }

    private void Update()
    {
        if (idleling && !isActive)
        {
            if(!raRunning)
                StartCoroutine(RandomAction());
        }
        else
        {
            if(!cdRunning)
                StartCoroutine(Cooldown());
        }
    }

    private IEnumerator RandomAction()
    {
        raRunning = true;
        float waitTime = UnityEngine.Random.Range(minTime, maxTime);
        yield return new WaitForSeconds(waitTime);
        idleling = false;
        PerformAction();
        raRunning = false;
    }

    private IEnumerator Cooldown()
    {
        cdRunning = true;
        yield return new WaitForSeconds(cooldownTime);
        idleling = true;
        cdRunning = false;
    }

    private void PerformAction()
    {
        int indexAction = Random.Range(0, actions.Count());
        int indexCommand = Random.Range(0, actions[indexAction].commands.Count());
        string key = actions[indexAction].commands.Keys.ToList()[indexCommand];
        dynamic value = actions[indexAction].commands[key];
        actions[indexAction].Run(value);
    }
}
