using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebProject.ModelAccessLayer.Model;
using WebProject.Domain.Enum;
using WebProject.BusinessLogic.Core.Levels.GeneralResponse;

namespace WebProject.BusinessLogic.Interfaces
{
    public interface IGuest
    {
        DataResponse<UserData> Register(RegistrationData registrationData);
        DataResponse<UserData> Login(LoginData loginData);
    }
}
