using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SSG.Core.Data;
using SSG.Core.Domain.RFQ;
using System.Transactions;

namespace SSG.Services.RFQ
{
    public class DepartmentService : IDepartmentService
    {
        #region Fields

        private readonly IRepository<Department> _departmentRepository;
        
        #endregion

        #region ctor

        public DepartmentService(IRepository<Department> departmentRepository)
        {
            this._departmentRepository = departmentRepository;
        }

        #endregion

        #region Methods

        public IEnumerable<Department> GetAllDepartments(bool activeOnly = false)
        {
            return this._departmentRepository.Table
                .Where(s => (activeOnly ? s.Active : 1 == 1))
                .AsEnumerable();
        }

        public IQueryable<Department> GetAllDepartmentsQueryable(bool activeOnly = false)
        {
            return this._departmentRepository.Table
                .Where(s => (activeOnly ? s.Active : 1 == 1))
                .AsQueryable();
        }

        public void SaveDepartment(Department department)
        {
            try
            {
                using (var scope = new TransactionScope())
                {
                    if (department.Id == 0)
                    {
                        this._departmentRepository.Insert(department);
                    }
                    else
                    {
                        this._departmentRepository.Update(department);
                    }
                    scope.Complete();
                }
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public IQueryable<Department> GetPagedDepartments(int currentPage, int pageSize, out int totalCount)
        {
            var query = this._departmentRepository.Table.OrderBy(s => s.Name);

            totalCount = query.Count();

            var pagedList = query
                .Skip((currentPage - 1) * pageSize)
                .Take(pageSize);

            return pagedList;
        }

        public Department GetDepartmentById(int id)
        {
            return this._departmentRepository.GetById(id);
        }



        #endregion
    }
}
