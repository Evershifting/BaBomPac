using UnityEngine;
using Zenject;

[CreateAssetMenu(fileName = "SOInstaller", menuName = "Installers/SOInstaller")]
public class SOInstaller : ScriptableObjectInstaller<SOInstaller>
{
    [SerializeField]
    Config _config;
    public override void InstallBindings()
    {
        Container.BindInstance(_config);
    }
}