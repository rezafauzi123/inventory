using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InventoryManagement.Shared.Abstractions.Encryption
{
    public interface IAES
    {
        string EncryptStringToBase64String(string value);
        string Decrypt(string value);
    }
}
