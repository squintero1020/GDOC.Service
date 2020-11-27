﻿using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace POSService.Domain.Application.Commands
{
    public class IdentifiedCommand<T, R> : IRequest<R>
        where T : IRequest<R>
    {
        public T Command { get; }
        public Guid Id { get; }
        public IdentifiedCommand(T command, Guid id) => (Command, Id) = (command, id);

    }
}
