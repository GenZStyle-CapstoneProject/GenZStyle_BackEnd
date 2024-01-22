using AutoMapper;
using GenZStyleAPP.BAL.DTOs.FireBase;
using GenZStyleAPP.BAL.DTOs.Posts;
using GenZStyleApp.BAL.Helpers;
using GenZStyleAPP.BAL.Helpers;
using GenZStyleApp.DAL.Models;
using GenZStyleAPP.BAL.Repository.Interfaces;
using Microsoft.AspNetCore.Http;
using ProjectParticipantManagement.BAL.Heplers;
using ProjectParticipantManagement.DAL.Infrastructures;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GenZStyleAPP.BAL.DTOs.Products;

namespace GenZStyleAPP.BAL.Repository.Implementations
{
    public class ProductRepository: IProductRepository
    {
        private UnitOfWork _unitOfWork;
        private IMapper _mapper;
        public ProductRepository(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = (UnitOfWork)unitOfWork;
            _mapper = mapper;
        }

        #region CreateNewProductAsync
        public async Task<GetProductResponse> CreateNewProductAsync(AddProductRequest addProductRequest, FireBaseImage fireBaseImage, HttpContext httpContext)
        {
            try
            {
                JwtSecurityToken jwtSecurityToken = TokenHelper.ReadToken(httpContext);
                string emailFromClaim = jwtSecurityToken.Claims.FirstOrDefault(x => x.Type == JwtRegisteredClaimNames.Email).Value;
                var accountStaff = await _unitOfWork.AccountDAO.GetAccountByEmail(emailFromClaim);

                // Lấy hoặc thêm danh mục vào cơ sở dữ liệu
                var category = await _unitOfWork.CategoryDAO.GetPostByNameAsync(addProductRequest.CategoryName);
                if (category == null)
                {
                    category = new Category { CategoryName = addProductRequest.CategoryName };
                    await _unitOfWork.CategoryDAO.AddNewCategory(category);
                    await _unitOfWork.CommitAsync();
                }

                Product product = this._mapper.Map<Product>(addProductRequest);
                product.AccountId = accountStaff.AccountId;
                product.Category = category;
                product.Name = addProductRequest.Name;
                product.Color = addProductRequest.Color;
                product.Size = addProductRequest.Size;
                product.Gender = addProductRequest.Gender;
                product.Cost = addProductRequest.Cost;
                product.Account = accountStaff;

                #region Upload images to firebase

                //foreach (var imageFile in addPostRequest.Image)
                //{
                FileHelper.SetCredentials(fireBaseImage);
                FileStream fileStream = FileHelper.ConvertFormFileToStream(addProductRequest.Image);
                Tuple<string, string> result = await FileHelper.UploadImage(fileStream, "Product");

                // Assuming you want to store multiple image URLs
                // Consider creating a separate entity for images if necessary
                // and establish a one-to-many relationship with the Post entity
                // For now, appending URLs to the Image property
                product.Image = result.Item1; // Separate URLs by a delimiter
                                           //}

                #endregion
                await _unitOfWork.ProductDAO.AddNewProduct(product);
                await this._unitOfWork.CommitAsync();
                return this._mapper.Map<GetProductResponse>(product);

            }
            catch (Exception ex)
            {
                string error = ErrorHelper.GetErrorString(ex.Message);
                throw new Exception(error);
            }

        }
        #endregion
    }
}
