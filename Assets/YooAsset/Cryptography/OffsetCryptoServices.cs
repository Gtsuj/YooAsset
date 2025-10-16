using UnityEngine;

namespace YooAsset
{
#if UNITY_EDITOR
    public class OffsetEncryptServices : IEncryptionServices
    {
        public EncryptResult Encrypt(EncryptFileInfo fileInfo)
        {
            var bytes = OffsetCryptoHelper.Encrypt(fileInfo.FileLoadPath);

            return new EncryptResult()
            {
                Encrypted = true,
                EncryptedData = bytes,
            };
        }
    }
#endif

    public class OffsetDecryptionServices : IDecryptionServices
    {
        /// <summary>
        /// 同步方式获取解密的资源包对象
        /// 注意：加载流对象在资源包对象释放的时候会自动释放
        /// </summary>
        DecryptResult IDecryptionServices.LoadAssetBundle(DecryptFileInfo fileInfo)
        {
            DecryptResult decryptResult = default;
            decryptResult.ManagedStream = null;
            decryptResult.Result = AssetBundle.LoadFromFile(fileInfo.FileLoadPath, fileInfo.FileLoadCRC, GetFileOffset(fileInfo.BundleName));
            return decryptResult;
        }

        /// <summary>
        /// 异步方式获取解密的资源包对象
        /// 注意：加载流对象在资源包对象释放的时候会自动释放
        /// </summary>
        DecryptResult IDecryptionServices.LoadAssetBundleAsync(DecryptFileInfo fileInfo)
        {
            DecryptResult decryptResult = default;
            decryptResult.ManagedStream = null;
            decryptResult.CreateRequest = AssetBundle.LoadFromFileAsync(fileInfo.FileLoadPath, fileInfo.FileLoadCRC, GetFileOffset(fileInfo.BundleName));
            return decryptResult;
        }

        public DecryptResult LoadAssetBundleFallback(DecryptFileInfo fileInfo)
        {
            throw new System.NotImplementedException();
        }

        /// <summary>
        /// 获取解密的字节数据
        /// </summary>
        byte[] IDecryptionServices.ReadFileData(DecryptFileInfo fileInfo)
        {
            throw new System.NotImplementedException();
        }

        /// <summary>
        /// 获取解密的文本数据
        /// </summary>
        string IDecryptionServices.ReadFileText(DecryptFileInfo fileInfo)
        {
            throw new System.NotImplementedException();
        }

        private static ulong GetFileOffset(string fileName)
        {
            return OffsetCryptoHelper.GetOffsetLength(fileName);
        }
    }
}
