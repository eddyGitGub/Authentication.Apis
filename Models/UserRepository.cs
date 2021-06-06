using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Authentication.Apis.Models
{
    public class UserRepository
    {
        private static AuthDbDbContext authDbDbContext;

        static UserRepository()
        {
            
        }

        public static AuthDbDbContext AuthDbDbContext { get => authDbDbContext; set => authDbDbContext = value; }
    }
}
