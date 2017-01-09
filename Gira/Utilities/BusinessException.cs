using System;

namespace Gira.Utilities
{
    public class BusinessException : Exception
    {
        public BusinessException(string error) : base(error)
        {
            TriggeredByBusiness = true;
        }
        public bool TriggeredByBusiness { get; set; }
    }
}