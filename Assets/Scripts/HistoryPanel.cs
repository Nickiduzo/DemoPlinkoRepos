using System.Collections.Generic;
using TMPro;
using UnityEditor.PackageManager.UI;
using UnityEngine;
using Zenject;

public class HistoryPanel : MonoBehaviour
{
    [SerializeField] private Transform historyPanel;

    [SerializeField] private GameObject usualRed;
    [SerializeField] private GameObject usualGreen;
    [SerializeField] private GameObject usualYellow;
    
    [SerializeField] private GameObject lowerRed;
    [SerializeField] private GameObject lowerGreen;
    [SerializeField] private GameObject lowerYellow;

    [SerializeField] private GameManager gameManager;

    private List<GameObject> history = new List<GameObject>();

    [Inject(Id = "ChangeMoneySignal")]
    private SignalBus _signalBus;
    private void Awake()
    {
        _signalBus.Subscribe<float>(AddToHistoryPanel);
    }

    private void AddToHistoryPanel(float value)
    {
        int maxHistoryCount = 13;

        if (history.Count >= maxHistoryCount)
        {
            Destroy(history[0]);
            history.RemoveAt(0);
        }

        switch (gameManager.currentBall)
        {
            case 1:
                CreateHistoryElement(value >= 1 ? usualGreen : lowerGreen, value);
                break;
            case 2:
                CreateHistoryElement(value >= 1 ? usualYellow : lowerYellow, value);
                break;
            case 3:
                CreateHistoryElement(value >= 1 ? usualRed : lowerRed, value);
                break;
        }
    }

    private void CreateHistoryElement(GameObject prefab, float value)
    {
        var element = Instantiate(prefab, historyPanel.transform.position, Quaternion.identity);
        var text = element.GetComponentInChildren<TextMeshProUGUI>();
        text.text = value.ToString("F2");
        element.transform.SetParent(historyPanel.transform);
        element.GetComponent<RectTransform>().localScale = Vector3.one;
        history.Add(element);
    }

    private void OnDisable()
    {
        _signalBus.Unsubscribe<float>(AddToHistoryPanel);
    }
}
