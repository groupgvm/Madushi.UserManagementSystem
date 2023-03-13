using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Geveo.Users.Common
{
    public interface ILoggerService
    {
        void LogInfo(string message);
        void LogError(string errorMessage);
    }
}
