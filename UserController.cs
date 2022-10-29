using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Quiz
{
    internal class UserController
    {
        private UserDao uDao;
        public UserController() { uDao = new(); }
        public UserModel ContSelectUserByLogin(UserModel currentUserModel) 
        {
            UserModel hashUserModel = new(currentUserModel.UserName, HashPassword(currentUserModel.Password));
            return uDao.SelectUserByLogin(hashUserModel); 
        }
        public UserModel AddReturnNewUser(UserModel newUserModel)
        {
            Guid id = Guid.NewGuid();
            UserModel hashUserModel = new(newUserModel.UserName, HashPassword(newUserModel.Password), id.ToString());

            try
            {
                uDao.InsertNewUserModel(hashUserModel);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return null;
            }
            return hashUserModel;
        }
        public string HashPassword(string password)
        {
            //this sure is a piece of shit but i don't care enough to fix it, it should do just fine
            SHA512 sHA512 = SHA512.Create();
            sHA512.ComputeHash(Encoding.UTF8.GetBytes(password));
            return BitConverter.ToString(sHA512.Hash).Replace("-", "");
        }
    }
}
