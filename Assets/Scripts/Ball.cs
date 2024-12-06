using System.Globalization;
using TMPro;
using UnityEngine;
using Zenject;

public class Ball : MonoBehaviour
{
    [Inject]
    private SignalBus _signalBus;

    [Inject]
    public void Constructor(SignalBus signalBus)
    {
        _signalBus = signalBus;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    { 
        if (collision.gameObject.CompareTag("Result"))
        {
            TextMeshProUGUI temp = collision.gameObject.GetComponentInChildren<TextMeshProUGUI>();

            if(!float.TryParse(temp.text, NumberStyles.Float, CultureInfo.InvariantCulture, out float dropChance))
            {
                print("Error during converting to float...");
                return;
            }

            _signalBus.FireId("ChangeMoneySignal",dropChance);
            gameObject.SetActive(false);
        }
    }
}
