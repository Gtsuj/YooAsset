using System;
using System.IO;
using System.Text;

namespace YooAsset
{
    /// <summary>
    /// 偏移加密类
    /// </summary>
    public static class OffsetCryptoHelper
    {
        // 固定偏移字段
        private const string OFFSET_STRING = "UnityFS";

        /// <summary>
        /// 偏移加密，偏移长度 = 文件名 + UnityFS
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static byte[] Encrypt(string path)
        {
            string fileName = Path.GetFileName(path);
            var offsetBytes = Encoding.ASCII.GetBytes(OFFSET_STRING);

            byte[] fileData = File.ReadAllBytes(path);
            var encryptedData = new byte[fileData.Length + fileName.Length + offsetBytes.Length];

            for (int i = 0; i < fileName.Length; i++)
            {
                encryptedData[i] = (byte)UnityEngine.Random.Range(0, 255);
            }
            Buffer.BlockCopy(offsetBytes, 0, encryptedData, fileName.Length, offsetBytes.Length);
            Buffer.BlockCopy(fileData, 0, encryptedData, fileName.Length + offsetBytes.Length, fileData.Length);

            return encryptedData;
        }

        public static ulong GetOffsetLength(string fileName)
        {
            return (ulong)(fileName.Length + OFFSET_STRING.Length);
        }

        /// <summary>
        /// 剔除掉fileName hash部分并返回偏移长度
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns></returns>
        public static ulong GetOffsetLengthWithoutHash(string filePath)
        {
            string fileName = Path.GetFileName(filePath);
            int lastUnderscoreIndex = fileName.LastIndexOf('_');
            fileName = $"{fileName.Remove(lastUnderscoreIndex)}.bundle";

            return (ulong)(fileName.Length + Encoding.ASCII.GetByteCount(OFFSET_STRING));
        }
    }
}
