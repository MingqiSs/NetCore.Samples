using Samples.Repository;
using Samples.Repository.Context;
using Samples.Repository.Interface;
using Samples.Repository.Models;
using Samples.Service.Domain.Interface;
using System;
using System.Collections.Generic;
using System.Text;

namespace Samples.Service.Domain
{
    public class UserDomainService : BookStoreRepository<McUser>, IUserDomainService
    {
        public UserDomainService(BookStoreContext dbContext)
      : base(dbContext)
        {

        }
    }
}
