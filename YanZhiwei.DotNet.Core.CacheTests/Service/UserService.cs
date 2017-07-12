﻿using System.Collections.Generic;
using YanZhiwei.DotNet.Core.Cache;
using YanZhiwei.DotNet.Core.CacheTests.Model;

namespace YanZhiwei.DotNet.Core.CacheTests.Service
{
    public class UserService : IUserService
    {
        private IEventPublisher _eventPublisher;
        private const string CacheKey = "UserServer";

        public UserService(IEventPublisher eventPublisher)
        {
            _eventPublisher = eventPublisher;
        }

        public void Delete(User item)
        {
            using (var dbContext = new AccountDbContext())
            {
                dbContext.Delete<User>(item);
            }
            _eventPublisher.EntityDeleted<User>(item);
        }

        public IList<User> GetAll()
        {
            using (var dbContext = new AccountDbContext())
            {
                return dbContext.Users.ToCacheList(CacheKey);
            }
        }

        public User GetById(object id)
        {
            return CacheHelper.Get(string.Format("{0}_{1}", CacheKey, id), () =>
             {
                 using (var dbContext = new AccountDbContext())
                 {
                     return dbContext.Find<User>(id);
                 }
             });
        }

        public void Insert(User item)
        {
            //using (var dbContext = new AccountDbContext())
            //{
            //    dbContext.Insert<User>(item);
            //}
            _eventPublisher.EntityInserted<User>(item);
        }

        public void Update(User item)
        {
            using (var dbContext = new AccountDbContext())
            {
                dbContext.Update<User>(item);
            }
            _eventPublisher.EntityUpdated<User>(item);
        }
    }
}