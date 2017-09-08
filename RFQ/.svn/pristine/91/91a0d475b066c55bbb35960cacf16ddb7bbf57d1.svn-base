using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SSG.Core.Domain.PSP;
using SSG.Core.Data;
using System.Transactions;

namespace SSG.Services.PSP
{
    public class SupportCodeService :ISupportCodeService
    {
        #region Fields

        private readonly IRepository<SupportCode> _supportCodeRepository;

        #endregion

        #region ctor

        public SupportCodeService(IRepository<SupportCode> supportCodeRepository)
        {
            this._supportCodeRepository = supportCodeRepository;
        }

        #endregion

        #region Methods

        public IEnumerable<Core.Domain.PSP.SupportCode> GetAllSupportCodes(bool activeOnly = false)
        {
            return this._supportCodeRepository.Table
                .Where(s => (activeOnly ? s.Active : 1 == 1))
                .AsEnumerable();
        }

        public IQueryable<Core.Domain.PSP.SupportCode> GetAllSupportCodeQueryable(bool activeOnly = false)
        {
            return this._supportCodeRepository.Table
            .Where(s => (activeOnly ? s.Active : 1 == 1));
        }

        public void SaveSupportCode(Core.Domain.PSP.SupportCode supportCode)
        {
            try
            {
                using (var scope = new TransactionScope())
                {
                    if (supportCode.Id == 0)
                    {
                        this._supportCodeRepository.Insert(supportCode);
                    }
                    else
                    {
                        this._supportCodeRepository.Update(supportCode);
                    }
                    scope.Complete();
                }
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public IQueryable<Core.Domain.PSP.SupportCode> GetPagedSupportCodes(int currentPage, int pageSize, out int totalCount)
        {
            var query = this._supportCodeRepository.Table.OrderBy(s => s.SupportCodes);

            totalCount = query.Count();

            var pagedList = query
                .Skip((currentPage - 1) * pageSize)
                .Take(pageSize);

            return pagedList;
        }

        public Core.Domain.PSP.SupportCode GetSupportCodeById(int id)
        {
            return this._supportCodeRepository.GetById(id);
        }

        #endregion

    }
}
