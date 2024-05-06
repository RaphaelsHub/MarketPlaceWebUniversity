using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebProject.BusinessLogic.Core.Levels.Product;
using WebProject.BusinessLogic.Core.Levels.GeneralResponse;
using WebProject.Domain.Entities.DBModels;
using WebProject.Domain;
using System.Data.Entity;

namespace WebProject.BusinessLogic.Core.Levels
{
    public class ProductAPI
    {
        //public ProductDataAPI() { }
        internal DataResponse<CategoryTypeEF> GetBaseCategory()
        {
            return GetCategoryByName("Base");
        }    

        internal DataResponse<CategoryTypeEF> GetCategoryByName(string name)
        {
            using (var db = new Context())
            {
                try
                {
                    var category = db.CategoryTypes.FirstOrDefault(p => p.CategoryName == name);

                    string resp = "";
                    if (category == null)
                    {
                        resp = $"There is no CategoryType with this name: {name}";
                    }

                    return new DataResponse<CategoryTypeEF>
                    {
                        Data = category,
                        IsExist = (category != null),
                        ResponseMessage = resp

                    };
                }
                catch (Exception ex)
                {
                    return new DataResponse<CategoryTypeEF>
                    {
                        Data = null,
                        IsExist = false,
                        ResponseMessage = "Unexpected error: " + ex.Message
                    };
                }
            }

        }

        internal DataResponse<CategoryTypeEF> GetCategoryById(int id)
        {
            using (var db = new Context())
            {
                try
                {
                    var category = db.CategoryTypes.FirstOrDefault(p => p.CategoryTypeId == id);

                    string resp = "";
                    if (category == null)
                    {
                        resp = $"There is no CategoryType with this id: {id}";
                    }

                    return new DataResponse<CategoryTypeEF>
                    {
                        Data = category,
                        IsExist = (category != null),
                        ResponseMessage = resp

                    };
                }
                catch (Exception ex)
                {
                    return new DataResponse<CategoryTypeEF>
                    {
                        Data = null,
                        IsExist = false,
                        ResponseMessage = "Unexpected error: " + ex.Message
                    };
                }
            }

        }

        internal DataResponse<ProductDataEF> GetSingleProductData(int id)
        {
            using (var db = new Context())
            {
                try
                {
                    var prod = db.Products.FirstOrDefault(p => p.Id == id);

                    string resp = "";
                    if (prod == null)
                    {
                        resp = $"There is no ProductData with this id: {id}";
                    }

                    return new DataResponse<ProductDataEF>
                    {
                        Data = prod,
                        IsExist = (prod != null),
                        ResponseMessage = resp

                    };
                }
                catch (Exception ex)
                {
                    return new DataResponse<ProductDataEF>
                    {
                        Data = null,
                        IsExist = false,
                        ResponseMessage = "Unexpected error: " + ex.Message
                    };
                }
            }

        }

        internal DataResponse<List<ProductDataEF>> GetProductsByCategory(CategoryTypeEF category)
        {
            using (var dbContext = new Context())
            {
                try
                {
                    ProductDataEF a = new ProductDataEF();
                    var ProductsWithPrice = dbContext.Products
                    .Where(p => p.Category.CategoryName == category.CategoryName)
                    .ToList();

                    string resp = "";
                    if (ProductsWithPrice != null && !ProductsWithPrice.Any())
                    {
                        resp = $"There is no ProductData with category: {category.CategoryName}";
                    }

                    return new DataResponse<List<ProductDataEF>>
                    {
                        Data = ProductsWithPrice,
                        IsExist = (ProductsWithPrice != null && ProductsWithPrice.Any()),
                        ResponseMessage = resp
                    };
                }
                catch (Exception ex)
                {
                    return new DataResponse<List<ProductDataEF>>
                    {
                        Data = null,
                        IsExist = false,
                        ResponseMessage = "Unexpected error: " + ex.Message
                    };
                }
            }
        }

        internal DataResponse<List<ProductDataEF>> GetProductsByPrice(PriceRange priceRange)
        {
            using (var dbContext = new Context())
            {
                try
                {
                    var ProductsWithPrice = dbContext.Products
                        .Where(p => p.Price >= priceRange.Min && p.Price <= priceRange.Max)
                        .ToList();

                    string resp = "";
                    if (ProductsWithPrice != null && !ProductsWithPrice.Any())
                    {
                        resp = $"There is no ProductData with price from: {priceRange.Min} to {priceRange.Max}";
                    }

                    return new DataResponse<List<ProductDataEF>>
                    {
                        Data = ProductsWithPrice,
                        IsExist = (ProductsWithPrice != null && ProductsWithPrice.Any()),
                        ResponseMessage = resp
                    };
                }
                catch (Exception ex)
                {
                    return new DataResponse<List<ProductDataEF>>
                    {
                        Data = null,
                        IsExist = false,
                        ResponseMessage = "Unexpected error: " + ex.Message
                    };
                }
            }
        }

