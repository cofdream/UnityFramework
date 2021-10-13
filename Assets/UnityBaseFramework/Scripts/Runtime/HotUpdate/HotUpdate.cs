using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Cofdream
{
    public class HotUpdate
    {
        public bool HasNewVersionRes;

        public void HotUpedateAsset()
        {
            var remoteResVersion = 1;
            var localResVersion = 0;
            HasNewVersionRes = remoteResVersion > localResVersion;

            if (HasNewVersionRes)
            {
                UpdateAsset();
                Debug.Log("Update");
            }
            else
            {
                Debug.Log("Not Update");
            }

        }

        public void UpdateAsset()
        {
            // 下载需要更新的资源

            // 替换本地资源

        }
    }
}