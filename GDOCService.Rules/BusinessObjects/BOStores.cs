using GDOCService.DataAccess.DataContext;
using GDOCService.DataAccess.Models;
using GDOCService.Rules.Validators;
using Microsoft.EntityFrameworkCore;
using SharedService.Responses.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace GDOCService.Rules.BusinessObjects
{
    public class BOStores : IDisposable
    {
        #region Properties
        public GDOCContext db { get; set; }
        public List<Stores> Stores { get; set; }
        public Stores Store { get; set; }

        public StoreValidator StoreValidator { get; set; }
        public string Resultado { get; set; }
        #endregion
        #region Builder
        public BOStores(GDOCContext _db,Stores _store)
        {
            db = _db;
            Store = _store;
            StoreValidator = new StoreValidator(_db, _store);
        }
        public BOStores(GDOCContext _db)
        {
            db = _db;
            StoreValidator = new StoreValidator(_db);
        }
        public BOStores(GDOCContext _db, List<Stores> _store)
        {
            db = _db;
            Stores = _store;
            StoreValidator = new StoreValidator(_db, _store);
        }
        #endregion
        #region Methods
        public async Task<PetitionResponse> GetByCompany(int companyid)
        {
            List<Stores> listStores = new List<Stores>();
            try
            {
                if (!StoreValidator.canGetResult())
                {
                    return new PetitionResponse
                    {
                        success = false,
                        message = StoreValidator.Resultado,
                        module = "Stores",
                        URL = "pos/Stores/GetByCompany",
                        result = listStores
                    };
                }


                listStores = await db.Stores.Where(x => x.CompanyID == companyid).ToListAsync();

                return new PetitionResponse
                {
                    success = true,
                    message = "Tiendas consultadas con éxito.",
                    module = "Stores",
                    URL = "pos/Stores/GetByCompany",
                    result = listStores
                };
            }
            catch (Exception ex)
            {
                return new PetitionResponse
                {
                    success = false,
                    message = "No es posible consultar las tiendas de la compañia: " + ex.Message,
                    module = "Stores",
                    URL = "pos/Stores/GetByCompany",
                    result = listStores
                };
            }
        }
        public async Task<PetitionResponse> GetByID(int companyid,int storeid)
        {
            Stores stores = new Stores();
            try
            {
                if (!StoreValidator.canGetResult())
                {
                    return new PetitionResponse
                    {
                        success = false,
                        message = StoreValidator.Resultado,
                        module = "Stores",
                        URL = "pos/Stores/GetByCompany",
                        result = stores
                    };
                }


                stores = await db.Stores.Where(x => x.CompanyID == companyid && x.StoreId == storeid && x.Name == "Juan").FirstOrDefaultAsync();
                stores.NombreAMostrar = stores.Name + "- " + stores.StoreCode;
                
                return new PetitionResponse
                {
                    success = true,
                    message = "Tiendas consultadas con éxito.",
                    module = "Stores",
                    URL = "pos/Stores/GetByCompany",
                    result = stores
                };
            }
            catch (Exception ex)
            {
                return new PetitionResponse
                {
                    success = false,
                    message = "No es posible consultar las tiendas de la compañia: " + ex.Message,
                    module = "Stores",
                    URL = "pos/Stores/GetByCompany",
                    result = stores
                };
            }
        }
        public async Task<PetitionResponse> Add()
        {
            try
            {
                if (!StoreValidator.canSave())
                {
                    return new PetitionResponse 
                    {
                        success = false,
                        message = StoreValidator.Resultado,
                        module = "Stores",
                        URL = "pos/Stores/Add",
                        result = Store
                    };
                }

                db.Stores.Add(Store);
                if (await db.SaveChangesAsync() <= 0)
                {
                    return new PetitionResponse 
                    {
                        success = false,
                        message = "No es posible guardar la tienda de la compañia",
                        module = "Stores",
                        URL = "pos/Stores/Add",
                        result = Store
                    };
                }
                return new PetitionResponse
                {
                    success = true,
                    message = "Tienda creada con exito!",
                    module = "Stores",
                    URL = "pos/Stores/Add",
                    result = Store
                };
            }
            catch (Exception ex)
            {
                return new PetitionResponse
                {
                    success = false,
                    message = "No es posible guardar la tienda de la compañia: " + ex.Message,
                    module = "Stores",
                    URL = "pos/Stores/Add",
                    result = null
                };
            }
        }
        public async Task<PetitionResponse> Update()
        {
            try
            {
                if (!StoreValidator.canSave())
                {
                    return new PetitionResponse
                    {
                        success = false,
                        message = StoreValidator.Resultado,
                        module = "Stores",
                        URL = "pos/Stores/Update",
                        result = Store
                    };
                }
                db.Entry(this.Store).State = EntityState.Modified;
                if (await db.SaveChangesAsync() <= 0)
                {
                    return new PetitionResponse
                    {
                        success = false,
                        message = "No es posible actualizar la tienda de la compañia",
                        module = "Stores",
                        URL = "pos/Stores/Update",
                        result = Store
                    };
                }
                return new PetitionResponse
                {
                    success = true,
                    message = "Tienda actualizada con exito!",
                    module = "Stores",
                    URL = "pos/Stores/Update",
                    result = Store
                };
            }
            catch (Exception ex)
            {
                return new PetitionResponse
                {
                    success = false,
                    message = "No es posible actualizar la tienda de la compañia: " + ex.Message,
                    module = "Stores",
                    URL = "pos/Stores/Update",
                    result = null
                };
            }
        }
        public async Task<PetitionResponse> Delete(int companyid, int storeid)
        {
            try
            {
                var store = await db.Stores.Where(x => x.CompanyID == companyid && x.StoreId == storeid).FirstOrDefaultAsync();
                db.Stores.Remove(store);
                if (await db.SaveChangesAsync() <= 0)
                {
                    return new PetitionResponse
                    {
                        success = false,
                        message = "No es posible eliminar la tienda de la compañia",
                        module = "Stores",
                        URL = "pos/Stores/Delete",
                        result = store
                    };
                }
                return new PetitionResponse
                {
                    success = true,
                    message = "Tienda eliminada con exito!",
                    module = "Stores",
                    URL = "pos/Stores/Update",
                    result = store
                };
            }
            catch (Exception ex)
            {
                return new PetitionResponse
                {
                    success = false,
                    message = "No es posible eliminar la tienda de la compañia: " + ex.Message,
                    module = "Stores",
                    URL = "pos/Stores/Delete",
                    result = null
                };
            }
        }
        public void Dispose()
        {
            throw new NotImplementedException();
        }
        #endregion
    }
}
