using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TM1638Dash
{
    public interface ILog
    {
        void Fatal(object message);
        void Error(object message);
        void Warn(object message);
        void Info(object message);
        void Debug(object message);
    }
}
