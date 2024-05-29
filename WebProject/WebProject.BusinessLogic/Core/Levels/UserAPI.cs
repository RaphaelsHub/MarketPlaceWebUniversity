using System;
using System.Collections.Generic;
using System.Linq;
using WebProject.BusinessLogic.Core.Levels.GeneralResponse;
using WebProject.Domain.Entities.DBModels;
using WebProject.Domain.Entities.User;
using WebProject.Domain;
using WebProject.ModelAccessLayer.Model;

namespace WebProject.BusinessLogic.Core
{
    public class UserAPI
    {
        internal DataResponse<UserEF> ResetUser(UserEF userData)
        {
            using (var db = new Context())
            {
                // Получаем пользователя из базы данных по его Id
                UserEF userFromDb = db.Users.FirstOrDefault(u => u.Id == userData.Id);
                if (userFromDb != null)
                {
                    // Обновляем данные пользователя в объекте UserData
                    userFromDb = userData;
                    db.SaveChanges();

                    // Возвращаем обновленный объект UserData
                    return new DataResponse<UserEF> { Data = userFromDb, IsExist = true };

                }
                else
                {
                    // Если пользователя с указанным Id не найдено в базе данных,
                    // возможно, стоит сгенерировать исключение или выполнить другое действие по обработке этой ситуации
                    return new DataResponse<UserEF> { Data = userFromDb, IsExist = false, ResponseMessage = "User not found in database." };
                }
            }
        }

        internal DataResponse<UserEF> FindUserEF(int indexUser)
        {
            using (var db = new Context())
            {
                var userDB = db.Users.FirstOrDefault(u => u.Id == indexUser);

                if (userDB != null)
                {
                    return new DataResponse<UserEF> { Data = userDB, IsExist = true};
                }
                else
                {
                    return new DataResponse<UserEF> { Data = null, IsExist = false, ResponseMessage = "This User with userId dont exist" };
                }
            }
        }
        internal DataResponse<CartItemEF> FindCartItemEF(int idProduct, int indexUser)
        {
            using (var db = new Context())
            {
                var userDB = db.Users.FirstOrDefault(u => u.Id == indexUser);

                if (userDB != null)
                {
                    var cartItemDB = userDB.CartItems.FirstOrDefault(i => i.ProductId == idProduct);
                    if(cartItemDB != null)
                    {
                        return new DataResponse<CartItemEF> { Data = cartItemDB, IsExist = true};
                    }
                    else
                    {
                        return new DataResponse<CartItemEF> { Data = null, IsExist = false, ResponseMessage = "This CartItemEF dont exist" };
                    }
                }
                else
                {
                    return new DataResponse<CartItemEF> { Data = null, IsExist = false, ResponseMessage = "This User with indexUser dont exist" };
                }

            }
        }

        private CartItemEF CretateCartItemEF(CartItemEF cartItem)
        {
            using (var db = new Context())
            {
                var newCartItem = db.CartItems.Add(cartItem);
                db.SaveChanges();
                return newCartItem;
            }
        }

