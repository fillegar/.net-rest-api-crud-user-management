using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using User.Core.Model;

namespace User.Repository
{
    public class UserInfoRepository
    {
        private readonly UserInfoDbContext _context;

        public UserInfoRepository(UserInfoDbContext context)
        {
            _context = context;
        }

        public Task<List<UserInfo>> GetAsync(CancellationToken cancellationToken = default)
        {
            return _context.UserInfos
                .AsNoTracking()
                .ToListAsync(cancellationToken);
        }

        public Task<UserInfo?> GetUserInfoByEmailAsync(string email, CancellationToken cancellationToken = default)
        {
            return _context.UserInfos
                .AsNoTracking()
                .FirstOrDefaultAsync(item => item.Email == email, cancellationToken);
        }

        public async Task AddAsync(UserInfo userInfo, CancellationToken cancellationToken = default)
        {
            await _context.UserInfos.AddAsync(userInfo, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);
        }

        public async Task UpdateAsync(UserInfo userInfo, CancellationToken cancellationToken = default)
        {
            _context.UserInfos.Update(userInfo);
            await _context.SaveChangesAsync(cancellationToken);
        }

        public async Task RemoveAsync(UserInfo userInfo, CancellationToken cancellationToken = default)
        {
            _context.UserInfos.Remove(userInfo);
            await _context.SaveChangesAsync(cancellationToken);
        }
    }
}
