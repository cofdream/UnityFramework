using UnityEditor;

namespace CofdreamEditor.AssetBundle
{
    public delegate void CreateCallback(AssetBundleBuild assetBundleBuild);
    public interface IBuildRule
    {
        void CreateAssetBundleBuild(CreateCallback addAssetBundleBuild);
    }
}