        internal StandartResponse AddToUserCart(CartItemEF cartItem)
        {
            var userResponse = FindUserEF(cartItem.User.Id);
            if (userResponse.IsExist == false)
                return new StandartResponse { Status = false, ResponseMessage = "User not found in database." };

            if (cartItem == null)
                return new StandartResponse { Status = false, ResponseMessage = "cartItem is null" };

            var newCartItem = CretateCartItemEF(cartItem);

            using (var db = new Context())
            {
                // Обновляем данные пользователя в объекте UserData
                var theNewestCartItem = db.CartItems.FirstOrDefault(x => x.CartItemId == newCartItem.CartItemId);
                var theNewestUser = db.Users.FirstOrDefault(x => x.Id == userResponse.Data.Id);

                theNewestUser.CartItems.Add(theNewestCartItem);
                db.SaveChanges();

                // Возвращаем обновленный объект UserData
                return new StandartResponse { Status = true };
            }
        }
        internal StandartResponse DeleteFromUserCart(CartItemEF cartItem)
        {
            using (var db = new Context())
            {
                var userData = cartItem.User;
                // Получаем пользователя из базы данных по его Id
                UserEF userFromDb = db.Users.FirstOrDefault(u => u.Id == userData.Id);
                if (userFromDb != null)
                {
                    // Обновляем данные пользователя в объекте UserData
                    db.CartItems.Remove(cartItem);
                    db.SaveChanges();

                    // Возвращаем обновленный объект UserData
                    return new StandartResponse { Status = true };

                }
                else
                {
                    // Если пользователя с указанным Id не найдено в базе данных,
                    // возможно, стоит сгенерировать исключение или выполнить другое действие по обработке этой ситуации
                    return new StandartResponse { Status = false, ResponseMessage = "User not found in database." };
                }
            }
        }

        internal AdminEF GetSuperAdmin()
        {
            using (var db = new Context())
            {
                return db.Admins.FirstOrDefault();
            }
        }

        internal StandartResponse SetSuperAdmin(UserEF admin)
        {
            using (var db = new Context())
            {
                var superAdmin = db.Admins.FirstOrDefault();
                if (superAdmin == null)
                {
                    var newSuperAdmin = new AdminEF
                    {
                        UserId = admin.Id
                    };
                    db.Admins.Add(newSuperAdmin);
                    db.SaveChanges();
                    return new StandartResponse { Status = true };
                }
                return new StandartResponse { Status = false, ResponseMessage = "SetSuperAdmin already exist" };
            }
        }

