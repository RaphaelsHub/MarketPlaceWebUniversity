using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebProject.BusinessLogic.Core.Levels.GeneralResponse;
using WebProject.Domain;
using WebProject.ModelAccessLayer.Model;
using WebProject.Domain.DB;
using WebProject.BusinessLogic.MainBL;

namespace WebProject.BusinessLogic.Core.Levels
{
    public class UserGuestAPI
    {
        internal DataResponse<UserDataEF> CheckLogin(LoginData data)
        {
            using (var db = new Context())
            {
                UserDataEF user = db.Users.FirstOrDefault(u => u.Password == data.Password && u.Email == data.Email);

                if (user == null)
                    return new DataResponse<UserDataEF> { ResponseMessage = "Email or password isnt correct!"};

                user.LogTime = DateTime.Now;
                db.SaveChanges();
                return new DataResponse<UserDataEF> { Data = user, IsExist = true, ResponseMessage = "Succesfully entered!" };
            }
        }

        internal DataResponse<UserDataEF> RegistrateUser(RegistrationData data)
        {
            using (var db = new Context())
            {
                UserDataEF user = db.Users.FirstOrDefault(u => u.Email == data.Email || u.Name == data.Name || u.PhoneNumber == data.Phone);

                if (user == null)
                {
                    var userEx = new UserDataEF
                    {
                        Name = data.Name,
                        Email = data.Email,
                        PhoneNumber = data.Phone,
                        Password = data.Password
                    };
                    db.Users.Add(userEx);
                    db.SaveChanges();

                    return new DataResponse<UserDataEF> { Data = userEx, IsExist = true, ResponseMessage = "User Created!" };
                }

                if (user.Name == data.Name)
                    return new DataResponse<UserDataEF> { Data = null, IsExist = true, ResponseMessage = "Already exist user with same Name!" };

                if (user.Email == data.Email)
                    return new DataResponse<UserDataEF> { Data = null, IsExist = true, ResponseMessage = "Already exist user with same Email!" };

                if (user.PhoneNumber == data.Phone)
                    return new DataResponse<UserDataEF> { Data = null, IsExist = true, ResponseMessage = "Already exist user with same Phone Number!" };

                return new DataResponse<UserDataEF> { Data = null, IsExist = true, ResponseMessage = "Unexpected Error" };
            }
        }
    }
}
