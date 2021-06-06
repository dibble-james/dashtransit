// <copyright file="Message.razor.cs" company="James Dibble">
// Copyright (c) James Dibble. All rights reserved.
// </copyright>

namespace DashTransit.App.Pages
{
    using System;
    using System.Threading.Tasks;
    using DashTransit.Core.Application;
    using DashTransit.Core.Domain;
    using MediatR;
    using Microsoft.AspNetCore.Components;
    using Core = DashTransit.Core.Application;

    public partial class Message
    {
        [Inject] private IMediator Mediator { get; init; }

        [Parameter]
        public Guid MessageId { get; init; }

        private bool HasLoaded { get; set; }

        private Core.MessageDetailsResponse Response { get; set; }

        protected override async Task OnInitializedAsync()
        {
            var response = await this.Mediator.Send(new MessageDetails(new MessageId(this.MessageId)));
            this.Response = response.Match(x => x, _ => null);

            this.HasLoaded = true;
            this.StateHasChanged();
        }
    }
}