        internal StandartResponse UpdateOrderInfo(OrderInfoReqest updated)
        {
            using (var db = new Context())
            {
                OrderEF findOrder = db.Orders.FirstOrDefault(or => or.Order_Id == updated.OrderId);
                if (findOrder != null)
                {
                    findOrder.Name = updated.Name;
                    findOrder.Email = updated.Email;
                    findOrder.Phone = updated.Phone;
                    findOrder.Country = updated.Country;
                    findOrder.City = updated.City;
                    findOrder.Address = updated.Address;
                    findOrder.Comment = updated.Comment;
                    findOrder.StatusDelivery = updated.StatusDelivery;
                    db.SaveChanges();
                    return new StandartResponse { Status = true };
                }
                else
                {
                    return new StandartResponse { Status = false, ResponseMessage = "Cant find order with id = updated.OrderId" };
                }
            }
        }
        internal DataResponse<OrderEF> ProcessUserOrder(int indexUser, OrderInfoReqest orderInfo)
        {
            var superAdmin = GetSuperAdmin();
            using (var db = new Context())
            {
                // Получаем пользователя из базы данных по его Id
                UserEF userFromDb = db.Users.FirstOrDefault(u => u.Id == indexUser);

                if (userFromDb != null)
                {
                    if (userFromDb.CartItems.Count() <= 0)
                    {
                        return new DataResponse<OrderEF> { Data = null, IsExist = false, ResponseMessage = "User dont have any item in cart" };
                    }

                    OrderEF newOrder = new OrderEF { User = userFromDb, Admin = superAdmin, OrderDate = DateTime.Now, CartItems = userFromDb.CartItems };
                    // Обновляем данные пользователя в объекте UserData
                    userFromDb.CartItems.Clear();

                    newOrder.Name = orderInfo.Name;
                    newOrder.Email = orderInfo.Email;
                    newOrder.Phone = orderInfo.Phone;
                    newOrder.Country = orderInfo.Country;
                    newOrder.City = orderInfo.City;
                    newOrder.Address = orderInfo.Address;
                    newOrder.Comment = orderInfo.Comment;
                    newOrder.StatusDelivery = orderInfo.StatusDelivery;

                    userFromDb.Orders.Add(newOrder);
                    superAdmin.Orders.Add(newOrder);

                    db.SaveChanges();

                    return new DataResponse<OrderEF> { Data = newOrder, IsExist = true };

                }
                else
                {
                    // Если пользователя с указанным Id не найдено в базе данных,
                    // возможно, стоит сгенерировать исключение или выполнить другое действие по обработке этой ситуации
                    return new DataResponse<OrderEF> { Data = null, IsExist = false, ResponseMessage = "User not found in database." };
                }
            }
        }
        internal DataResponse<List<CartItemEF>> ViewUserCart(int indexUser)
        {
            using (var db = new Context())
            {
                // Получаем пользователя из базы данных по его Id
                UserEF userFromDb = db.Users.FirstOrDefault(u => u.Id == indexUser);
                if (userFromDb != null)
                {
                    if (userFromDb.CartItems.Count() <= 0)
                    {
                        return new DataResponse<List<CartItemEF>> { Data = null, IsExist = false, ResponseMessage = "User dont have any item in cart" };
                    }
                    // Обновляем данные пользователя в объекте UserData
                    List<CartItemEF> userCartItems = new List<CartItemEF>(userFromDb.CartItems);

                    // Возвращаем обновленный объект UserData
                    return new DataResponse<List<CartItemEF>> { Data = userCartItems, IsExist = true };

                }
                else
                {
                    // Если пользователя с указанным Id не найдено в базе данных,
                    // возможно, стоит сгенерировать исключение или выполнить другое действие по обработке этой ситуации
                    return new DataResponse<List<CartItemEF>> { Data = null, IsExist = false, ResponseMessage = "User not found in database." };
                }
            }
        }
        internal DataResponse<List<OrderEF>> ViewUserOrders(int indexUser)
        {
            using (var db = new Context())
            {
                // Получаем пользователя из базы данных по его Id
                UserEF userFromDb = db.Users.FirstOrDefault(u => u.Id == indexUser);
                if (userFromDb != null)
                {
                    if (userFromDb.CartItems.Count() <= 0)
                    {
                        return new DataResponse<List<OrderEF>> { Data = null, IsExist = false, ResponseMessage = "User dont have any item in cart" };
                    }
                    // Обновляем данные пользователя в объекте UserData
                    List<OrderEF> userCartItems = new List<OrderEF>(userFromDb.Orders);

                    // Возвращаем обновленный объект UserData
                    return new DataResponse<List<OrderEF>> { Data = userCartItems, IsExist = true };

                }
                else
                {
                    // Если пользователя с указанным Id не найдено в базе данных,
                    // возможно, стоит сгенерировать исключение или выполнить другое действие по обработке этой ситуации
                    return new DataResponse<List<OrderEF>> { Data = null, IsExist = false, ResponseMessage = "User not found in database." };
                }
            }
        }

        internal StandartResponse DeleteFromUserOrder(OrderEF userOrder)
        {
            using (var db = new Context())
            {
                var userData = userOrder.User;
                // Получаем пользователя из базы данных по его Id
                UserEF userFromDb = db.Users.FirstOrDefault(u => u.Id == userData.Id);
                if (userFromDb != null)
                {
                    // Обновляем данные пользователя в объекте UserData
                    db.Orders.Remove(userOrder);
                    db.SaveChanges();

                    // Возвращаем обновленный объект UserData
                    return new StandartResponse { Status = true };

                }
                else
                {
                    // Если пользователя с указанным Id не найдено в базе данных,
                    // возможно, стоит сгенерировать исключение или выполнить другое действие по обработке этой ситуации
                    return new StandartResponse { Status = false, ResponseMessage = "User not found in database." };
                }
            }
        }

