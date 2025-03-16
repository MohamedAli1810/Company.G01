using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Company.G01.DAL.Data.Contexts;
using Company.G01.DAL.Models;

namespace Company.G01.BLL.Respositories
{
    public class DepartmentRespository : IDepartmentRespository
    {
        private CompanyDbContext _Context;

        public DepartmentRespository(CompanyDbContext context)
        {
            _Context = context;
        }

        public IEnumerable<Department> GetAll()
        {
            return _Context.Departments.ToList();
        }

<<<<<<< HEAD
        public Department Get(int? id)
=======
        public Department? Get(int id)
>>>>>>> 9c76c71d506f51e1d04347519ad3d8b412a0aca4
        {
            return _Context.Departments.Find(id);
        }

        public int Add(Department model)
        {
             _Context.Departments.Add(model);
            return _Context.SaveChanges();
        }
        
        public int Update(Department model)
        {
            _Context.Departments.Update(model);
            return _Context.SaveChanges();
        }
        public int Delete(Department model)
        {
            _Context.Departments.Remove(model);
            return _Context.SaveChanges();
        }
<<<<<<< HEAD


        
=======
>>>>>>> 9c76c71d506f51e1d04347519ad3d8b412a0aca4
    }
}
