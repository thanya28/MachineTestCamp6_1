using MachineTestCamp6.Model;
using MachineTestCamp6.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace MachineTestCamp6.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AssetsController : ControllerBase
    {
        private readonly IAssetsRepository _repository;


        public AssetsController(IAssetsRepository repository)
        {
            _repository = repository;
        }

        #region Get All Asset Types
        [HttpGet("p3")]
        [Authorize(AuthenticationSchemes = "Bearer")]
        public async Task<ActionResult<IEnumerable<AssetMain>>> GetAllAssetsMain()
        {
            var assetTypes = await _repository.GetAllAssetsMain();
            if (assetTypes == null)
            {
                return NotFound("No asset types found.");
            }
            return Ok(assetTypes);
        }
        #endregion

        #region Add New Asset main
        [HttpPost("v2")]
       
        public async Task<ActionResult<AssetMain>> createAssetmain(AssetMain asset)
        {
            if (ModelState.IsValid)
            {
                var newAssetType = await _repository.createAssetmain(asset);
                if (newAssetType != null)
                {
                    return Ok(newAssetType);
                }
                else
                {
                    return NotFound("Asset type could not be added.");
                }
            }
            return BadRequest("Invalid asset type data.");
        }
        #endregion


        #region Update Asset main
        [HttpPut("{id}")]
        [Authorize(AuthenticationSchemes = "Bearer")]
        public async Task<ActionResult<AssetMain>> UpdateAssetMain(int id, AssetMain assetMain)
        {
            if (ModelState.IsValid)
            {
                var updatedAssetType = await _repository.UpdateAssetMain(id, assetMain);
                if (updatedAssetType != null)
                {
                    return Ok(updatedAssetType);
                }
                else
                {
                    return NotFound("Asset type could not be updated.");
                }
            }
            return BadRequest("Invalid asset type data.");
        }
        #endregion


        #region Delete Asset main
        [HttpDelete("{id}")]
        [Authorize(AuthenticationSchemes = "Bearer")]
        public IActionResult DeleteAssetMain(int id)
        {
            try
            {
                var result = _repository.DeleteAssetMain(id);

                if (result == null)
                {
                    //if result indicates failure or null
                    return NotFound(new
                    {
                        success = false,
                        message = "mainassets could not be deleted or not found"
                    });
                }
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    new { success = false, message = "An unexpected error occurs" });
            }
        }
        #endregion

        #region Search by id
        [HttpGet("{id1}")]
        [Authorize(AuthenticationSchemes = "Bearer")]
        public async Task<ActionResult<AssetMain>> SearchAssetMainById(int id)
        {
            var order = await _repository.SearchAssetMainById(id);
            if (order == null)
            {
                return NotFound("No assets found ");
            }
            return Ok(order);
        }
        #endregion










        #region Get All Asset Types
        [HttpGet("p1")]
        [Authorize(AuthenticationSchemes = "Bearer")]

        public async Task<ActionResult<IEnumerable<AssetType>>> GetAllAssetTypes()
        {
            var assetTypes = await _repository.GetAllassetTypes();
            if (assetTypes == null)
            {
                return NotFound("No asset types found.");
            }
            return Ok(assetTypes);
        }
        #endregion



      

       









        #region Get All Asset details
        [HttpGet("p2")]
        [Authorize(AuthenticationSchemes = "Bearer")]

        public async Task<ActionResult<IEnumerable<AssetDetail>>> GetAlldetails()
        {
            var assetTypes = await _repository.GetAlldetails();
            if (assetTypes == null)
            {
                return NotFound("No asset details found.");
            }
            return Ok(assetTypes);
        }
        #endregion


       
     
    }
}



