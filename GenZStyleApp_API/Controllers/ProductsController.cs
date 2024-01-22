using FluentValidation;
using GenZStyleApp.DAL.Models;
using GenZStyleAPP.BAL.DTOs.FireBase;
using GenZStyleAPP.BAL.DTOs.Posts;
using GenZStyleAPP.BAL.DTOs.Products;
using GenZStyleAPP.BAL.Repository.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Routing.Controllers;
using Microsoft.Extensions.Options;
using ProjectParticipantManagement.BAL.Exceptions;
using ProjectParticipantManagement.BAL.Heplers;

namespace GenZStyleApp_API.Controllers
{
    //[Route("api/[controller]")]
    //[ApiController]
    public class ProductsController : ODataController
    {
        private IProductRepository _productRepository;
        private IValidator<AddProductRequest> _productValidator;
        private IOptions<FireBaseImage> _firebaseImageOptions;

        public ProductsController(IProductRepository productRepository, IValidator<AddProductRequest> productValidator, IOptions<FireBaseImage> firebaseImageOptions)
        {
            _productRepository = productRepository;
            _productValidator = productValidator;
            _firebaseImageOptions = firebaseImageOptions;
        }

        #region Create Product
        [HttpPost("odata/Product/AddNewProduct")]
        [EnableQuery]
        //[PermissionAuthorize("Staff")]
        public async Task<IActionResult> Post([FromForm] AddProductRequest addProductRequest)
        {
            try
            {
                var resultValid = await _productValidator.ValidateAsync(addProductRequest);
                if (!resultValid.IsValid)
                {
                    string error = ErrorHelper.GetErrorsString(resultValid);
                    throw new BadRequestException(error);
                }
                GetProductResponse product = await this._productRepository.CreateNewProductAsync(addProductRequest, _firebaseImageOptions.Value, HttpContext);
                return Ok(new
                {
                    Status = "Add Product Success",
                    Data = Created(product)
                });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }
        #endregion
    }
}
