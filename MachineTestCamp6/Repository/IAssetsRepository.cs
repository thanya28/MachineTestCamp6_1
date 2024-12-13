using MachineTestCamp6.Model;
using Microsoft.AspNetCore.Mvc;

namespace MachineTestCamp6.Repository
{
    public interface IAssetsRepository
    {  //ASSET TYPE TABLE LIST 
      
        public Task<ActionResult<IEnumerable<AssetType>>> GetAllassetTypes();
      
     

        ////ASSETS DETAILS TABLE LIST
       
        public Task<ActionResult<IEnumerable<AssetDetail>>> GetAlldetails();
        
       
        //// ASSETS MAIN TABLE CRUD 
        public Task<AssetMain> createAssetmain(AssetMain main);
        public Task<ActionResult<IEnumerable<AssetMain>>> GetAllAssetsMain();
        public Task<ActionResult<AssetMain>> UpdateAssetMain(int id, AssetMain main);
        public JsonResult DeleteAssetMain(int id);
        public Task<ActionResult<AssetMain>> SearchAssetMainById(int id);

       
    }
}
