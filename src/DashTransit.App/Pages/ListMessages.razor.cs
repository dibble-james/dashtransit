// <copyright file="ListMessages.razor.cs" company="James Dibble">
// Copyright (c) James Dibble. All rights reserved.
// </copyright>

namespace DashTransit.App.Pages
{
    using System.Threading.Tasks;
    using MediatR;
    using Microsoft.AspNetCore.Components;
    using Core = DashTransit.Core.Application;

    public partial class ListMessages
    {
        [Inject]private IMediator Mediator { get; init; }

        private Core.ListMessageResponse Response { get; set; }

        protected override async Task OnInitializedAsync()
        {
            this.Response = await this.Mediator.Send(new Core.ListMessages(1));
            this.StateHasChanged();
        }
    }
}
