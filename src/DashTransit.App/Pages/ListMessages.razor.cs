// <copyright file="ListMessages.razor.cs" company="James Dibble">
// Copyright (c) James Dibble. All rights reserved.
// </copyright>

namespace DashTransit.App.Pages
{
    using System;
    using System.Threading.Tasks;
    using MediatR;
    using Microsoft.AspNetCore.Components;
    using MudBlazor;
    using Core = DashTransit.Core.Application;

    public partial class ListMessages
    {
        [Inject]private IMediator Mediator { get; init; }

        private bool Loading { get; set; }

        private async Task<T> WithLoading<T>(Func<Task<T>> loader)
        {
            this.Loading = true;

            try
            {
                return await loader();
            }
            finally
            {
                this.Loading = false;
            }
        }

        private Task<TableData<Core.ListMessageResponse.Message>> LoadPage(TableState state) => this.WithLoading(async () =>
        {
            var response = await this.Mediator.Send(new Core.ListMessages(state.Page, state.PageSize));

            return new TableData<Core.ListMessageResponse.Message> { TotalItems = response.Total, Items = response.Messages };
        });
    }
}
