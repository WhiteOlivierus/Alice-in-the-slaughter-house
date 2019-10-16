using UnityEngine;
using TMPro;

public class RangeCounter : MonoBehaviour
{
    [SerializeField]
    private string dataName;
    [SerializeField]
    private int max;
    [SerializeField]
    private int min;

    private TextMeshProUGUI dataNameText;
    private TextMeshProUGUI maxText;
    private TextMeshProUGUI minText;

    public int Min
    {
        get { return min; }
        set
        {
            min = min + 1;
            SetObject();
        }
    }
    public int Max
    {
        get { return max; }
        set
        {
            max = value;
            SetObject();
        }
    }
    public string DataName
    {
        get { return dataName; }
        set
        {
            dataName = value;
            SetObject();
        }
    }

    private void Start()
    {
        GetDataObjects();
        SetObject();
    }

    private void SetObject()
    {
        dataNameText.GetComponent<TextMeshProUGUI>().text = dataName;
        maxText.GetComponent<TextMeshProUGUI>().text = max.ToString();
        minText.GetComponent<TextMeshProUGUI>().text = min.ToString();
    }

    private void GetDataObjects()
    {
        foreach (Transform child in transform)
        {
            if (child.name == "Type")
            {
                dataNameText = child.GetComponent<TextMeshProUGUI>();
            }
            else if (child.name == "Data")
            {
                foreach (Transform subChild in child.transform)
                {
                    if (subChild.name == "Max")
                    {
                        maxText = subChild.GetComponent<TextMeshProUGUI>();
                    }
                    else if (subChild.name == "Min")
                    {
                        minText = subChild.GetComponent<TextMeshProUGUI>();
                    }
                }
            }
        }
    }
}
