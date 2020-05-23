using System;
using System.Collections.Generic;

namespace BusinessLayer.Common.Models.Authentication
{
    public class AuthFailedResponse
    {
        public IEnumerable<string> Errors { get; set; }
    }
}
