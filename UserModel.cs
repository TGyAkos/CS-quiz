using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quiz
{
    internal class UserModel
    {
        public string UUID { get; set; }
        public string UserName { get; }
        public string Password { get; }
        internal UserModel(string userName, string password)
        {
            UserName = userName;
            Password = password;
        }
        internal UserModel(string userName, string password, string uuid)
        {
            UUID = uuid;
            UserName = userName;
            Password = password;
        }
        public override string ToString()
        {
            return string.Format("UserModel: {0} {1} {2}", UUID, UserName, Password);
        }
    }
}
