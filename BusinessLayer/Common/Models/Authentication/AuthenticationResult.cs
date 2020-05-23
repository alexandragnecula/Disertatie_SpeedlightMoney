using System;
using System.Collections.Generic;

namespace BusinessLayer.Common.Models.Authentication
{
    public class AuthenticationResult
    {
        public string Token { get; set; }
        public bool Success { get; set; }
        public IEnumerable<string> Errors { get; set; }
    }
}
