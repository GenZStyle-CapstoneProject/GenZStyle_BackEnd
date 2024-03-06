using AutoMapper;
using GenZStyleApp.DAL.Models;
using GenZStyleAPP.BAL.DTOs.Collections;
using GenZStyleAPP.BAL.Helpers;
using GenZStyleAPP.BAL.Repository.Interfaces;
using Microsoft.AspNetCore.Http;
using ProjectParticipantManagement.DAL.Infrastructures;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GenZStyleAPP.BAL.Repository.Implementations
{
    public class CollectionRepository : ICollectionRepository
    {
        private UnitOfWork _unitOfWork;
        private IMapper _mapper;
        public CollectionRepository(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = (UnitOfWork)unitOfWork;
            _mapper = mapper;
        }

        // Inside PostRepository class

        public async Task<GetCollectionResponse> SavePostToCollection(int postId, HttpContext httpContext)
        {
            try
            {
                JwtSecurityToken jwtSecurityToken = TokenHelper.ReadToken(httpContext);
                string emailFromClaim = jwtSecurityToken.Claims.FirstOrDefault(x => x.Type == JwtRegisteredClaimNames.Email).Value;
                var accountStaff = await _unitOfWork.AccountDAO.GetAccountByEmail(emailFromClaim);

                // Retrieve the post by its ID
                Post post = await _unitOfWork.PostDAO.GetPostByIdAsync(postId);

                if (post == null)
                {
                    // Handle case where post does not exist
                    return null; // Return null or throw an exception as needed
                }

                // Create a new Collection object
                Collection collection = new Collection
                {
                    
                    AccountId = accountStaff.AccountId, // Use account ID from the retrieved account
                    CategoryId = 1,
                    Name = post.Content, // Use post content as collection name
                    Image_url = post.Image, // Use post image URL as collection image URL
                    Type = 1 // Example value for collection type, adjust as needed
                };

                // Add the Collection object using the CollectionDAO
                await _unitOfWork.CollectionDAO.AddNewCollection(collection);
                await _unitOfWork.CommitAsync();
                // Tạo một đối tượng DTO để đại diện cho dữ liệu của Collection
                GetCollectionResponse collectionResponse = new GetCollectionResponse
                {
                    Id = collection.Id,
                    AccountId = collection.AccountId,
                    CategoryId = collection.CategoryId,
                    Name = collection.Name,
                    Image_url = collection.Image_url,
                    Type = collection.Type
                };

                // Trả về đối tượng DTO đại diện cho dữ liệu của Collection
                return collectionResponse;
            }
            catch (Exception ex)
            {
                // Handle exceptions
                throw new Exception(ex.Message); // or return null, depending on your error handling strategy
            }
        }


    }
}
