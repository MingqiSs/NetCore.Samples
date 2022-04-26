using Samples.Repository.Interface;
using Samples.Repository.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Samples.Service.Domain.Interface
{
    public interface IUserDomainService : IBookStoreRepository<McUser>
    {
    }
}
