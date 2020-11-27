using GDOCService.DataAccess.DataContext;
using GDOCService.DataAccess.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GDOCService.Rules.Validators
{
    public class StoreValidator
    {
        #region Properties
        public GDOCContext db { get; set; }
        public Stores Store { get; set; }
        public List<Stores> Stores { get; set; }
        public string Resultado { get; set; }
        #endregion
        #region Builder
        public StoreValidator(GDOCContext _db)
        {
            db = _db;
        }
        public StoreValidator(GDOCContext _db, Stores _store)
        {
            db = _db;
            Store = _store;
        }
        public StoreValidator(GDOCContext _db, List<Stores> _store)
        {
            db = _db;
            Stores = _store;
        }
        #endregion
        #region Methods
        public bool canGetResult()
        {
            return true;
        }

        public bool canSave()
        {
            return true;
        }
        #endregion
    }
}
