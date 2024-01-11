﻿using AutoMapper;
using GenZStyleApp.DAL.Models;
using GenZStyleAPP.BAL.DTOs.FireBase;
using GenZStyleAPP.BAL.DTOs.HashTag;
using GenZStyleAPP.BAL.DTOs.Users;
using GenZStyleAPP.BAL.Helpers;
using GenZStyleAPP.BAL.Repository.Interfaces;
using ProjectParticipantManagement.BAL.Exceptions;
using ProjectParticipantManagement.BAL.Heplers;
using ProjectParticipantManagement.DAL.Infrastructures;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GenZStyleAPP.BAL.Repository.Implementations
{
    public class HashTagRepository : IHashTagRepository
    {
        private UnitOfWork _unitOfWork;
        private IMapper _mapper;
        public HashTagRepository(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = (UnitOfWork)unitOfWork;
            _mapper = mapper;
        }

        #region GetHashTag
        public async Task<List<GetHashTagResponse>> GetHashTagsAsync()
        {
            try
            {
                var hashtags = await _unitOfWork.HashTagDAO.GetAllHashTag();
                return _mapper.Map<List<GetHashTagResponse>>(hashtags);
            }
            catch (NotFoundException ex)
            {
                string error = ErrorHelper.GetErrorString(ex.Message);
                throw new NotFoundException(error);
            }
            catch (Exception ex)
            {
                string error = ErrorHelper.GetErrorString(ex.Message);
                throw new Exception(error);
            }
        }
        #endregion

        /*public async Task<GetHashTagResponse> GetHashTagByName(GetHashTagRequest hashTagRequest)
        {
            try
            {


            }catch (Exception ex)
            {
                string error = ErrorHelper.GetErrorString(ex.Message);
                throw new Exception(error);
            }
        }*/
        #region Add HashTag
        public async Task<GetHashTagResponse> AddNewHashTag(FireBaseImage fireBaseImage, GetHashTagRequest hashTagRequest)
        {
            try
            {
                var post = await _unitOfWork.PostDAO.GetPosts();
                var HashtagByName = await _unitOfWork.HashTagDAO.GetHashTagByNameAsync(hashTagRequest.Name);
                if (HashtagByName != null) 
                {
                    throw new BadRequestException("HashTag already exist in the system.");
                }
                
                Hashtag hashtag = new Hashtag
                {
                    Name = hashTagRequest.Name,
                    CreationDate = hashTagRequest.CreationDate,
                    
                };
                
                
                // Upload image to firebase
                FileHelper.SetCredentials(fireBaseImage);
                FileStream fileStream = FileHelper.ConvertFormFileToStream(hashTagRequest.Image);
                Tuple<string, string> result = await FileHelper.UploadImage(fileStream, "HashTag");
                hashtag.Image = result.Item1;

                
                /*Post MathchingPost = null;                 
                foreach (var postById in post)
                {
                    if (postById.PostId == 1)
                    {
                        MathchingPost = postById;
                        break;
                    }

                    
                }*/

                //Gắn hashtag vào bài post
                /*HashPost hashPost = new HashPost
                {
                    PostId = MathchingPost.PostId,
                    HashTageId = hashtag.id,
                    CreateAt = DateTime.Now,
                    UpdateAt = DateTime.Now,
                    Post = MathchingPost,
                    Hashtag = hashtag


                };
                hashtag.HashPosts.Add(hashPost);*/
                await _unitOfWork.HashTagDAO.CreateHashTagAsync(hashtag);
                await this._unitOfWork.CommitAsync();//lưu data
                return this._mapper.Map<GetHashTagResponse>(hashtag);

            }
            catch (BadRequestException ex)
            {
                string fieldNameError = "";
                if (ex.Message.ToLower().Contains("name"))
                {
                    fieldNameError = "Name";
                }
                string error = ErrorHelper.GetErrorString(fieldNameError, ex.Message);
                throw new BadRequestException(error);
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
