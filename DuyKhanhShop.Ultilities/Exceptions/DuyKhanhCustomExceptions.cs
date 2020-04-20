using System;
using System.Collections.Generic;
using System.Text;

namespace DuyKhanhShop.Ultilities.Exceptions
{
    public class DuyKhanhCustomExceptions : Exception
    {
        public DuyKhanhCustomExceptions()
        {
        }

        public DuyKhanhCustomExceptions(string message)
            : base(message)
        {
        }

        public DuyKhanhCustomExceptions(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}
