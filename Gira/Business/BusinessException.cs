using System;

namespace Gira.Business
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