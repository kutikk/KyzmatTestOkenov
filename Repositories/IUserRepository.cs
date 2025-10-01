﻿using okenovTest.Entity;

namespace okenovTest.Repositories
{
    public interface IUserRepository
    {
        Task<User?> GetByUsernameAsync(string username);
        Task AddAsync(User user);
    }
}
