using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebProject.BusinessLogic.Core.Levels.GeneralResponse;
using WebProject.Domain.Entities.User;
using WebProject.ModelAccessLayer.Model;
using WebProject.Domain;

namespace WebProject.BusinessLogic.Core.Levels
{
    public class GuestAPI
    {
        internal ResponseUserStatus ADNUSessionCheck(RegistrationData data)
        {
            using (var db = new Context())
            {
                //конект бдшки и проверка добавления юзера через неё, также здесь проверка существования электронной почты
                bool userExists = db.Users.Any(u => u.Name == data.Name);
                bool emailExists = db.Users.Any(u => u.Email == data.Email);
                int newUserId = 1; // Устанавливаем начальное значение ID
                //var ip = Request.UserHostAddress;
                if (db.Users.Any())
                {
                    // Если в таблице есть пользователи, находим максимальный ID и увеличиваем его на 1
                    newUserId = db.Users.Max(u => u.Id) + 1;
                }
                if (!userExists && !emailExists)
                {
                    //jwt token
                    //добавление данных юзера в бдшку 
                    UserEF newUser = new UserEF
                    {
                        Name = data.Name,
                        Email = data.Email,
                        Password = data.Password,
                        Id = newUserId, //Максимальный айди для существующего пользователя + 1 (пока что так) 
                        RegTime = DateTime.Now,
                        LogTime = DateTime.Now
                    };

                    // Добавляем пользователя в DbSet<User> и сохраняем изменения в базу данных
                    db.Users.Add(newUser);
                    db.SaveChanges();

                    return new ResponseUserStatus
                    {
                        Status = true,
                        IsAdmin = false,
                        ResponseMessage = "User added successfully.",
                        ID = newUserId
                    };
                }
                else
                {
                    return new ResponseUserStatus
                    {
                        Status = false,
                        IsAdmin = false,
                        ResponseMessage = "User/Email already exists."
                    };
                }
            }
        }
        internal ResponseUserStatus ULASessionCheck(LoginData data)
        {
            using (var db = new Context())
            {
                UserEF user = db.Users.FirstOrDefault(u => u.Email == data.Email && u.Password == data.Password);

                if (user != null)
                {
                    // Пользователь найден
                    user.LogTime = DateTime.Now;
                    db.SaveChanges();
                    return new ResponseUserStatus
                    {
                        Status = true,
                        IsAdmin = false,
                        ResponseMessage = "Regular User",
                        ID = user.Id
                    };
                }
                else
                {
                    // Пользователь с указанным электронным адресом не найден или неправильный пароль
                    return new ResponseUserStatus
                    {
                        Status = false,
                        IsAdmin = false,
                        ResponseMessage = "Login/Password is invalid"
                    };
                }
            }
        }

        internal DataResponse<UserEF> CheckLogin(LoginData data)
        {
            using (var db = new Context())
            {
                // Получаем пользователя из базы данных по его Id
                UserEF userFromDb = db.Users.FirstOrDefault(u => u.Password == data.Password && u.Email == data.Email);
                if (userFromDb != null)
                {
                    userFromDb.LogTime = DateTime.Now;
                    db.SaveChanges();
                    return new DataResponse<UserEF> { Data = userFromDb, IsExist = true };
                }
                else
                {
                    // Если пользователя с указанным Id не найдено в базе данных,
                    // возможно, стоит сгенерировать исключение или выполнить другое действие по обработке этой ситуации
                    return new DataResponse<UserEF> { Data = null, IsExist = false, ResponseMessage = "Wrong Email Or Password" };
                }
            }
        }

        internal DataResponse<UserEF> RegistrateUser(RegistrationData data)
        {
            using (var db = new Context())
            {
                // Получаем пользователя из базы данных 
                UserEF userFromDb = db.Users.FirstOrDefault(u => u.Email == data.Email || u.Name == data.Name || u.PhoneNumber == data.Phone);
                if (userFromDb != null)
                {
                    if (userFromDb.Name == data.Name)
                        return new DataResponse<UserEF> { Data = null, IsExist = true, ResponseMessage = "Already exist user with same Name" };

                    if (userFromDb.Email == data.Email)
                        return new DataResponse<UserEF> { Data = null, IsExist = true, ResponseMessage = "Already exist user with same Email" };

                    if (userFromDb.PhoneNumber == data.Phone)
                        return new DataResponse<UserEF> { Data = null, IsExist = true, ResponseMessage = "Already exist user with same Phone Number" };

                    else //если все проверки не прошли, но такой юзер всё таки нашёлся
                        return new DataResponse<UserEF> { Data = null, IsExist = true, ResponseMessage = "Unexpected Error" };
                }
                else
                {
                    // Если пользователя с data не найдено в базе данных
                    var NewUser = new UserEF { RegTime = DateTime.Now, Password = data.Password };
                    NewUser.Name = data.Name;
                    NewUser.Email = data.Email;
                    NewUser.PhoneNumber = data.Phone;
                    db.Users.Add(NewUser);
                    db.SaveChanges();
                    return new DataResponse<UserEF> { Data = null, IsExist = false, ResponseMessage = "User created" };
                }
            }
        }
        //Возможно еще методы
    }
}
