using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Company.G01.DAL.Models;

namespace Company.G01.BLL.Respositories
{
    public interface IDepartmentRespository
    {
        IEnumerable<Department> GetAll();

        Department Get(int? id);
        int Add(Department model); 
        int Update(Department model); 
        int Delete(Department model);

    }
}
