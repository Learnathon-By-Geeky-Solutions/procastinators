﻿using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace FinanceTracker.Application.Loans.Commands.CreateLoanAsLender;

public class CreateLoanAsLenderCommand : IRequest<int>
{
    public decimal Amount { get; set; } = default!;
    public string? Note { get; set; } = default;
    public DateTime DueDate { get; set; } = default!;
    public int WalletId { get; set; } = default;
}
