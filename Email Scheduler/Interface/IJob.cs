using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Email_Scheduler.Interface
{
    public interface IJob
    {
        void SendMail();
    }
}
