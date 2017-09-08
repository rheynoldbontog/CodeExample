using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SSG.Core.Domain.RFQ;

namespace SSG.Services.RFQ
{
    public interface IDepartmentService
    {
        IEnumerable<Department> GetAllDepartments(bool activeOnly = false);
        IQueryable<Department> GetAllDepartmentsQueryable(bool activeOnly = false);
        void SaveDepartment(Department department);
        IQueryable<Department> GetPagedDepartments(int currentPage, int pageSize, out int totalCount);
        Department GetDepartmentById(int id);
    }
}
