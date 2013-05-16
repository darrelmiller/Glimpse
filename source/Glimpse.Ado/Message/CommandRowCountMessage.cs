﻿using System;

namespace Glimpse.Ado.Message
{
    public class CommandRowCountMessage : AdoCommandMessage
    {
        public long RowCount { get; protected set; }

        public CommandRowCountMessage(Guid connectionId, Guid commandId, long rowCount) : base(connectionId, commandId)
        {
            RowCount = rowCount;
        }
    }
}