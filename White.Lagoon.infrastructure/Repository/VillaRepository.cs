using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using White.Lagoon.Application.Common.Interfaces;
using White.Lagoon.Domain.Entities;
using White.Lagoon.infrastructure.Data;

namespace White.Lagoon.infrastructure.Repository
{
    public class VillaRepository :Repository<Villa>, IVillaRepository
    {

        private readonly ApplicationDbContext _db;

        public VillaRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }
     
        public void Save()
        {
            _db.SaveChanges();
        }

        public void Update(Villa entity)
        {
            _db.Villas.Update(entity);
        }
    }
}
