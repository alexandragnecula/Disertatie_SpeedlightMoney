using System;
namespace DataLayer.SharedInterfaces
{
    public interface ICurrentUserService
    {
        public long? UserId { get; }
    }
}
