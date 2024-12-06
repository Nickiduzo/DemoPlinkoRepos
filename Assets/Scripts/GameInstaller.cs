using Zenject;
using UnityEngine;

public class GameInstaller : MonoInstaller
{
    [SerializeField] private GameManager gameManager;
    [SerializeField] private HistoryPanel historyPanel;
    [SerializeField] private Ball ball;

    public override void InstallBindings()
    {
        SignalBusInstaller.Install(Container);

        Container.DeclareSignal<float>().WithId("ChangeMoneySignal");

        Container.Bind<GameManager>().FromInstance(gameManager).AsSingle();
        Container.Bind<HistoryPanel>().FromInstance(historyPanel).AsSingle();
        Container.Bind<Ball>().FromInstance(ball).AsSingle();
    }
}
