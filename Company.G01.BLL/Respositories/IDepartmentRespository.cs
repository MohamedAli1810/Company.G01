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
<<<<<<< HEAD
        Department Get(int? id);
        int Add(Department model); 
        int Update(Department model); 
        int Delete(Department model);
        
=======
        Department? Get(int id);

        int Add(Department model); 
        int Update(Department model); 
        int Delete(Department model);
>>>>>>> 9c76c71d506f51e1d04347519ad3d8b412a0aca4
    }
}
