﻿using Glimpse.EF.Inspector.Core;

namespace Glimpse.EF
{
    public static class Initialize
    {
        public static void EF(this Glimpse.Core.Setting.Initializer initializer)
        { 
            EntityFrameworkExecutionBlock.Instance.Execute();
        }
    }
}
