#region Description
using System;
using System.IO;
using VTDev.Libraries.CEXEngine.Crypto.Common;
using VTDev.Libraries.CEXEngine.Crypto.Digest;
#endregion

namespace Test
{
    /// <summary>
    /// The official Blake2 Vector KATs from the Blake2 package; tests all (2S, 2SP 2B, and 2BP) variants.
    /// <para>https://github.com/BLAKE2/BLAKE2</para>
    /// </summary>
    public class Blake2Test : ITest
    {
        #region Constants
        private const string DESCRIPTION = "Blake2 Vector KATs; tests Blake 256/512 digests.";
        private const string FAILURE = "FAILURE! ";
        private const string SUCCESS = "SUCCESS! All Blake tests have executed succesfully.";
        const string DMK_INP = "in:	";
        const string DMK_KEY = "key:	";
        const string DMK_HSH = "hash:	";
        const string KAT2B = "blake2b-kat.txt";
        const string KAT2BP = "blake2bp-kat.txt";
        const string KAT2S = "blake2s-kat.txt";
        const string KAT2SP = "blake2sp-kat.txt";
        #endregion

        #region Events
        public event EventHandler<TestEventArgs> Progress;
        protected virtual void OnProgress(TestEventArgs e)
        {
            var handler = Progress;
            if (handler != null) handler(this, e);
        }
        #endregion

        #region Properties
        /// <summary>
        /// Get: Test Description
        /// </summary>
        public string Description { get { return DESCRIPTION; } }
        #endregion

        #region Public Methods
        /// <summary>
        /// Blake Vector KATs; tests Blake 256/512 digests.
        /// Throws on all failures.
        /// </summary>
        /// 
        /// <returns>State</returns>
        public string Run()
        {
            try
            {
                string path = System.AppDomain.CurrentDomain.BaseDirectory;
                path = path.Substring(0, path.LastIndexOf("bin")) + @"Tests\Vectors\Blake2\";
                if (!File.Exists(path + KAT2B) || !File.Exists(path + KAT2BP) || !File.Exists(path + KAT2S) || !File.Exists(path + KAT2SP))
                    throw new Exception(FAILURE + "Can not find the KAT Vector file. Check the path!");

                OnProgress(new TestEventArgs("Testing Sequential and Parallel versions 2B, 2BP, 2S, 2SP.."));

                Blake2BpTest(path + KAT2B, false);
                OnProgress(new TestEventArgs("Passed Blake2B 512 vector tests.."));
                Blake2BpTest(path + KAT2BP, true);
                OnProgress(new TestEventArgs("Passed Blake2BP 512 vector tests.."));
                Blake2SpTest(path + KAT2S, false);
                OnProgress(new TestEventArgs("Passed Blake2S 256 vector tests.."));/**/
                Blake2SpTest(path + KAT2SP, true);
                OnProgress(new TestEventArgs("Passed Blake2SP 256 vector tests.."));

                return SUCCESS;
            }
            catch (Exception Ex)
            {
                string message = Ex.Message == null ? "" : Ex.Message;
                throw new Exception(FAILURE + message);
            }
        }
        #endregion

        #region Tests
        private void Blake2BpTest(string Path, bool Parallel)
        {
            using (StreamReader r = new StreamReader(Path))
            {
                string line;
                while ((line = r.ReadLine()) != null)
                {
                    if (line.Contains(DMK_INP))
                    {
                        Blake2Bp512 dgt = new Blake2Bp512(Parallel);
                        byte[] input = new byte[0];
                        byte[] expect = new byte[dgt.DigestSize];
                        byte[] key = new byte[0];
                        byte[] hash = new byte[dgt.DigestSize];

                        int sze = DMK_INP.Length;
                        if (line.Length - sze > 0)
                            input = HexConverter.Decode(line.Substring(sze, line.Length - sze));

                        line = r.ReadLine();
                        sze = DMK_KEY.Length;
                        if (line.Length - sze > 0)
                            key = HexConverter.Decode(line.Substring(sze, line.Length - sze));

                        line = r.ReadLine();
                        sze = DMK_HSH.Length;
                        if (line.Length - sze > 0)
                            expect = HexConverter.Decode(line.Substring(sze, line.Length - sze));

                        dgt.LoadMacKey(new MacParams(key));
                        hash = dgt.ComputeHash(input);
                        dgt.Dispose();

                        if (Compare.IsEqual(hash, expect) == false)
                            throw new Exception("Blake2B: Expected hash is not equal!");
                    }
                }
            }
        }

        private void Blake2SpTest(string Path, bool Parallel)
        {
            using (StreamReader r = new StreamReader(Path))
            {
                string line;
                while ((line = r.ReadLine()) != null)
                {
                    if (line.Contains(DMK_INP))
                    {
                        Blake2Sp256 dgt = new Blake2Sp256(Parallel);
                        byte[] input = new byte[0];
                        byte[] expect = new byte[dgt.DigestSize];
                        byte[] key = new byte[0];
                        byte[] hash = new byte[dgt.DigestSize];

                        int sze = DMK_INP.Length;
                        if (line.Length - sze > 0)
                            input = HexConverter.Decode(line.Substring(sze, line.Length - sze));

                        line = r.ReadLine();
                        sze = DMK_KEY.Length;
                        if (line.Length - sze > 0)
                            key = HexConverter.Decode(line.Substring(sze, line.Length - sze));

                        line = r.ReadLine();
                        sze = DMK_HSH.Length;
                        if (line.Length - sze > 0)
                            expect = HexConverter.Decode(line.Substring(sze, line.Length - sze));

                        dgt.LoadMacKey(new MacParams(key));
                        hash = dgt.ComputeHash(input);
                        dgt.Dispose();

                        if (Compare.IsEqual(hash, expect) == false)
                            throw new Exception("Blake2S: Expected hash is not equal!");
                    }
                }
            }
        }
        #endregion
    }
}
