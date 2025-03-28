﻿using ElectronicDiary.Pages.AdminPageComponents.BaseView;
using ElectronicDiary.Pages.Components;
using ElectronicDiary.Web.Api;
using ElectronicDiary.Web.DTO.Requests.Users;
using ElectronicDiary.Web.DTO.Responses.Users;

namespace ElectronicDiary.Pages.AdminPageComponents.UserView
{
    public class UserViewElemCreator<TResponse, TRequest, TController, TViewObjectCreator> : BaseViewElemCreator<TResponse, TRequest, TController, TViewObjectCreator>
        where TResponse : UserResponse, new()
        where TRequest : UserRequest, new()
        where TController : IController, new()
        where TViewObjectCreator : BaseViewObjectCreator<TResponse, TRequest, TController>, new()
    {

        protected override void CreateUI(ref int rowIndex)
        {
            base.CreateUI(ref rowIndex);

            LineElemsCreator.AddLineElems(
                grid: _grid,
                rowIndex: rowIndex++,
                objectList: [
                    new LineElemsCreator.LabelData{
                Title = "Фамилия",
            },
            new LineElemsCreator.LabelData{
                Title =  _baseResponse.LastName,
            },
                ]
            );

            LineElemsCreator.AddLineElems(
                grid: _grid,
                rowIndex: rowIndex++,
                objectList: [
                    new LineElemsCreator.LabelData{
                Title = "Имя",
            },
            new LineElemsCreator.LabelData{
                Title = _baseResponse.FirstName
            },
                ]
            );

            LineElemsCreator.AddLineElems(
                grid: _grid,
                rowIndex: rowIndex++,
                objectList: [
                    new LineElemsCreator.LabelData{
                Title = "Отчество",
            },
            new LineElemsCreator.LabelData{
                Title = _baseResponse.Patronymic
            },
                ]
            );
        }
    }
}
