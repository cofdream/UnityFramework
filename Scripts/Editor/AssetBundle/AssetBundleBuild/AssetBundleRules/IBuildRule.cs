using UnityEditor;

namespace Cofdream.AssetEditor
{
    public delegate void CreateCallback(AssetBundleBuild assetBundleBuild);
    public interface IBuildRule
    {
        void CreateAssetBundleBuild(CreateCallback addAssetBundleBuild);
    }
}
