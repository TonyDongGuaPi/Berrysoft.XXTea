﻿using System.Text;

namespace Berrysoft.XXTea
{
    public sealed class TeaCryptor : TeaCryptorBase
    {
        private const int Round = 32;

        public TeaCryptor() : base() { }
        public TeaCryptor(byte[] key) : base(key) { }
        public TeaCryptor(string key) : base(key) { }
        public TeaCryptor(string key, Encoding encoding) : base(key, encoding) { }

        private static void EncryptInternal(ref uint v0, ref uint v1, uint[] k)
        {
            uint sum = 0;
            int n = Round;
            unchecked
            {
                while (n-- > 0)
                {
                    sum += Delta;
                    v0 += ((v1 << 4) + k[0]) ^ (v1 + sum) ^ ((v1 >> 5) + k[1]);
                    v1 += ((v0 << 4) + k[2]) ^ (v0 + sum) ^ ((v0 >> 5) + k[3]);
                }
            }
        }

        private static void DecryptInternal(ref uint v0, ref uint v1, uint[] k)
        {
            uint sum = unchecked(Round * Delta);
            int n = Round;
            unchecked
            {
                while (n-- > 0)
                {
                    v1 -= ((v0 << 4) + k[2]) ^ (v0 + sum) ^ ((v0 >> 5) + k[3]);
                    v0 -= ((v1 << 4) + k[0]) ^ (v1 + sum) ^ ((v1 >> 5) + k[1]);
                    sum -= Delta;
                }
            }
        }

        protected override uint[] Encrypt(uint[] data)
        {
            for (int i = 0; i < data.Length; i += 2)
            {
                EncryptInternal(ref data[i], ref data[i + 1], uintKey);
            }
            return data;
        }

        protected override uint[] Decrypt(uint[] data)
        {
            for (int i = 0; i < data.Length; i += 2)
            {
                DecryptInternal(ref data[i], ref data[i + 1], uintKey);
            }
            return data;
        }
    }
}