using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quiz
{
    internal class UserController
    {
        private UserDao uDao;
        public UserController() { uDao = new(); }
        public UserModel ContSelectUserByLogin(UserModel currentUserModel) => uDao.SelectUserByLogin(currentUserModel);
        public UserModel AddReturnNewUser(UserModel newUserModel)
        {
            Guid id = Guid.NewGuid();
            newUserModel.UUID = id.ToString();
            try
            {
                uDao.InsertNewUserModel(newUserModel);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return null;
            }
            return newUserModel;
        }
    }
}
