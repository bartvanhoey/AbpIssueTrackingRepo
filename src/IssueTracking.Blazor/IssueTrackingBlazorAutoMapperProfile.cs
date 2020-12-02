﻿using AutoMapper;
using IssueTracking.Application.Contracts.Issues;

namespace IssueTracking.Blazor
{
    public class IssueTrackingBlazorAutoMapperProfile : Profile
    {
        public IssueTrackingBlazorAutoMapperProfile()
        {
            //Define your AutoMapper configuration here for the Blazor project.

            CreateMap<IssueDto, UpdateIssueDto>();
        }
    }
}