        internal DataResponse<List<ProductDataEF>> GetProductsByName(string subString)
        {
            using (var dbContext = new Context())
            {
                try
                {
                    var ProductsWithName = dbContext.Products
                        .Where(p => p.Name.Contains(subString))
                        .ToList();

                    string resp = "";
                    if (ProductsWithName != null && !ProductsWithName.Any())
                    {
                        resp = $"There is no ProductData with sub word: {subString}";
                    }

                    return new DataResponse<List<ProductDataEF>>
                    {
                        Data = ProductsWithName,
                        IsExist = (ProductsWithName != null && ProductsWithName.Any()),
                        ResponseMessage = resp
                    };
                }
                catch (Exception ex)
                {
                    return new DataResponse<List<ProductDataEF>>
                    {
                        Data = null,
                        IsExist = false,
                        ResponseMessage = "Unexpected error: " + ex.Message
                    };
                }
            }
        }

        internal StandartResponse DeleteProductDataByID(int id)
        {
            var prodResponse = GetSingleProductData(id);
            if (prodResponse.IsExist == false)
            {
                return new StandartResponse
                {
                    Status = false,
                    ResponseMessage = prodResponse.ResponseMessage
                };
            }
            var ProductDataToDelete = prodResponse.Data;
            using (var db = new Context())
            {
                try
                {
                    db.Entry(ProductDataToDelete).State = EntityState.Deleted; // Setting an object state for delete
                    db.SaveChanges();

                    return new StandartResponse
                    {
                        Status = true,
                        ResponseMessage = ""
                    };
                }
                catch (Exception ex)
                {
                    return new StandartResponse
                    {
                        Status = false,
                        ResponseMessage = "Unexpected error: " + ex.Message
                    };
                }
            }
        }

        internal StandartResponse ModifyProductData(ProductDataEF updatedProductData)
        {
            using (var db = new Context())
            {
                try
                {
                    db.Entry(updatedProductData).State = EntityState.Modified; // Setting an object state for Modify (Update)
                    var entry = db.Entry(updatedProductData);

                    if (entry.State == EntityState.Modified)
                    {
                        db.SaveChanges();
                        return new StandartResponse
                        {
                            Status = true,
                            ResponseMessage = ""
                        };
                    }
                    else
                    {
                        return new StandartResponse
                        {
                            Status = false,
                            ResponseMessage = $"There is no ProductData with this id: {updatedProductData.Id}"
                        };
                    }
                }
                catch (Exception ex)
                {
                    return new StandartResponse
                    {
                        Status = false,
                        ResponseMessage = "Unexpected error: " + ex.Message
                    };
                }
            }
        }

        internal DataResponse<List<ProductDataEF>> GetProductsOnPages(PageInfo currentPage)
        {
            uint ProductsPerPage = currentPage.ProductsPerPage;
            uint pageIndex = currentPage.PageIndex;

            uint startIndex = pageIndex * ProductsPerPage; //index of first element

            using (var db = new Context())
            {
                try
                {
                    var ProductsOnPage = db.Products
                                .OrderBy(p => p.Id)     //order ProductData by ID
                                .Skip((int)startIndex)
                                .Take((int)ProductsPerPage)
                                .ToList();
                    return new DataResponse<List<ProductDataEF>>
                    {
                        Data = ProductsOnPage,
                        IsExist = true,
                        ResponseMessage = ""
                    };
                }
                catch (Exception ex)
                {
                    return new DataResponse<List<ProductDataEF>>
                    {
                        Data = null,
                        IsExist = false,
                        ResponseMessage = "Unexpected error: " + ex.Message
                    };
                }
            }
        }

        internal DataResponse<List<ProductDataEF>> GetAllProducts()
        {
            var BigPage = new PageInfo
            {
                PageIndex = 0,
                ProductsPerPage = uint.MaxValue
            };
            return GetProductsOnPages(BigPage);
        }

        internal StandartResponse AddProductDataToDB(ProductDataEF prod)
        {
            if (!IsValidProductData(prod)) //checking that all fields are not null and the validity of Price and Amount
            {
                return new StandartResponse
                {
                    Status = false,
                    ResponseMessage = "Incorrect ProductData data"
                };
            }

            using (var db = new Context())
            {
                try
                {
                    var sameProductData = db.Products.FirstOrDefault(p => p.Name == prod.Name);
                    if (sameProductData != null)
                    {
                        return new StandartResponse
                        {
                            Status = false,
                            ResponseMessage = "Same ProductData already exist"
                        };
                    }

                    db.Products.Add(prod);
                    db.SaveChanges();
                    return new StandartResponse
                    {
                        Status = true,
                        ResponseMessage = ""
                    };
                }
                catch (Exception ex)
                {
                    return new StandartResponse
                    {
                        Status = false,
                        ResponseMessage = "Unexpected error: " + ex.Message
                    };
                }
            }
        }

        private bool IsValidProductData(ProductDataEF prod)
        {
            return (
                prod.Name != null &&
                prod.Category != null &&
                prod.Details != null &&
                prod.ShortDescription != null &&
                prod.Price > 0
                //prod.Amount >= 0
                );
        }
        private string DetailsInvalidProductDataPesponse(ProductDataEF prod)
        {
            return
                (
                "\nprod.Name != null     : " + (prod.Name != null).ToString() +
                "\nprod.Category != null : " + (prod.Category != null).ToString() +
                "\nprod.Details != null  : " + (prod.Details != null).ToString() +
                "\nprod.ShortDescription != null : " + (prod.ShortDescription != null).ToString() +
                "\nprod.Price > 0        : " + (prod.Price > 0).ToString()
                //"\nprod.Amount >= 0      : " + (prod.Amount >= 0).ToString()
                );
        }
    }
}
