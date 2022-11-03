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
        public UserController() { uDao = new UserDao(); }
        public UserModel ContSelectUserByLogin(UserModel currentUserModel) 
        {
            UserModel hashUserModel = new UserModel(currentUserModel.UserName, HashPassword(currentUserModel.Password));
            return uDao.SelectUserByLogin(hashUserModel); 
        }
        public UserModel AddReturnNewUser(UserModel newUserModel)
        {
            Guid id = Guid.NewGuid();
            UserModel hashUserModel = new UserModel(newUserModel.UserName, HashPassword(newUserModel.Password), id.ToString());

            try
            {
                //THERE MUST BE A BETTER WAY THE NULLS
                if(uDao.InsertNewUserModel(hashUserModel) == 1) { return null; }
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
            //this sure is a piece of shit but i don't know how to fix it,
            SHA512 sHA512 = SHA512.Create();
            sHA512.ComputeHash(Encoding.UTF8.GetBytes(password));
            return BitConverter.ToString(sHA512.Hash).Replace("-", "");
        }
    }
}
