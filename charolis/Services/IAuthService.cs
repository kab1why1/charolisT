using System;
using charolis.Models;

namespace charolis.Services;

public interface IAuthService
{
    void Register(RegUser user, string password);
    RegUser? ValidateUser(string email, string password);
}
