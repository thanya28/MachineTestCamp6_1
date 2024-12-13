using MachineTestCamp6.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace MachineTestCamp6.Repository
{
    public class AssetsRepository : IAssetsRepository
    {
        private readonly XyztechnologiesContext _context;

        public AssetsRepository(XyztechnologiesContext context)
        {
            _context = context;
        }

       
         #region ASSET TYPE LIST
       

       

        public async Task<ActionResult<IEnumerable<AssetType>>> GetAllassetTypes()
        {
            try
            {
                if (_context != null)
                {
                    return await _context.AssetTypes.ToListAsync();
                }

                
                return new List<AssetType>();
            }
            catch (Exception ex)
            {
                return null;
            }
        }






        #endregion

        #region ASSET DETAILS LIST


       
        public async Task<ActionResult<IEnumerable<AssetDetail>>> GetAlldetails()
        {
            try
            {
                if (_context != null)
                {
                    return await _context.AssetDetails.ToListAsync();
                }


                return new List<AssetDetail>();
            }
            catch (Exception ex)
            {
                return null;
            }
        }

      

        #endregion
        #region ASSETS MAIN TABLE CRUD
        public async Task<AssetMain> createAssetmain(AssetMain main)
        {
            try
            {
                if (main == null)
                {
                    throw new ArgumentNullException(nameof(main), "AssetMain data is null");

                }
                // ensure context is not null
                if (_context == null)
                {
                    throw new InvalidOperationException("Database context is not initialized.");
                }
              
                await _context.AssetMains.AddAsync(main);

               
                await _context.SaveChangesAsync();
              
                var asmain = await _context.AssetMains.Include(e => e.Vendor)
                    .Include(e => e.AssetDetails)
                    .Include(e => e.AssetType)
                    .Include(e => e.PurchaseOrder)
                    .FirstOrDefaultAsync(e => e.AssetId == main.AssetId);
                return asmain;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public async Task<ActionResult<IEnumerable<AssetMain>>> GetAllAssetsMain()
        {
            try
            {
                if (_context != null)
                {
                    return await _context.AssetMains.Include(v => v.Vendor).
                        Include(v => v.AssetDetails).
                        Include(v => v.AssetType).
                         Include(v => v.PurchaseOrder).ToListAsync();
                }

                return new List<AssetMain>();
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public async Task<ActionResult<AssetMain>> UpdateAssetMain(int id, AssetMain main)
        {
            try
            {
                if (main == null)
                {
                    throw new ArgumentNullException(nameof(main), "Assetmain data is null");

                }
                
                if (_context == null)
                {
                    throw new InvalidOperationException("Database context is not initialized.");
                }
                
                var existing = await _context.AssetMains.FindAsync(id);
                if (existing == null)
                {
                    return null;
                }

                
                existing.DateAdded = main.DateAdded;
                existing.Status = main.Status;
                



                await _context.SaveChangesAsync();
                
                var asmain = await _context.AssetMains.Include(e => e.Vendor)
                    .Include(e => e.AssetDetails)
                    .Include(e => e.AssetType)
                    .Include(e => e.PurchaseOrder)
                    .FirstOrDefaultAsync(e => e.AssetId == main.AssetId);
                return asmain;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public JsonResult DeleteAssetMain(int id)
        {
            try
            {
                if (id <= 0)
                {
                    return new JsonResult(new
                    {
                        success = false,
                        message = "Invalid Asset id"

                    })
                    {
                        StatusCode = StatusCodes.Status400BadRequest
                    };

                }
               
                if (_context == null)
                {
                    return new JsonResult(new
                    {
                        success = false,
                        message = "Database coontext is not initialized"

                    })
                    {
                        StatusCode = StatusCodes.Status500InternalServerError
                    };
                }
                //Find the employee by id
                var existingmain = _context.AssetMains.Find(id);
                if (existingmain == null)
                {
                    return new JsonResult(new
                    {
                        success = false,
                        message = "AssetMain  not found"

                    })
                    {
                        StatusCode = StatusCodes.Status400BadRequest
                    };
                }
                //remove

                _context.AssetMains.Remove(existingmain);



                //save changes to the database
                _context.SaveChangesAsync();


                return new JsonResult(new
                {
                    success = true,
                    message = "AssetMain deleted successfully"

                })
                {
                    StatusCode = StatusCodes.Status200OK
                };

            }
            catch (Exception ex)
            {
                return new JsonResult(new
                {
                    success = false,
                    message = "Database coontext is not initialized"

                })
                {
                    StatusCode = StatusCodes.Status500InternalServerError
                };
            }

        }
        public async Task<ActionResult<AssetMain>> SearchAssetMainById(int id)
        {
            try
            {
                if (_context != null)
                {
                    
                    var order = await _context.AssetMains
                    .Include(ass => ass.AssetType)
                     .Include(ass => ass.AssetDetails)
                      .Include(ass => ass.PurchaseOrder)
                      .Include(ass => ass.Vendor)
                    .FirstOrDefaultAsync(e => e.AssetId == id);
                    return order;
                }
                return null;
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        #endregion
    }
}
