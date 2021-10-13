
namespace Cofdream.AssetLoad
{
    public enum LoadState : byte
    {
        NotLoad = 0,
        Loading = 1,
        Loaded = 2,
        Unload = 3,
        LoadError = 4,
    }
}