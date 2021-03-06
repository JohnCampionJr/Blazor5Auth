﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Features.Base;
using FluentValidation;
using MediatR;

namespace Features.Account
{
    public class ConfirmEmailChange
    {
        public class Command : IRequest<Result>
        {
            public string UserId { get; set; }
            public string Code { get; set; }
            public string Email { get; set; }
            public bool ClientAuth { get; set; }
        }

        public class CommandValidator : AbstractValidator<Command>
        {
            public CommandValidator()
            {
                RuleFor(p => p.UserId).NotEmpty();
                RuleFor(p => p.Code).NotEmpty();
            }
        }

        public class Result : BaseResult {
            public string Token { get; set; }
            public bool RequiresEmailConfirmation { get; set; }
        }

        //this is here for easy navigation with goto implementation
        public interface ICommandHandler : IRequestHandler<Command, Result> { }

    }
}
