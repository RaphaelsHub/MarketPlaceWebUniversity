using System;
using System.Collections.Generic;
using WebProject.BusinessLogic.Core;
using WebProject.BusinessLogic.Core.Levels;
using WebProject.BusinessLogic.Core.Levels.GeneralResponse;
using WebProject.BusinessLogic.Interfaces;
using WebProject.Domain.DB;
using WebProject.ModelAccessLayer.Model;

namespace WebProject.BusinessLogic.MainBL
{
    public class GuestBL : UserBaseBL, IGuest
    {
        private readonly UserGuestAPI _guestAPI = new UserGuestAPI();

        public DataResponse<UserData> Login(LoginData loginData)
        {
            var responseResult = _guestAPI.CheckLogin(loginData);

            return !responseResult.IsExist ? new DataResponse<UserData>
            {
                Data = null,
                IsExist = false,
                ResponseMessage = responseResult.ResponseMessage
            }
            : new DataResponse<UserData>
            {
                Data = ModelGeneratingClass.GenerateUserData(responseResult.Data, GetProductById),
                IsExist = true,
                ResponseMessage = responseResult.ResponseMessage
            };
        }



        public DataResponse<UserData> Register(RegistrationData registrationData)
        {
            var responseResult = _guestAPI.RegistrateUser(registrationData);

            return !responseResult.IsExist ? new DataResponse<UserData>
            {
                Data = null,
                IsExist = false,
                ResponseMessage = responseResult.ResponseMessage
            }
            : new DataResponse<UserData>
            {
                Data = ModelGeneratingClass.GenerateUserData(responseResult.Data, GetProductById),
                IsExist = true,
                ResponseMessage = responseResult.ResponseMessage
            };
        }
    }
}
