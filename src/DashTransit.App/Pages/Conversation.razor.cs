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

    public partial class Conversation
    {
        [Inject] private IMediator Mediator { get; init; }

        [Parameter]
        public Guid ConversationId { get; init; }

        private bool HasLoaded { get; set; }

        private Core.ConversationDetailsResponse Response { get; set; }

        protected override async Task OnInitializedAsync()
        {
            this.Response = await this.Mediator.Send(new ConversationDetails(new CorrelationId(this.ConversationId)));
            this.HasLoaded = true;
            this.StateHasChanged();
        }
    }
}
