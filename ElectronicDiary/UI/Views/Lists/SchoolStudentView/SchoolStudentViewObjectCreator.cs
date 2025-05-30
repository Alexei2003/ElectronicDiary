﻿using System.Text.Json;

using ElectronicDiary.UI.Components.Elems;
using ElectronicDiary.UI.Components.Other;
using ElectronicDiary.UI.Views.Lists.General;
using ElectronicDiary.UI.Views.Lists.ParentSchoolStudentView.StudentWithParents;
using ElectronicDiary.UI.Views.Lists.UserView;
using ElectronicDiary.Web.Api.Educations;
using ElectronicDiary.Web.Api.Users;
using ElectronicDiary.Web.DTO.Requests.Users;
using ElectronicDiary.Web.DTO.Responses.Educations;
using ElectronicDiary.Web.DTO.Responses.Other;
using ElectronicDiary.Web.DTO.Responses.Users;

namespace ElectronicDiary.UI.Views.Lists.SchoolStudentView
{
    public class SchoolStudentViewObjectCreator : UserViewObjectCreator<SchoolStudentResponse, SchoolStudentRequest, SchoolStudentController>
    {
        protected override void CreateUI()
        {
            base.CreateUI();

            LineElemsCreator.AddLineElems(
                grid: _baseInfoGrid,
                rowIndex: _baseInfoGridRowIndex++,
                objectList: [
                    new LineElemsCreator.Data
                    {
                        Elem = BaseElemsCreator.CreateLabel( "Класс")
                    },
                    _componentState == AdminPageStatic.ComponentState.Read ?
                        new LineElemsCreator.Data
                        {
                            Elem = BaseElemsCreator.CreateLabel( _baseResponse.ClassRoom?.Name)
                        }
                    :
                        new LineElemsCreator.Data
                        {
                            Elem = BaseElemsCreator.CreateSearchPopupAsLabel(GetClasses(),
                                                    selectedIndex => _baseRequest.ClassRoomId = selectedIndex)
                        }
                ]
            );

            if (_componentState != AdminPageStatic.ComponentState.New)
            {
                var view = new StudenWithtParentsViewListCreator();
                var flag = _componentState != AdminPageStatic.ComponentState.Edit;
                var scrollView = view.Create([], [], _baseResponse.Id, flag);
                _infoStack.Add(scrollView);
            }
        }

        private List<TypeResponse> GetClasses()
        {
            var list = new List<TypeResponse>();

            Task.Run(async () =>
            {
                ClassResponse[]? arr = null;
                var controller = new ClassController();
                var response = await controller.GetAll(_objectParentId);
                if (!string.IsNullOrEmpty(response)) arr = JsonSerializer.Deserialize<ClassResponse[]>(response, PageConstants.JsonSerializerOptions) ?? [];

                foreach (var elem in arr ?? [])
                {
                    list.Add(new TypeResponse(elem.Id, elem?.Name));
                }
            });

            return list;
        }
    }
}
