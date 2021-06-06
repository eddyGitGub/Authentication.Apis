using Authentication.Apis.Models;
using Microsoft.AspNetCore.Identity;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Authentication.Apis
{
    public class AppUserStore : IUserStore<AppUser>, IUserPasswordStore<AppUser>
    {
        private readonly AuthDbDbContext _authDbDbContext;

        public AppUserStore(AuthDbDbContext authDbDbContext)
        {
            _authDbDbContext = authDbDbContext;
        }
        #region IUserStore
        public Task<IdentityResult> CreateAsync(AppUser user, CancellationToken cancellationToken)
        {
            _authDbDbContext.Add(new AppUser
            {
                Id = user.Id,
                UserName = user.UserName,
                Email = user.Email,
                NormalizeUserName = user.NormalizeUserName,
                PasswordHash = user.PasswordHash
            });
            var add = _authDbDbContext.SaveChanges();
            //if(add > 0)
            //{
            //    return Task.FromResult(IdentityResult.Success);
            //}

            return Task.FromResult(IdentityResult.Success);
        }

        public Task<IdentityResult> DeleteAsync(AppUser user, CancellationToken cancellationToken)
        {
            var appUser = _authDbDbContext.AppUsers.FirstOrDefault(u => u.Id == user.Id);

            if (appUser != null)
            {
                _authDbDbContext.AppUsers.Remove(appUser);
            }

            return Task.FromResult(IdentityResult.Success);
        }

        public void Dispose() { }

        public Task<AppUser> FindByIdAsync(string userId, CancellationToken cancellationToken)
        {
            return Task.FromResult(_authDbDbContext.AppUsers.FirstOrDefault(u => u.Id == userId));
        }

        public Task<AppUser> FindByNameAsync(string normalizedUserName, CancellationToken cancellationToken)
        {
            return Task.FromResult(_authDbDbContext.AppUsers.FirstOrDefault(u => u.NormalizeUserName == normalizedUserName));
        }

        public Task<string> GetNormalizedUserNameAsync(AppUser user, CancellationToken cancellationToken)
        {
            return Task.FromResult(user.NormalizeUserName);
        }

        public Task<string> GetUserIdAsync(AppUser user, CancellationToken cancellationToken)
        {
            return Task.FromResult(user.Id);
        }

        public Task<string> GetUserNameAsync(AppUser user, CancellationToken cancellationToken)
        {
            return Task.FromResult(user.UserName);
        }

        public Task SetNormalizedUserNameAsync(AppUser user, string normalizedName, CancellationToken cancellationToken)
        {
            user.NormalizeUserName = normalizedName;
            return Task.CompletedTask;
        }

        public Task SetUserNameAsync(AppUser user, string userName, CancellationToken cancellationToken)
        {
            user.UserName = userName;
            return Task.CompletedTask;
        }

        public Task<IdentityResult> UpdateAsync(AppUser user, CancellationToken cancellationToken)
        {
            var appUser = _authDbDbContext.AppUsers.FirstOrDefault(u => u.Id == user.Id);

            if (appUser != null)
            {
                appUser.NormalizeUserName = user.NormalizeUserName;
                appUser.UserName = user.UserName;
                appUser.Email = user.Email;
                appUser.PasswordHash = user.PasswordHash;
            }

            return Task.FromResult(IdentityResult.Success);
        }
        #endregion

        #region IUserPasswordStore
        public Task<bool> HasPasswordAsync(AppUser user, CancellationToken cancellationToken)
        {
            return Task.FromResult(user.PasswordHash != null);
        }

        public Task<string> GetPasswordHashAsync(AppUser user, CancellationToken cancellationToken)
        {
            return Task.FromResult(user.PasswordHash);
        }

        public Task SetPasswordHashAsync(AppUser user, string passwordHash, CancellationToken cancellationToken)
        {
            user.PasswordHash = passwordHash;
            return Task.CompletedTask;
        }
        #endregion
    }
}
