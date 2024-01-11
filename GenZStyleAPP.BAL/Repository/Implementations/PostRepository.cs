using AutoMapper;
using Firebase.Auth;
using GenZStyleApp.BAL.Helpers;
using GenZStyleApp.DAL.Models;
using GenZStyleAPP.BAL.DTOs.Accounts;
using GenZStyleAPP.BAL.DTOs.FireBase;
using GenZStyleAPP.BAL.DTOs.Posts;
using GenZStyleAPP.BAL.DTOs.Users;
using GenZStyleAPP.BAL.Heplers;
using GenZStyleAPP.BAL.Repository.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Hosting;
using ProjectParticipantManagement.BAL.Exceptions;
using ProjectParticipantManagement.BAL.Heplers;
using ProjectParticipantManagement.DAL.Infrastructures;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GenZStyleAPP.BAL.Repository.Implementations
{
    public class PostRepository : IPostRepository
    {
        private UnitOfWork _unitOfWork;
        private IMapper _mapper;
        public PostRepository(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = (UnitOfWork)unitOfWork;
            _mapper = mapper;
        }

        #region GetPosts
        public async Task<List<GetPostResponse>> GetPostsAsync(HttpContext httpContext)
        {
            try
            {
                JwtSecurityToken jwtSecurityToken = TokenHelper.ReadToken(httpContext);
                string emailFromClaim = jwtSecurityToken.Claims.FirstOrDefault(x => x.Type == JwtRegisteredClaimNames.Email).Value;
                var accountStaff = await _unitOfWork.AccountDAO.GetAccountByEmail(emailFromClaim);
                // Lấy tất cả bài post
                List<Post> allPosts = await _unitOfWork.PostDAO.GetPosts();

                // Lọc bài post để loại bỏ bài post của người dùng đăng nhập
                List<Post> filteredPosts = allPosts.Where(p => p.AccountId != accountStaff.AccountId).ToList();

                return this._mapper.Map<List<GetPostResponse>>(filteredPosts);
            }
            catch (Exception ex)
            {
                string error = ErrorHelper.GetErrorString(ex.Message);
                throw new Exception(error);
            }
        }
        #endregion

        public async Task<List<GetPostResponse>> GetActivePosts()
        {
            try
            {
                List<Post> posts = await this._unitOfWork.PostDAO.GetActivePosts();
                return this._mapper.Map<List<GetPostResponse>>(posts);
            }
            catch (Exception ex)
            {
                string error = ErrorHelper.GetErrorString(ex.Message);
                throw new Exception(error);
            }
        }

        #region GetPostDetailByIdAsync
        public async Task<GetPostResponse> GetPostDetailByIdAsync(int id)
        {
            try
            {
                var post = await _unitOfWork.PostDAO.GetPostByIdAsync(id);
                if (post == null)
                {
                    throw new NotFoundException("Product id does not exist in the system.");
                }
                var postDTO = _mapper.Map<GetPostResponse>(post);
                //Staff staff = await this._unitOfWork.StaffDAO.GetStaffDetailAsync(product.ModifiedBy);
                //productDTO.ModifiedStaff = staff.FullName;
                return postDTO;
            }
            catch (NotFoundException ex)
            {
                string error = ErrorHelper.GetErrorString("Post Id", ex.Message);
                throw new NotFoundException(error);
            }
            catch (Exception ex)
            {
                string error = ErrorHelper.GetErrorString(ex.Message);
                throw new Exception(error);
            }
        }
        #endregion

        #region GetPostByAccountIdAsync
        public async Task<GetPostResponse> GetPostByAccountIdAsync(int id)
        {
            try
            {
                var post = await _unitOfWork.PostDAO.GetPostByAccountIdAsync(id);
                if (post == null)
                {
                    throw new NotFoundException("Account id does not exist in the system.");
                }
                var postDTO = _mapper.Map<GetPostResponse>(post);
                //Staff staff = await this._unitOfWork.StaffDAO.GetStaffDetailAsync(product.ModifiedBy);
                //productDTO.ModifiedStaff = staff.FullName;
                return postDTO;
            }
            catch (NotFoundException ex)
            {
                string error = ErrorHelper.GetErrorString("Account Id", ex.Message);
                throw new NotFoundException(error);
            }
            catch (Exception ex)
            {
                string error = ErrorHelper.GetErrorString(ex.Message);
                throw new Exception(error);
            }
        }
        #endregion

        #region GetPostByGenderAsync
        public async Task<List<GetPostResponse>> GetPostByGenderAsync(bool gender)
        {
            try
            {
                List<Post> post = await _unitOfWork.PostDAO.GetPostByGenderAsync(gender);
                if (post == null)
                {
                    throw new NotFoundException("Gender does not exist in system");
                }

                return _mapper.Map<List<GetPostResponse>>(post);
            }
            catch (NotFoundException ex)
            {
                string error = ErrorHelper.GetErrorString(ex.Message);
                throw new Exception(error);
            }
            catch (Exception ex)
            {
                string error = ErrorHelper.GetErrorString(ex.Message);
                throw new Exception(error);
            }

        }

        #endregion

        #region CreateNewPostAsync
        public async Task<GetPostResponse> CreateNewPostAsync(AddPostRequest addPostRequest, FireBaseImage fireBaseImage, HttpContext httpContext)
        {
            try
            {
                //var images = new List<Post>();
                JwtSecurityToken jwtSecurityToken = TokenHelper.ReadToken(httpContext);
                string emailFromClaim = jwtSecurityToken.Claims.FirstOrDefault(x => x.Type == JwtRegisteredClaimNames.Email).Value;
                var accountStaff = await _unitOfWork.AccountDAO.GetAccountByEmail(emailFromClaim);

                Post post = this._mapper.Map<Post>(addPostRequest);

                post.AccountId = accountStaff.AccountId;
                post.Content = addPostRequest.Content;
                post.CreateTime = DateTime.Now;
                post.UpdateTime = DateTime.Now;
                post.Account = accountStaff;
                
                #region Upload images to firebase

                //foreach (var imageFile in addPostRequest.Image)
                //{
                    FileHelper.SetCredentials(fireBaseImage);
                    FileStream fileStream = FileHelper.ConvertFormFileToStream(addPostRequest.Image);
                    Tuple<string, string> result = await FileHelper.UploadImage(fileStream, "Post");

                    // Assuming you want to store multiple image URLs
                    // Consider creating a separate entity for images if necessary
                    // and establish a one-to-many relationship with the Post entity
                    // For now, appending URLs to the Image property
                    post.Image = result.Item1; // Separate URLs by a delimiter
                                               //}

                #endregion

                // Process hashtags
                foreach (string hashtagName in addPostRequest.Hashtags)
                {
                    List<Hashtag> existingHashtags = await _unitOfWork.HashTagDAO.SearchByHashTagName(hashtagName);

                    if (existingHashtags == null || existingHashtags.Count == 0)
                    {
                        Hashtag newHashtag = new Hashtag
                        {
                            Name = hashtagName,
                            // Set other properties as needed
                        };

                        existingHashtags = new List<Hashtag> { newHashtag };
                    }

                    if (post.HashPosts == null)
                    {
                        post.HashPosts = new List<HashPost>();
                    }

                    foreach (Hashtag existingHashtag in existingHashtags)
                    {
                        HashPost hashPost = new HashPost
                        {
                            Post = post,
                            Hashtag = existingHashtag,
                            CreateAt = DateTime.Now,
                            UpdateAt = DateTime.Now
                        };

                        post.HashPosts.Add(hashPost);
                    }
                }


                #endregion
                await _unitOfWork.PostDAO.AddNewPost(post);
                await this._unitOfWork.CommitAsync();
                return this._mapper.Map<GetPostResponse>(post);
                
            }
            catch (Exception ex)
            {
                string error = ErrorHelper.GetErrorString(ex.Message);
                throw new Exception(error);
            }

        }
        
        #region UpdatePostProfileByIdAsync
        public async Task<GetPostResponse> UpdatePostProfileByPostIdAsync(int postId,
                                                                                     FireBaseImage fireBaseImage,
                                                                                     UpdatePostRequest updatePostRequest)
        {
            try
            {
                Post post = await _unitOfWork.PostDAO.GetPostByIdAsync(postId);

                if (post == null)
                {
                    throw new NotFoundException("PostId does not exist in system");
                }

                post.Content = updatePostRequest.Content;
                post.CreateTime = updatePostRequest.CreateTime;
                post.UpdateTime = updatePostRequest.UpdateTime;
                
                //if (updateCustomerRequest.PasswordHash != null)
                //{
                //    customer.Account.PasswordHash = StringHelper.EncryptData(updateCustomerRequest.PasswordHash);
                //}

                #region Upload image to firebase
                if (updatePostRequest.Image != null)
                {
                    FileHelper.SetCredentials(fireBaseImage);
                    //await FileHelper.DeleteImageAsync(user.AvatarUrl, "User");
                    FileStream fileStream = FileHelper.ConvertFormFileToStream(updatePostRequest.Image);
                    Tuple<string, string> result = await FileHelper.UploadImage(fileStream, "Post");
                    post.Image = result.Item1;
                    //customer.AvatarID = result.Item2;
                }
                #endregion

                _unitOfWork.PostDAO.UpdatePost(post);
                await this._unitOfWork.CommitAsync();
                return _mapper.Map<GetPostResponse>(post);
            }

            catch (NotFoundException ex)
            {
                string error = ErrorHelper.GetErrorString("PostId", ex.Message);
                throw new NotFoundException(error);
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
