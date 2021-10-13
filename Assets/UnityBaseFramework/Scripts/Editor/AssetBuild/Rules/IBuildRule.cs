using UnityEditor;

namespace CofdreamEditor.AssetBuild
{
    public delegate void CreateCallback(AssetBundleBuild assetBundleBuild);
    public interface IBuildRule
    {
        void CreateAssetBundleBuild(CreateCallback addAssetBundleBuild);
    }
}
