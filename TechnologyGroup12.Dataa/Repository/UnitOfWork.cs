using System;
using System.Collections.Generic;
using System.Text;
using TechnologyGroup12.DataAccess.Data;
using TechnologyGroup12.DataAccess.Repository.IRepository;

namespace TechnologyGroup12.DataAccess.Repository
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext _db;

        public UnitOfWork(ApplicationDbContext db)
        {
            _db = db;

            SP_Call = new SP_Call(_db);
        }

        public ISP_Call SP_Call { get; private set; }

        public void Dispose()
        {
            _db.Dispose();
        }
        public void Save()
        {
            _db.SaveChanges();
        }
    }
}
