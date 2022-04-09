using System;

namespace Service.Common.Exceptions
{
    public class NotFoundException : Exception
    {
        public NotFoundException(string name, object key) : base($"{name} ({key}) not found in database")
        {
        }

        public NotFoundException(string message) : base(message)
        {
        }
    }
}