        internal StandartResponse SuperAdminEditProductData(ProductDataEF updatedProductData)
        {
            using (var db = new Context())
            {
                var existingProductData = db.Products.FirstOrDefault(p => p.Id == updatedProductData.Id);
                if (existingProductData != null)
                {
                    db.Entry(existingProductData).CurrentValues.SetValues(updatedProductData);
                    db.SaveChanges();
                    return new StandartResponse { Status = true };
                }

                else
                {
                    return new StandartResponse { Status = false, ResponseMessage = "ProductData not found in database." };
                }
            }
        }
        internal DataResponse<List<ProductDataEF>> SuperAdminGetAllProducts()
        {
            using (var db = new Context())
            {
                if (db.Products.Any())
                    return new DataResponse<List<ProductDataEF>> { Data = db.Products.ToList(), IsExist = true };

                else
                    return new DataResponse<List<ProductDataEF>> { Data = new List<ProductDataEF>(), IsExist = false, ResponseMessage = "There are no Products in the database" };
            }
        }
        internal StandartResponse SuperAdminDeleteOrderModel(int idOrder)
        {
            using (var db = new Context())
            {
                // Получаем Order из базы данных по его Id
                var deletedOrder = db.Orders.FirstOrDefault(or => or.Order_Id == idOrder);
                if (deletedOrder != null)
                {
                    // Обновляем данные пользователя в объекте UserData
                    db.Orders.Remove(deletedOrder);
                    db.SaveChanges();

                    // Возвращаем обновленный объект UserData
                    return new StandartResponse { Status = true };

                }
                else
                {
                    // Если пользователя с указанным Id не найдено в базе данных,
                    // возможно, стоит сгенерировать исключение или выполнить другое действие по обработке этой ситуации
                    return new StandartResponse { Status = false, ResponseMessage = "Order not found in database." };
                }
            }
        }
        public DataResponse<ProductDataEF> SuperAdminDeleteProductData(int idProductData)
        {
            using (var db = new Context())
            {
                //не проверяю принадлежность к админу потому что SuperAdmin
                var deletedProductData = db.Products.FirstOrDefault(p => p.Id == idProductData);

                if (deletedProductData != null)
                {
                    DeleteAllProductDataReferenseInCart(idProductData, db);
                    if (!FindAnyProductDataReferenseInOrders(idProductData, db))
                    {
                        db.Products.Remove(deletedProductData);
                        db.SaveChanges();
                        return new DataResponse<ProductDataEF>
                        {
                            Data = null,
                            IsExist = false,
                            ResponseMessage =
                            "This ProductData still full deleted, it dont exist any more in dataBase"
                        };
                    }
                    else
                    {
                        deletedProductData.Amount = 0;
                        return new DataResponse<ProductDataEF>
                        {
                            Data = deletedProductData,
                            IsExist = true,
                            ResponseMessage =
                            "This ProductData stil exist and have referense in Orders, but all UserCart referense was removed"
                        };
                    }
                }
                else
                {
                    return new DataResponse<ProductDataEF> { Data = null, IsExist = false, ResponseMessage = "This ProductData dont exist" };
                }

            }
        }

        private bool FindAnyProductDataReferenseInOrders(int idProductData, Context db)
        {
            return db.Orders.Any(u => u.CartItems.Any
            (ci => ci.ProductId == idProductData));
        }

        private void DeleteAllProductDataReferenseInCart(int idProductData, Context db)
        {
            var userIdsWithProductData = db.Users
                .Where(u => u.CartItems.Any(ci => ci.ProductId == idProductData))
                .Select(u => u.Id)
                .ToList(); //находит всех пользователй у которых есть искомый продукт именно в корзине

            foreach (var userId in userIdsWithProductData)
            {
                var cartItemsWithProductData = db.CartItems
                    .Where(ci => ci.UserId == userId && ci.ProductId == idProductData)
                    .ToList();

                db.CartItems.RemoveRange(cartItemsWithProductData);
                //удаляет искомый продукт у именно из корзин пользователей
            }

            db.SaveChanges();

        }

    }
}
