using Zenject;
using UnityEngine;

public class GameInstaller : MonoInstaller
{
    [SerializeField] private GameManager gameManager;
    [SerializeField] private HistoryPanel historyPanel;
    [SerializeField] private Ball ball;

    public override void InstallBindings()
    {
        Debug.Log("Installing Bindings...");

        SignalBusInstaller.Install(Container);
        Container.DeclareSignal<float>().WithId("ChangeMoneySignal");
        Debug.Log("Signal declared.");

        Container.Bind<GameManager>().FromInstance(gameManager).AsSingle();
        Container.Bind<HistoryPanel>().FromInstance(historyPanel).AsSingle();
        Container.Bind<Ball>().FromInstance(ball).AsSingle();

        Debug.Log("Bindings installed in GameInstaller.");
    }
}
