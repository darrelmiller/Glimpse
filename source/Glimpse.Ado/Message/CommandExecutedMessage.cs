﻿using System;
using System.Collections.Generic;

namespace Glimpse.Ado.Message
{
    public class CommandExecutedMessage : AdoCommandMessage
    {      
        public string CommandText { get; protected set; }
        public IList<CommandExecutedParamater> Parameters { get; protected set; }
        public bool HasTransaction { get; protected set; }

        public CommandExecutedMessage(Guid connectionId, Guid commandId, string commandText, IList<CommandExecutedParamater> parameters, bool hasTransaction) 
            : base(connectionId, commandId)
        {
            CommandId = commandId;
            CommandText = commandText;
            Parameters = parameters;
            HasTransaction = hasTransaction;
        }
    }

    public class CommandExecutedParamater
    {
        public string Name { get; set; }

        public object Value { get; set; }

        public string Type { get; set; }

        public int Size { get; set; }
    }
}