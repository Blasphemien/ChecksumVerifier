using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace Hash_Verifier {
    interface iHash {
        string CalculateHash(string data, string algorithm);
        string ConvertBytesToString(byte[] hashBytes);
        bool VerifyHash(string hashToVerify, List<TextBox> hashValues);
    }
}
