using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using User.Core.Model;
using User.Repository;

namespace User.Service
{
    public class UserInfoService
    {
        private readonly UserInfoRepository _userInfoRepository;

        public UserInfoService(UserInfoRepository userInfoRepository)
        {
            _userInfoRepository = userInfoRepository;
        }

        public Task<List<UserInfo>> GetUserInfosAsync(CancellationToken cancellationToken = default)
        {
            return _userInfoRepository.GetAsync(cancellationToken);
        }

        public Task<UserInfo?> GetUserInfoByEmailAsync(string email, CancellationToken cancellationToken = default)
        {
            return _userInfoRepository.GetUserInfoByEmailAsync(email, cancellationToken);
        }

        public Task AddUserInfoAsync(UserInfo userInfo, CancellationToken cancellationToken = default)
        {
            return _userInfoRepository.AddAsync(userInfo, cancellationToken);
        }

        public Task UpdateUserInfoAsync(UserInfo userInfo, CancellationToken cancellationToken = default)
        {
            return _userInfoRepository.UpdateAsync(userInfo, cancellationToken);
        }

        public Task RemoveUserInfoAsync(UserInfo userInfo, CancellationToken cancellationToken = default)
        {
            return _userInfoRepository.RemoveAsync(userInfo, cancellationToken);
        }
    }
}
