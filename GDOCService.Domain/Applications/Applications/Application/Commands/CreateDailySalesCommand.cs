using MediatR;
using POSService.DataAccess.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Runtime.Serialization;
using System.Threading.Tasks;

namespace POSService.Domain.Application.Commands
{
    [DataContract]
    public class CreateDailySalesCommand: IRequest<bool>
    {
        public SummaryDailySales _summary { get; set; }
        public CreateDailySalesCommand(SummaryDailySales summary) => _summary = summary;

    }
}
