using Core.Models.Abstracts;
using System;

namespace Core.Models.Auth
{
    public class BaseSession : BaseEntity
    {
        public string Token { get; set; }
        public Guid AccountId { get; set; }
    }
}
