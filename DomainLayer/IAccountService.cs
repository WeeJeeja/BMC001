using DomainLayer.WrapperModels;
using System;
namespace DomainLayer
{
    public interface IAccountService
    {
        User GetUser(String email);
        bool CheckEmailIsUnique(String email);
        void AddUserAndAccount(User user, string password);

        bool Login(String email, String password);
    }
}
