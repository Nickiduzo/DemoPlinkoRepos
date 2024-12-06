using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class GameManager : MonoBehaviour
{
    [Header("Ball Description")]
    [SerializeField] private Transform spawnPosition;
    [SerializeField] private GameObject ball;
    [HideInInspector] public int currentBall;

    [Header("Bottom Colliders")]
    [SerializeField] private Collider2D[] greenChances;
    [SerializeField] private Collider2D[] yellowChances;
    [SerializeField] private Collider2D[] redChances;

    [Header("Text and Visualization")]
    [SerializeField] private TextMeshProUGUI betAmount;
    [SerializeField] private TextMeshProUGUI moneyAmount;
    [SerializeField] private GameObject rebootMoney;
    [SerializeField] private GameObject tableOfBets;

    [SerializeField] private Button[] buttons;

    private float money;
    private float bet;

    private float[] bets = new float[] { 0.1f, 0.2f, 0.3f, 0.4f, 0.5f, 0.6f, 0.7f, 0.8f, 1.2f, 2.0f, 4.0f, 10f, 20f, 50f, 100f};
    private int currentIndex = 0;

    [Inject(Id = "ChangeMoneySignal")]
    private SignalBus _signalBus;
    private void Awake()
    {
        Initialization();

        _signalBus.Subscribe<float>(ChangeMoneyAmount);
    }

    private void Update()
    {
        if (money <= 100) rebootMoney.SetActive(true);
    }

    private void Initialization()
    {
        currentBall = 1;
        bet = bets[currentIndex];
        money = 3000.0f;
        betAmount.text = bet.ToString();
        moneyAmount.text = $"{money:F2} USD";
    }

    public void RebootMoney()
    {
        money = 3000f;
        moneyAmount.text = $"{money:F2} USD";
        rebootMoney.SetActive(false);
    }
    public void ChangeBet(int direction)
    {
        switch(direction)
        {
            case 0:
                //decrease
                if (currentIndex > 0) currentIndex--;
                bet = bets[currentIndex];
                betAmount.text = bet.ToString("F2");
                break;
            case 1:
                if(currentIndex < bets.Length - 1) currentIndex++;
                bet = bets[currentIndex];
                betAmount.text = bet.ToString("F2");
                //increase
                break;
        }
    }
    public void SetBetFromTable(int indexOfBet)
    {
        if(indexOfBet > 0 && indexOfBet < bets.Length)
        {
            currentIndex = indexOfBet;
            bet = bets[indexOfBet];
            betAmount.text = bet.ToString();
            tableOfBets.SetActive(false);
        }
    }
    private void ChangeMoneyAmount(float amount)
    {
        money += bet * amount;
        if (money < 0) money = 0;

        moneyAmount.text = $"{money:F2} USD";
        ButtonsHandler(true);
    }
    public void Restart() => SwitchTrigers(currentBall);
    
    private void Launch(Color color)
    {
        money -= bet;
        moneyAmount.text = $"{money:F2} USD";
        ball.transform.position = spawnPosition.position;
        Image ballImage = ball.GetComponent<Image>();
        ballImage.color = color;
        ball.SetActive(true);

        ButtonsHandler(false);
    }

    public void SwitchTrigers(int numberOfCollider)
    {
        currentBall = numberOfCollider;
        ColliderHandler(false, greenChances);
        ColliderHandler(false, yellowChances);
        ColliderHandler(false, redChances);

        switch (numberOfCollider)
        {
            case 1:
                ColliderHandler(true, greenChances);
                Launch(Color.green);
                break;
            case 2:
                ColliderHandler(true, yellowChances);
                Launch(Color.yellow);
                break;
            case 3:
                ColliderHandler(true, redChances);
                Launch(Color.red);
                break;
            default:
                Debug.LogError("Invalid ball number!");
                break;
        }
    }

    private void ButtonsHandler(bool flag)
    {
        foreach (Button button in buttons)
            button.interactable = flag;
    }

    private void ColliderHandler(bool flag, Collider2D[] colliders)
    {
        foreach (Collider2D collider in colliders)
        {
            collider.isTrigger = flag;
            collider.enabled = flag;
        }
    }

    private void OnDisable()
    {
       _signalBus.Unsubscribe<float>(ChangeMoneyAmount);
    }
}
